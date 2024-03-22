using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMonsterState
{ 
    SPAWN,
    TRACE,
    ATTACK,
    HITTED,
    DIE,
    COUNT
}

public class MonsterController : BaseCharacterController
{
    public Transform PlayerTransform { get; private set; }
    public MonsterStat Stat { get; private set; }
    public float AwarenessRangeToTrace { get; private set; }
    public float AwarenessRangeToAttack { get; private set; }
    EMonsterState meCurrentState;
    StateMachine<MonsterController> mStateMachine;
    State<MonsterController>[] mStates;
    public override void Init()
    {
        base.Init();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Assert(PlayerTransform != null);
        Stat = gameObject.GetOrAddComponent<MonsterStat>();
        meCurrentState = EMonsterState.SPAWN;
        AwarenessRangeToTrace = 10f;
        AwarenessRangeToAttack = 2f;
    }

    
    void Update()
    {
        SetLookDir();
        mStateMachine.Excute();    
    }
    public void ChangeState(EMonsterState eChangingState)
    {
        meCurrentState = eChangingState;
        mStateMachine.ChangeState(mStates[(uint)eChangingState]);
        switch (eChangingState)
        {
            case EMonsterState.SPAWN:
                Animator.Play("Idle");
                break;
            case EMonsterState.TRACE:
                Animator.Play("Run");
                break;
            case EMonsterState.ATTACK:
                Animator.Play("Attack");
                break;
            case EMonsterState.HITTED:
                Animator.Play("Hitted");
                break;
            case EMonsterState.DIE:
                Animator.Play("Die");
                break;
            default:
                Debug.Assert(false, "You must Cheking Swich case");
                break;
        }
    }
    protected void SetLookDir()
    {
        Vector2 dir = PlayerTransform.position - transform.position;
        if (dir.x > 0)
        {
            mSpriteRenderer.flipX = false;
            ELookDir = define.ECharacterLookDir.RIGHT;
        }
        else
        {
            mSpriteRenderer.flipX = true;
            ELookDir = define.ECharacterLookDir.LEFT;
        }
    }
    protected override void initStates()
    {
        mStateMachine = new StateMachine<MonsterController>();
        mStates = new State<MonsterController>[(uint)EMonsterState.COUNT];
        mStates[(uint)EMonsterState.SPAWN] = new monster_states.Spawn();
        mStates[(uint)EMonsterState.TRACE] = new monster_states.Trace();
        mStates[(uint)EMonsterState.ATTACK] = new monster_states.Attack();
        mStates[(uint)EMonsterState.HITTED] = new monster_states.Hitted();
        mStates[(uint)EMonsterState.DIE] = new monster_states.Die();
        mStateMachine.Init(this, mStates[(uint)EMonsterState.SPAWN]);
    }

}
