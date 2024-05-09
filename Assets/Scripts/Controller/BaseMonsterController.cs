using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterState
{ 
    SPAWN,
    TRACE,
    ATTACK,
    HITTED_KNOCKBACK,
    HITTED,
    DIE,
    COUNT
}

public abstract class BaseMonsterController : BaseCharacterController
{
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
        AwarenessRangeToTrace = 10f;
        AwarenessRangeToAttack = 2f;
        NormalAttackRange = 1f;
        HealthBar = Utill.GetComponentInChildrenOrNull<UIMonsterHPBar>(gameObject, "MonsterHpBar");
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
        SetLookDir();
        _stateMachine.Excute();
    }
    public void HittedByPlayer()
    {
        if (Stat.HP > 0)
            ChangeState(EMonsterState.HITTED);
    }

    public void ChangeState(EMonsterState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
    }
    public void OnMonsterFootStep()
    {
        FootDustParticle.Play();
    }

    public void OnPlayerBlockSuccess()
    {
        ChangeState(EMonsterState.HITTED_KNOCKBACK);
    }
    protected void SetLookDir()
    {
        if (ECurrentState == EMonsterState.ATTACK ||
            ECurrentState == EMonsterState.HITTED ||
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
        _states[(uint)EMonsterState.DIE] = new monster_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EMonsterState.SPAWN]);
    }
    protected abstract void AssignAttackState<T>() where T : monster_states.BaseAttack;
    protected abstract void InitStat();
}
