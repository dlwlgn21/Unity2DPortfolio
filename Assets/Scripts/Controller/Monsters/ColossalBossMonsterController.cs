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
public class ColossalBossMonsterController : BaseMonsterController, IMelleAttackable
{
    static public UnityAction<EColossalBossState> ColossalChangeStateEventHandler;

    public EColossalBossPhase EColossalPhase { get; private set; } = EColossalBossPhase.FIRST_PHASE;
    public EColossalBossState ECurrentState { get; private set; }
    protected StateMachine<ColossalBossMonsterController> _stateMachine;
    protected State<ColossalBossMonsterController>[] _states;

    public bool IsPlayerInSpinAttackZone { get; set; }
    public bool IsPlayerInFistAttackZone { get; set; }
    public bool IsPlayerInBurstAttackZone { get; set; }


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

        #region ATTACK_ZONE_DETECTION_EVENT
        ColossalAttackZoneDetection.ColossalAttackZoneEnterEvnetHandler += OnPlayerEnterAttackZone;
        ColossalAttackZoneDetection.ColossalAttackZoneExitEvnetHandler += OnPlayerExitAttackZone;
        #endregion
    }

    private void OnDestroy()
    {
        ColossalAttackZoneDetection.ColossalAttackZoneEnterEvnetHandler -= OnPlayerEnterAttackZone;
        ColossalAttackZoneDetection.ColossalAttackZoneExitEvnetHandler -= OnPlayerExitAttackZone;
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedExcute();
    }
    void Update()
    {
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

    protected override void OnAnimFullyPlayed()
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
                ELookDir = define.ECharacterLookDir.RIGHT;
                transform.localRotation = Quaternion.Euler(RIGHT_ROT_VECTOR);
            }
            else
            {
                ELookDir = define.ECharacterLookDir.LEFT;
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
        
        _stateMachine.Init(this, _states[(uint)EColossalBossState.WAKE]);
    }


    public override void OnHittedByPlayerNormalAttack(ECharacterLookDir eLookDir, int damage, EPlayerNoramlAttackType eAttackType)
    {
        if (ECurrentState != EColossalBossState.DIE && 
            ECurrentState != EColossalBossState.WAKE &&
            ECurrentState != EColossalBossState.BURF)
        {
            IsHittedByPlayerNormalAttack = true;
            DecreasHpAndInvokeHitEvents(damage, eAttackType);
            #region PROCESS_BACK_ATTACK_OR_THIRD_ATTACK
            if (eAttackType == EPlayerNoramlAttackType.BACK_ATTACK || eAttackType == EPlayerNoramlAttackType.ATTACK_3)
            {
                Managers.TimeManager.OnMonsterHittedByPlayerNormalAttack();
                Managers.HitParticle.PlayBigHittedParticle(transform.position);
            }
            #endregion
            IsHittedByPlayerNormalAttack = false;

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
        Stat.Init(define.EMonsterNames.BossColossal);
    }


    #region OVERRIDE_ABSTRACT_HITTED_PLAYER_SPECIAL_ATTACK
    public override void OnPlayerBlockSuccess()
    {
        HittedByNormalAttackNoArgsEventHandler?.Invoke();
    }

    public override void OnHittedByPlayerKnockbackBomb()
    {
        // TODO : 이것도 나름 먹히도록 바꾸자.
        HittedByNormalAttackNoArgsEventHandler?.Invoke();
    }

    public override void OnHittedByPlayerSpawnReaper()
    {
        // TODO : 이거 나중에 처리해야함. 마비는 먹히도록 바꾸자.
        HittedByNormalAttackNoArgsEventHandler?.Invoke();
    }
    #endregion
}
