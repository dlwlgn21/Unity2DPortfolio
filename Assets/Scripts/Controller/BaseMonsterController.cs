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
    public MonsterStat Stat { get; protected set; }
    public float AwarenessRangeToTrace { get; private set; }
    public float AwarenessRangeToAttack { get; private set; }
    protected EMonsterState meCurrentState;
    protected StateMachine<BaseMonsterController> mStateMachine;
    protected State<BaseMonsterController>[] mStates;

    public override void Init()
    {
        base.Init();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Assert(PlayerTransform != null);
        Stat = gameObject.GetOrAddComponent<MonsterStat>();
        meCurrentState = EMonsterState.SPAWN;
        AwarenessRangeToTrace = 10f;
        AwarenessRangeToAttack = 2f;
        NormalAttackRange = 1f;
        HealthBar = Utill.GetComponentInChildrenOrNull<UIMonsterHPBar>(gameObject, "MonsterHpBar");
    }
    private void FixedUpdate()
    {
        mStateMachine.FixedExcute();
    }
    void Update()
    {
        SetLookDir();
        mStateMachine.Excute();
    }
    public void HittedByPlayer()
    {
        if (Stat.HP > 0)
            ChangeState(EMonsterState.HITTED);
    }

    public void ChangeState(EMonsterState eChangingState)
    {
        meCurrentState = eChangingState;
        mStateMachine.ChangeState(mStates[(uint)eChangingState]);
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
        if (meCurrentState == EMonsterState.ATTACK ||
            meCurrentState == EMonsterState.HITTED ||
            meCurrentState == EMonsterState.DIE)
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
        mStateMachine = new StateMachine<BaseMonsterController>();
        mStates = new State<BaseMonsterController>[(uint)EMonsterState.COUNT];
        mStates[(uint)EMonsterState.SPAWN] = new monster_states.Spawn(this);
        mStates[(uint)EMonsterState.TRACE] = new monster_states.Trace(this);
        mStates[(uint)EMonsterState.HITTED] = new monster_states.Hitted(this);
        mStates[(uint)EMonsterState.HITTED_KNOCKBACK] = new monster_states.HittedKnockback(this);
        mStates[(uint)EMonsterState.DIE] = new monster_states.Die(this);
        mStateMachine.Init(this, mStates[(uint)EMonsterState.SPAWN]);
    }
    protected abstract void AssignAttackState<T>() where T : monster_states.BaseAttack;
    protected abstract void InitStat();
}