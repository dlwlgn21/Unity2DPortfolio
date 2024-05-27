using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum EMonsterState
{ 
    SPAWN,
    TRACE,
    ATTACK,
    HITTED_KNOCKBACK,
    HITTED_PARALYSIS,
    HITTED_KNOCKBACK_BOMB,
    HITTED,
    DIE,
    COUNT
}

public abstract class BaseMonsterController : BaseCharacterController
{
    public UIWSMonsterHpBar HealthBar { get; set; }
    public Transform PlayerTransform { get; private set; }
    public EMonsterNames MonsterType { get; protected set; }
    public MonsterStat Stat { get; protected set; }
    public float AwarenessRangeToTrace { get; private set; }
    public float AwarenessRangeToAttack { get; private set; }
    public EMonsterState ECurrentState { get; private set; }
    protected StateMachine<BaseMonsterController> _stateMachine;
    protected State<BaseMonsterController>[] _states;

    public override void Init()
    {
        base.Init();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Assert(PlayerTransform != null);
        Stat = gameObject.GetOrAddComponent<MonsterStat>();
        ECurrentState = EMonsterState.SPAWN;

        // TODO : 여기 하드코딩 되어 있는 수치들 나중에 다 MonsterData로 빼서 읽어와야 함.
        AwarenessRangeToTrace = 10f;
        AwarenessRangeToAttack = 2f;
        NormalAttackRange = 1f;

        HealthBar = Utill.GetComponentInChildrenOrNull<UIWSMonsterHpBar>(gameObject, "UIWSMonsterHpBar");
    }
    public void InitStatForRespawn()
    {
        InitStat();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedExcute();
    }
    void Update()
    {
        _stateMachine.Excute();
    }
    public void HittedByPlayerNormalAttack()
    {
        //  ECurrentState != EMonsterState.DIE DieState에서 플레이어 공격시에 다시 HitState로 변환되는 경우가 간혹 있었음. 그걸 막기위한 조치
        if (Stat.HP > 0 && ECurrentState != EMonsterState.DIE)
        {
            ChangeState(EMonsterState.HITTED);
        }
    }

    #region CHANGE_TO_HITTED_CALLED_BY_PLAYER
    public void OnPlayerBlockSuccess()          { ChangeState(EMonsterState.HITTED_KNOCKBACK); }
    public void HittedByPlayerKnockbackBomb()   { ChangeState(EMonsterState.HITTED_KNOCKBACK_BOMB); }
    public void HittedByPlayerSpawnReaper()     { ChangeState(EMonsterState.HITTED_PARALYSIS); }
    #endregion

    public void OnMonsterFootStep()             { FootDustParticle.Play(); }

    public void ChangeState(EMonsterState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
    }


    public void OnValidHittedAnimEnd()
    {
        switch (ECurrentState)
        {
            case EMonsterState.HITTED:
                ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED]).OnHittedAnimFullyPlayed();
                return;
            case EMonsterState.HITTED_KNOCKBACK:
                ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED_KNOCKBACK]).OnHittedAnimFullyPlayed();
                return;
            case EMonsterState.HITTED_PARALYSIS:
                ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED_PARALYSIS]).OnHittedAnimFullyPlayed();
                return;
            case EMonsterState.HITTED_KNOCKBACK_BOMB:
                ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED_KNOCKBACK_BOMB]).OnHittedAnimFullyPlayed();
                return;
        }
        Debug.Assert(false);
    }


    public void SetLookDir()
    {
        if (ECurrentState == EMonsterState.ATTACK ||
            ECurrentState == EMonsterState.HITTED ||
            ECurrentState == EMonsterState.HITTED_KNOCKBACK ||
            ECurrentState == EMonsterState.HITTED_PARALYSIS ||
            ECurrentState == EMonsterState.DIE)
            return;

        Vector2 dir = PlayerTransform.position - transform.position;
        if (dir.x > 0)
        {
            SpriteRenderer.flipX = false;
            ELookDir = define.ECharacterLookDir.RIGHT;
            NormalAttackPoint.localPosition = CachedAttackPointLocalRightPos;
        }
        else
        {
            SpriteRenderer.flipX = true;
            ELookDir = define.ECharacterLookDir.LEFT;
            NormalAttackPoint.localPosition = CachedAttackPointLocalLeftPos;
        }
    }
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<BaseMonsterController>();
        _states = new State<BaseMonsterController>[(uint)EMonsterState.COUNT];
        _states[(uint)EMonsterState.SPAWN] = new monster_states.Spawn(this);
        _states[(uint)EMonsterState.TRACE] = new monster_states.Trace(this);
        _states[(uint)EMonsterState.HITTED] = new monster_states.Hitted(this);
        _states[(uint)EMonsterState.HITTED_KNOCKBACK] = new monster_states.HittedKnockback(this);
        _states[(uint)EMonsterState.HITTED_PARALYSIS] = new monster_states.HittedParalysis(this);
        _states[(uint)EMonsterState.HITTED_KNOCKBACK_BOMB] = new monster_states.HittedKnockbackBomb(this);
        _states[(uint)EMonsterState.DIE] = new monster_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EMonsterState.SPAWN]);
    }
    protected abstract void AssignAttackState<T>() where T : monster_states.BaseAttack;
    protected abstract void InitStat();
}
