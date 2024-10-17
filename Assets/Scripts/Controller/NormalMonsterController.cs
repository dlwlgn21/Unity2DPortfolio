using data;
using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public enum ENormalMonsterState
{
    Idle,
    Trace,
    MelleAttack,
    LaunchAttack,
    HitByPlayerBlockSucces,
    HitByPlayerSkillParallysis,
    HitByPlayerSkillKnockbackBoom,
    Die,
    Count
}

public enum ENormalMonsterAttackType
{
    MelleAttack,
    LaunchAttack,
    BothAttack,
    Count
}
public abstract class NormalMonsterController : BaseMonsterController, IAttackZoneDetectable, ITraceZoneDetectable
{
    public static UnityAction<ENormalMonsterState> MonsterChangeStateEventHandler;
    public ENormalMonsterAttackType EMonsterAttackType { get; protected set; }
    public ENormalMonsterState ECurrentState { get; private set; }
    protected StateMachine<NormalMonsterController> _stateMachine;
    protected State<NormalMonsterController>[] _states;
    public bool IsPlayerInAttackZone { get; private set; } = false;
    public bool IsPlayerInTraceZone { get; private set; } = false;

    UI_WSNormalMonsterPopupStatusController _statusTextController;
    protected LightController _attackLightController;
    protected LightController _dieController;

    Light2D _headLight;
    Light2D _weaponLightOrNull;
    Light2D _backLightOrNull;

    MonsterBloodAnimController _bloodAnimationController;


    private void FixedUpdate()
    {
        _stateMachine.FixedExcute();
    }
    void Update()
    {
        _stateMachine.Excute();
    }

    #region Public
    public override void Init()
    {
        base.Init();
        if (_statusTextController == null)
        {
            _statusTextController = Utill.GetFirstComponentInChildrenOrNull<UI_WSNormalMonsterPopupStatusController>(gameObject);
            _attackLightController = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "AttackLight");
            _dieController = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "DieLight");
            _bloodAnimationController = Utill.GetFirstComponentInChildrenOrNull<MonsterBloodAnimController>(gameObject);

            _headLight = transform.Find("HeadLight").gameObject?.GetComponent<Light2D>();
            Transform trnasformOrNull = transform.Find("WeaponLight");
            if (trnasformOrNull != null)
                _weaponLightOrNull = trnasformOrNull.gameObject.GetComponent<Light2D>();
            trnasformOrNull = transform.Find("BackLight");
            if (trnasformOrNull != null)
                _backLightOrNull = trnasformOrNull.gameObject.GetComponent<Light2D>();
            SetLightControllersTurnOffTimeInSec();
        }
    }
    public void InitForRespawn()
    {
        InitStat();
        //Init();
        SetEnableTrueAllLightsWithoutDieLight();
        HealthBar.InitForRespawn();
        RigidBody.WakeUp();
        ChangeState(ENormalMonsterState.Idle);
    }
    public void ChangeState(ENormalMonsterState eChangingState)
    {
        if (_parallysisCoroutineOrNull != null)
        {
            StopCoroutine(_parallysisCoroutineOrNull);
            _parallysisCoroutineOrNull = null;
        }
        ECurrentState = eChangingState;
        if (ECurrentState == ENormalMonsterState.Die)
            SetEnableFalseAllLightsWithoutDieLight();
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
        _statusTextController.ShowMonsterStatus(eChangingState);
        MonsterChangeStateEventHandler?.Invoke(eChangingState);
    }

    public void SetLookDir()
    {
        if (ECurrentState == ENormalMonsterState.MelleAttack ||
            ECurrentState == ENormalMonsterState.HitByPlayerBlockSucces ||
            ECurrentState == ENormalMonsterState.HitByPlayerSkillParallysis ||
            ECurrentState == ENormalMonsterState.Die)
        {
            return;
        }
        if (_pc == null)
        {
            _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            PlayerTransform = _pc.transform;
        }
        Vector2 dir = PlayerTransform.position - transform.position;
        if (dir.x > 0)
        {
            ELookDir = define.ECharacterLookDir.Right;
            transform.localRotation = Quaternion.Euler(RIGHT_ROT_VECTOR);
        }
        else
        {
            ELookDir = define.ECharacterLookDir.Left;
            transform.localRotation = Quaternion.Euler(LEFT_ROT_VECTOR);
        }
    }
    #region TraceOrAttackZone
    public void OnEnterAttackZone()
    { IsPlayerInAttackZone = true; }
    public void OnExitAttackZone()
    { IsPlayerInAttackZone = false; }
    public void OnTraceZoneEnter()
    { IsPlayerInTraceZone = true; }
    public void OnTraceZoneExit()
    { IsPlayerInTraceZone = false; }
    #endregion

    #region HittedByPlayer
    public override void DamagedFromPlayer(ECharacterLookDir eLookDir, int damage, EPlayerNoramlAttackType eAttackType)
    {
        if (ECurrentState != ENormalMonsterState.Die)
        {
            DecreasHpAndInvokeHitEvents(damage, eAttackType);
            _bloodAnimationController.PlayerBloodAnimation(transform.position, ELookDir, eAttackType);
            ((monster_states.BaseMonsterState)_states[(int)ECurrentState]).MakeSlow();
            switch (eAttackType)
            {
                case EPlayerNoramlAttackType.Attack_1:
                    AddKnockbackForce(PlayerController.NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE);
                    break;
                case EPlayerNoramlAttackType.Attack_2:
                    AddKnockbackForce(PlayerController.NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE);
                    break;
                case EPlayerNoramlAttackType.Attack_3:
                case EPlayerNoramlAttackType.BackAttack:
                    AddKnockbackForce(PlayerController.NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE * PlayerController.NORMAL_ATTACK_3_FORCE_COEFF);
                    break;
                default:
                    break;
            }
            if (Stat.HP <= 0)
                ChangeState(ENormalMonsterState.Die);
        }
    }
    public override void OnPlayerBlockSuccess()
    {
        ChangeState(ENormalMonsterState.HitByPlayerBlockSucces);
        AddKnockbackForce(new Vector2(5f, 3f));
    }
    public override void OnHittedByPlayerSkill(data.SkillInfo skillInfo)
    {
        ESkillType eType = (ESkillType)skillInfo.id;
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
            case ESkillType.Spawn_Reaper_LV2:
            case ESkillType.Spawn_Reaper_LV3:
                ChangeState(ENormalMonsterState.HitByPlayerSkillParallysis);
                _parallysisCoroutineOrNull = StartCoroutine(PlayHitAnimForSeconds(skillInfo.parallysisTime));
                break;
            case ESkillType.Spawn_Shooter_LV1:
            case ESkillType.Spawn_Shooter_LV2:
            case ESkillType.Spawn_Shooter_LV3:
                RigidBody.velocity = Vector3.zero;
                AddKnockbackForce(new Vector2(skillInfo.knockbackForceX, skillInfo.knockbackForceY));
                break;
            case ESkillType.Cast_BlackFlame_LV1:
            case ESkillType.Cast_BlackFlame_LV2:
            case ESkillType.Cast_BlackFlame_LV3:
                DamagedFromPlayer(ELookDir, Managers.Data.SkillInfoDict[skillInfo.id].damage, EPlayerNoramlAttackType.Attack_3);
                break;
            case ESkillType.Cast_SwordStrike_LV1:
            case ESkillType.Cast_SwordStrike_LV2:
            case ESkillType.Cast_SwordStrike_LV3:
                DamagedFromPlayer(ELookDir, Managers.Data.SkillInfoDict[skillInfo.id].damage, EPlayerNoramlAttackType.Attack_3);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }


    #endregion
    #endregion


    #region Protected
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<NormalMonsterController>();
        _states = new State<NormalMonsterController>[(uint)ENormalMonsterState.Count];
        _states[(uint)ENormalMonsterState.Idle] = new monster_states.Idle(this);
        _states[(uint)ENormalMonsterState.Trace] = new monster_states.Trace(this);
        _states[(uint)ENormalMonsterState.HitByPlayerBlockSucces] = new monster_states.HittedKnockbackByBlockSuccess(this);
        _states[(uint)ENormalMonsterState.HitByPlayerSkillParallysis] = new monster_states.HittedParalysis(this);
        _states[(uint)ENormalMonsterState.Die] = new monster_states.Die(this);
        _stateMachine.Init(this, _states[(uint)ENormalMonsterState.Idle]);
    }

    protected void InstantiateDeadBody(string prefabPath)
    {
        GameObject go = Managers.Resources.Instantiate<GameObject>(prefabPath);
        if (ELookDir == ECharacterLookDir.Left)
            go.GetComponent<SpriteRenderer>().flipX = true;
        go.transform.position = transform.position;
    }

    #endregion
    #region Private
    #region ANIM_CALLBACK

    void OnTurnOnDieLightTiming()
    {
        _headLight.enabled = false;
        _dieController.TurnOnLight();
    }
    void OnTurnOffDieLightTiming()
    {
        _dieController.TurnOffLightGradually();
    }
    void OnAttackAnimTurnOnLightTiming()
    {
        _attackLightController.TurnOnLight();
    }
    void OnAttackAnimTurnOffLightTiming()
    {
        _attackLightController.TurnOffLightGradually();
    }
    void OnMonsterFootStep() 
    { FootDustParticle.Play(); }

    void OnAnimFullyPlayed()
    { ((monster_states.BaseMonsterState)_states[(uint)ECurrentState]).OnAnimFullyPlayed(); }
    #endregion

    void OnDrawGizmosSelected()
    {

    }


    IEnumerator PlayHitAnimForSeconds(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        ChangeState(ENormalMonsterState.Idle);
    }

    void SetEnableFalseAllLightsWithoutDieLight()
    {
        _headLight.enabled = false;
        _attackLightController.ForceToStopCoroutineAndTurnOffLight();
        if (_weaponLightOrNull != null)
            _weaponLightOrNull.enabled = false;
        if (_backLightOrNull != null)
            _backLightOrNull.enabled = false;
    }

    void SetEnableTrueAllLightsWithoutDieLight()
    {
        _headLight.gameObject.SetActive(true);
        _headLight.enabled = true;
        if (_weaponLightOrNull != null)
        {
            _weaponLightOrNull.gameObject.SetActive(true);
            _weaponLightOrNull.enabled = true;
        }
        if (_backLightOrNull != null)
        {
            _backLightOrNull.gameObject.SetActive(true);
            _backLightOrNull.enabled = true;
        }
        _attackLightController.gameObject.SetActive(true);
    }
    #endregion


}