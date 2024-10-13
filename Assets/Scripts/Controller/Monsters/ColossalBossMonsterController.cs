using define;
using JetBrains.Annotations;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EColossalBossPhase
{ 
    FIRST_PHASE, // FIST, SPIN, BURST
    SECOND_UNDER_50_PERCENT_HP_PHASE, // FIST, SPIN, BURFED_BURST,
    COUNT
}

public enum EColossalBossState
{
    WAKE,
    RUN,
    FIST_MELLE_ATTACK,
    SPIN_MELLE_ATTACK,
    BURST_MEELE_ATTACK,
    BURFED_BURST_ATTACK,
    BURF,
    DIE,
    COUNT
}
public class ColossalBossMonsterController : BaseMonsterController, IMelleAttackable, IZonePlayerDetetable
{
    static public UnityAction<EColossalBossState> ColossalChangeStateEventHandler;
    private readonly Vector2 COLOSSAL_KNOCKBACK_FORCE = new Vector2(7f, 7f);
    public EColossalBossPhase EColossalPhase { get; private set; } = EColossalBossPhase.FIRST_PHASE;
    public EColossalBossState ECurrentState { get; private set; }
    protected StateMachine<ColossalBossMonsterController> _stateMachine;
    protected State<ColossalBossMonsterController>[] _states;

    public bool IsPlayerInSpinAttackZone { get; set; }
    public bool IsPlayerInFistAttackZone { get; set; }
    public bool IsPlayerInBurstAttackZone { get; set; }

    public bool IsWake { get; private set; }

    #region ATTACK_LIGHTS
    [SerializeField] private ColossalBossAttackLightController _fistAttackLight;
    [SerializeField] private ColossalBossAttackLightController _spinAttackLight;
    [SerializeField] private ColossalBossAttackLightController _burstAttackLight;
    [SerializeField] private ColossalBossAttackLightController _burfedBurstLight;
    [SerializeField] private ColossalBossAttackLightController _burfLight;
    #endregion
    public override void Init()
    {
        base.Init();
        InitStates();
        //AllocateMelleAttackState();
        EMonsterType = EMonsterNames.BossColossal;
        Animator.enabled = false;
        #region ATTACK_ZONE_DETECTION_EVENT
        ColossalAttackZoneDetection.ColossalAttackZoneEnterEvnetHandler += OnPlayerEnterAttackZone;
        ColossalAttackZoneDetection.ColossalAttackZoneExitEvnetHandler += OnPlayerExitAttackZone;
        #endregion
        Stat.KnockbackForce = COLOSSAL_KNOCKBACK_FORCE;
    }

    private void OnDestroy()
    {
        ColossalAttackZoneDetection.ColossalAttackZoneEnterEvnetHandler -= OnPlayerEnterAttackZone;
        ColossalAttackZoneDetection.ColossalAttackZoneExitEvnetHandler -= OnPlayerExitAttackZone;
    }

    private void FixedUpdate()
    {
        if (!IsWake)
        {
            return;
        }
        _stateMachine.FixedExcute();
    }
    void Update()
    {
        if (!IsWake)
        {
            return;
        }
        _stateMachine.Excute();
    }

    public void ChangeState(EColossalBossState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
        ColossalChangeStateEventHandler?.Invoke(eChangingState);
    }
    public void AllocateMelleAttackState()
    {
        //_states[(uint)EColossalBossState.SPIN_MELLE_ATTACK] = new monster_states.BossColossalSpinAttack(this);
    }

    private void OnAnimFullyPlayed()
    {
        ((monster_states.BaseBossMonsterState)_states[(uint)ECurrentState]).OnAnimFullyPlayed();
    }

    #region ATTACK_ANIM_LIGHT_EVENT
    private void OnAttackAnimTurnOnLightTiming()
    {
        switch (ECurrentState)
        {
            case EColossalBossState.FIST_MELLE_ATTACK:
                _fistAttackLight.OnMonsterAttackStart();
                break;
            case EColossalBossState.SPIN_MELLE_ATTACK:
                _spinAttackLight.OnMonsterAttackStart();
                break;
            case EColossalBossState.BURST_MEELE_ATTACK:
                _burstAttackLight.OnMonsterAttackStart();
                break;
            case EColossalBossState.BURFED_BURST_ATTACK:
                _burfedBurstLight.OnMonsterAttackStart();
                break;
        }
    }
    private void OnAttackAnimTurnOffLightTiming()
    {
        switch (ECurrentState)
        {
            case EColossalBossState.FIST_MELLE_ATTACK:
                _fistAttackLight.OnMonsterAttackEnd();
                break;
            case EColossalBossState.SPIN_MELLE_ATTACK:
                _spinAttackLight.OnMonsterAttackEnd();
                break;
            case EColossalBossState.BURST_MEELE_ATTACK:
                _burstAttackLight.OnMonsterAttackEnd();
                break;
            case EColossalBossState.BURFED_BURST_ATTACK:
                _burfedBurstLight.OnMonsterAttackEnd();
                break;
        }
    }
    #endregion

    public void SetLookDir()
    {
        if (ECurrentState == EColossalBossState.WAKE ||
            ECurrentState == EColossalBossState.RUN)
        {
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
    }
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<ColossalBossMonsterController>();
        _states = new State<ColossalBossMonsterController>[(uint)EColossalBossState.COUNT];

        _states[(uint)EColossalBossState.WAKE]                  = new monster_states.BossColossalWake(this);
        _states[(uint)EColossalBossState.RUN]                   = new monster_states.BossColossalRun(this);
        _states[(uint)EColossalBossState.SPIN_MELLE_ATTACK]     = new monster_states.BossColossalSpinAttack(this);
        _states[(uint)EColossalBossState.FIST_MELLE_ATTACK]     = new monster_states.BossColossalFistAttack(this);
        _states[(uint)EColossalBossState.BURST_MEELE_ATTACK]    = new monster_states.BossColossalBurstAttack(this);
        _states[(uint)EColossalBossState.BURFED_BURST_ATTACK]   = new monster_states.BossColossalBurfedBurstAttack(this);
        _states[(uint)EColossalBossState.BURF]                  = new monster_states.BossColossalBurf(this);
        _states[(uint)EColossalBossState.DIE]                   = new monster_states.BossColossalDie(this);
        
        //_stateMachine.Init(this, _states[(uint)EColossalBossState.WAKE]);
    }


    public override void DamagedFromPlayer(ECharacterLookDir eLookDir, int damage, EPlayerNoramlAttackType eAttackType)
    {
        if (ECurrentState != EColossalBossState.DIE && 
            ECurrentState != EColossalBossState.WAKE &&
            ECurrentState != EColossalBossState.BURF)
        {
            DecreasHpAndInvokeHitEvents(damage, eAttackType);
            if (Stat.HP <= 0)
            {
                ChangeState(EColossalBossState.DIE);
            }
            else
            {
                // Change To Second Phase.
                if (Stat.HP < (int)(Stat.MaxHP * 0.5f) && 
                    EColossalPhase != EColossalBossPhase.SECOND_UNDER_50_PERCENT_HP_PHASE)
                {
                    ChangeState(EColossalBossState.BURF);
                    EColossalPhase = EColossalBossPhase.SECOND_UNDER_50_PERCENT_HP_PHASE;
                }
            }
        }
    }

    private void OnBurfAnimTurnOnLightTiming()
    {
        _burfLight.OnMonsterAttackStart();
    }

    private void OnBurfAnimTurnOffLightTiming()
    {
        _burfLight.OnMonsterAttackEnd();
    }

    #region ATTACK_DETECTION_ZONE_EVENT
    private void OnPlayerEnterAttackZone(EColossalAttackType eType)
    {
        switch (eType)
        {
            case EColossalAttackType.FIST:
                IsPlayerInFistAttackZone = true;
                break;
            case EColossalAttackType.SPIN:
                IsPlayerInSpinAttackZone = true;
                break;
            case EColossalAttackType.BURST:
                IsPlayerInBurstAttackZone = true;
                break;
        }
    }

    private void OnPlayerExitAttackZone(EColossalAttackType eType)
    {
        switch (eType)
        {
            case EColossalAttackType.FIST:
                IsPlayerInFistAttackZone = false;
                break;
            case EColossalAttackType.SPIN:
                IsPlayerInSpinAttackZone = false;
                break;
            case EColossalAttackType.BURST:
                IsPlayerInBurstAttackZone = false;
                break;
        }
    }
    #endregion
    public override void InitStat()
    {
        Stat.InitBasicStat(define.EMonsterNames.BossColossal);
    }


    #region OVERRIDE_ABSTRACT_HITTED_PLAYER_SPECIAL_ATTACK
    public override void OnPlayerBlockSuccess()
    {
        HittedByNormalAttackNoArgsEventHandler?.Invoke();
    }

    public override void OnHittedByPlayerSkill(data.SkillInfo skillInfo)
    {
        // TODO : ColossalBoss 플레이어 스킬에 어떻게 반응할지 결정해주어야 한다.
        ESkillType eType = (ESkillType)skillInfo.id;
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
                //ChangeState(ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS);
                break;
            case ESkillType.Spawn_Shooter_LV1:
                //ChangeState(ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB);
                break;
            case ESkillType.Cast_BlackFlame_LV1:
                DamagedFromPlayer(ELookDir, Managers.Data.SkillInfoDict[skillInfo.id].damage, EPlayerNoramlAttackType.Attack_3);
                break;
            case ESkillType.Cast_SwordStrike_LV1:
                DamagedFromPlayer(ELookDir, Managers.Data.SkillInfoDict[skillInfo.id].damage, EPlayerNoramlAttackType.Attack_3);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }



    public void OnPlayerEnter(Collider2D collision)
    {
        IsWake = true;
        Animator.enabled = true;
        _stateMachine.Init(this, _states[(uint)EColossalBossState.WAKE]);
    }

    public void OnPlayerStay(Collider2D collision)
    {
    }

    public void OnPlayerExit(Collider2D collision)
    {
        GameObject.Find("ColossalBossWakeZone")?.GetComponent<InteractBox>().Unactive();
    }


    #endregion
}
