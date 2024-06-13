using define;
using DG.Tweening;
using monster_states;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.InspectorCurveEditor;

public enum EMonsterState
{ 
    IDLE,
    TRACE,
    ATTACK,
    HITTED_KNOCKBACK,
    HITTED_PARALYSIS,
    HITTED_KNOCKBACK_BOMB,
    DIE,
    COUNT
}

public abstract class BaseMonsterController : BaseCharacterController
{
    private readonly static Vector3 sLeftRotationVector = new Vector3(0f, 180f, 0f);
    private readonly static Vector3 sRightRotationVector = new Vector3(0f, 0f, 0f);
    public UIWSMonsterHpBar HealthBar { get; set; }
    public SpawnEffectController SpawnEffectController { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public EMonsterNames MonsterType { get; protected set; }
    public MonsterStat Stat { get; protected set; }
    public float AwarenessRangeToTrace { get; private set; }
    public float AwarenessRangeToAttack { get; protected set; }
    public EMonsterState ECurrentState { get; private set; }
    public Vector3 OriginalHpBarScale { get; private set; }

    protected StateMachine<BaseMonsterController> _stateMachine;
    protected State<BaseMonsterController>[] _states;
    protected PlayerController _pc;

    public const string HITTED_STATUS_TEXT_STRING = "피격!";


    // Added part for DamageFlash 6.8 day
    public DamageFlasher DamageFlasher { get; private set; }
    // Added part for BloodEffect 6.9 day
    public BloodEffectController BloodEffectController { get; private set; }

    public override void Init()
    {
        if (HealthBar == null)
        {
            base.Init();
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Assert(PlayerTransform != null);
            _pc = PlayerTransform.gameObject.GetComponent<PlayerController>();
            Stat = gameObject.GetOrAddComponent<MonsterStat>();
            ECurrentState = EMonsterState.IDLE;

            // TODO : 여기 하드코딩 되어 있는 수치들 나중에 다 MonsterData로 빼서 읽어와야 함.
            AwarenessRangeToTrace = 10f;
            NormalAttackRange = 2f;

            HealthBar = Utill.GetComponentInChildrenOrNull<UIWSMonsterHpBar>(gameObject, "UIWSMonsterHpBar");
            OriginalHpBarScale = HealthBar.transform.localScale;

            SpawnEffectController = Utill.GetComponentInChildrenOrNull<SpawnEffectController>(gameObject, "MonSpawnEffect");
            SpawnEffectController.gameObject.SetActive(false);

            // Addpart For DamageFlash 6.8 day
            DamageFlasher = GetComponent<DamageFlasher>();
            // Addpart For BloodEffectController 6.9 day
            BloodEffectController = Utill.GetComponentInChildrenOrNull<BloodEffectController>(gameObject, "BloodEffect");

            PlayerNormalAttack.PlayerNormalAttackEventHandler += HittedByPlayerNormalAttack;
        }
        InitStat();
        HealthBar.transform.localScale = OriginalHpBarScale;

    }
    public void InitForRespawn()
    {
        InitStat();
        Init();
        HealthBar.Init();
        ChangeState(EMonsterState.IDLE);
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedExcute();
    }
    void Update()
    {
        _stateMachine.Excute();
    }


    #region HITTED_BY_PLAYER_NORMAL_ATTACK
    public static void HittedByPlayerNormalAttack(PlayerController pc, EPlayerNoramlAttackType eAttackType, BaseMonsterController instance)
    {
        //  DieState에서 플레이어 공격시에 다시 HitState로 변환되는 경우가 간혹 있었음. 그걸 막기위한 조치
        //if (ECurrentState != EMonsterState.DIE)
        //{
        //    ((BaseMonsterState)_states[(int)ECurrentState]).OnHittedByPlayerNormalAttack(pc, eAttackType);
            
        //}
        if (instance.ECurrentState != EMonsterState.DIE)
        {
            BaseMonsterState currState = (BaseMonsterState)instance._states[(int)instance.ECurrentState];
            if (currState != null)
            {
                currState.OnHittedByPlayerNormalAttack(pc, eAttackType);
            }
        }
    }
    #endregion

    #region CHANGE_TO_HITTED_CALLED_BY_PLAYER
    public void OnPlayerBlockSuccess()          { ChangeState(EMonsterState.HITTED_KNOCKBACK); }
    public void HittedByPlayerKnockbackBomb()   { ChangeState(EMonsterState.HITTED_KNOCKBACK_BOMB); }
    public void HittedByPlayerSpawnReaper()     { ChangeState(EMonsterState.HITTED_PARALYSIS); }
    #endregion


    #region ANIM_CALLBACK

    public void OnAttackAnimFullyPlayed()       { ((monster_states.Attack)_states[(uint)EMonsterState.ATTACK]).OnAttackAnimFullyPlayed(); }
    public void OnMonsterDieAnimFullyPlayed()   { ((monster_states.Die)_states[(uint)EMonsterState.DIE]).OnDieAnimFullyPlayed(); }
    public void OnMonsterFootStep()             { FootDustParticle.Play(); }

    #endregion

    public void ChangeState(EMonsterState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
    }


    public void OnHittedAnimFullyPlayed()
    {
        switch (ECurrentState)
        {
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
    }


    public void SetLookDir()
    {
        if (ECurrentState == EMonsterState.ATTACK ||
            ECurrentState == EMonsterState.HITTED_KNOCKBACK ||
            ECurrentState == EMonsterState.HITTED_PARALYSIS ||
            ECurrentState == EMonsterState.DIE)
        {
            return;
        }
        Vector2 dir = PlayerTransform.position - transform.position;
        if (dir.x > 0)
        {
            ELookDir = define.ECharacterLookDir.RIGHT;
            transform.localRotation = Quaternion.Euler(sRightRotationVector);
        }
        else
        {
            ELookDir = define.ECharacterLookDir.LEFT;
            transform.localRotation = Quaternion.Euler(sLeftRotationVector);
        }
    }
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<BaseMonsterController>();
        _states = new State<BaseMonsterController>[(uint)EMonsterState.COUNT];
        _states[(uint)EMonsterState.IDLE] = new monster_states.Idle(this);
        _states[(uint)EMonsterState.TRACE] = new monster_states.Trace(this);
        _states[(uint)EMonsterState.ATTACK] = new monster_states.Attack(this);
        _states[(uint)EMonsterState.HITTED_KNOCKBACK] = new monster_states.HittedKnockbackByBlockSuccess(this);
        _states[(uint)EMonsterState.HITTED_PARALYSIS] = new monster_states.HittedParalysis(this);
        _states[(uint)EMonsterState.HITTED_KNOCKBACK_BOMB] = new monster_states.HittedKnockbackByBomb(this);
        _states[(uint)EMonsterState.DIE] = new monster_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EMonsterState.IDLE]);
    }
    protected abstract void InitStat();

    void OnDrawGizmosSelected()
    {
        if (NormalAttackPoint == null)
            return;
        Gizmos.DrawWireSphere(NormalAttackPoint.position, NormalAttackRange);
    }


}
