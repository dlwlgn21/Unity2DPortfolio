using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum ENormalMonsterState
{
    IDLE,
    TRACE,
    MELLE_ATTACK,
    LAUNCH_ATTACK,
    HITTED_BY_PLAYER_BLOCK_SUCCESS,
    HITTED_BY_PLAYER_SKILL_PARALYSIS,
    HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB,
    DIE,
    COUNT
}

public enum ENormalMonsterAttackType
{
    MELLE_ATTACK,
    LAUNCH_ATTACK,
    BOTH_ATTACK,
    COUNT
}
public abstract class NormalMonsterController : BaseMonsterController, IAttackZoneDetectable, ITraceZoneDetectable
{
    public static UnityAction<ENormalMonsterState> MonsterChangeStateEventHandler;
    public static UnityAction MonsterAttackStartEventHandler;
    public static UnityAction MonsterAttackEndEventHandler;
    public ENormalMonsterAttackType EMonsterAttackType { get; protected set; }
    public ENormalMonsterState ECurrentState { get; private set; }

    protected StateMachine<NormalMonsterController> _stateMachine;
    protected State<NormalMonsterController>[] _states;
    public bool IsPlayerInAttackZone { get; private set; } = false;
    public bool IsPlayerInTraceZone { get; private set; } = false;
    public override void Init()
    {
        base.Init();
    }
    public void InitForRespawn()
    {
        InitStat();
        Init();
        HealthBar.OnMonsterInit();
        RigidBody.WakeUp();
        ChangeState(ENormalMonsterState.IDLE);
    }
    private void FixedUpdate()
    {
        _stateMachine.FixedExcute();
    }
    void Update()
    {
        _stateMachine.Excute();
    }

    public void ChangeState(ENormalMonsterState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
        MonsterChangeStateEventHandler?.Invoke(eChangingState);
    }

    public void SetLookDir()
    {
        if (ECurrentState == ENormalMonsterState.MELLE_ATTACK ||
            ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS ||
            ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS ||
            ECurrentState == ENormalMonsterState.DIE)
        {
            return;
        }
        Vector2 dir = PlayerTransform.position - transform.position;
        if (dir.x > 0)
        {
            ELookDir = define.ECharacterLookDir.RIGHT;
            transform.localRotation = Quaternion.Euler(RIGHT_ROT_VECTOR);
        }
        else
        {
            ELookDir = define.ECharacterLookDir.LEFT;
            transform.localRotation = Quaternion.Euler(LEFT_ROT_VECTOR);
        }
    }
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<NormalMonsterController>();
        _states = new State<NormalMonsterController>[(uint)ENormalMonsterState.COUNT];
        _states[(uint)ENormalMonsterState.IDLE] = new monster_states.Idle(this);
        _states[(uint)ENormalMonsterState.TRACE] = new monster_states.Trace(this);
        _states[(uint)ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS] = new monster_states.HittedKnockbackByBlockSuccess(this);
        _states[(uint)ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS] = new monster_states.HittedParalysis(this);
        _states[(uint)ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB] = new monster_states.HittedKnockbackByBomb(this);
        _states[(uint)ENormalMonsterState.DIE] = new monster_states.Die(this);
        _stateMachine.Init(this, _states[(uint)ENormalMonsterState.IDLE]);
    }


    public override void OnHittedByPlayerNormalAttack(ECharacterLookDir eLookDir, int damage, EPlayerNoramlAttackType eAttackType)
    {
        if (ECurrentState != ENormalMonsterState.DIE)
        {
            IsHittedByPlayerNormalAttack = true;
            DecreasHpAndInvokeHitEvents(damage, eAttackType);
            #region PROCESS_BACK_ATTACK_OR_THIRD_ATTACK
            if (eAttackType == EPlayerNoramlAttackType.BACK_ATTACK || eAttackType == EPlayerNoramlAttackType.ATTACK_3)
            {
                BigAttackEventHandler?.Invoke();
                Managers.TimeManager.OnMonsterHittedByPlayerNormalAttack();
            }
            #endregion
            ((monster_states.BaseMonsterState)_states[(int)ECurrentState]).OnHittedByPlayerNormalAttack(eLookDir, damage, eAttackType);
            IsHittedByPlayerNormalAttack = false;
        }
    }
    public override void OnPlayerBlockSuccess() 
    { ChangeState(ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS); }
    public override void OnHittedByPlayerKnockbackBomb() 
    { ChangeState(ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB); }
    public override void OnHittedByPlayerSpawnReaper() 
    { ChangeState(ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS); }

    public override void OnDie()
    {
        Animator.speed = 1f;
    }

    #region ANIM_CALLBACK
    private void OnMonsterFootStep() 
    { FootDustParticle.Play(); }

    private void OnAnimFullyPlayed()
    { ((monster_states.BaseMonsterState)_states[(uint)ECurrentState]).OnAnimFullyPlayed(); }
    #endregion

    void OnDrawGizmosSelected()
    {

    }

    public void OnEnterAttackZone()
    { IsPlayerInAttackZone = true; }
    public void OnExitAttackZone()
    { IsPlayerInAttackZone = false; }
    public void OnTraceZoneEnter()
    { IsPlayerInTraceZone = true; }
    public void OnTraceZoneExit()
    { IsPlayerInTraceZone = false; }

    private void OnAttackAnimTurnOnLightTiming()
    {
        MonsterAttackStartEventHandler?.Invoke();
    }
    private void OnAttackAnimTurnOffLightTiming()
    {
        MonsterAttackEndEventHandler?.Invoke();
    }


}
