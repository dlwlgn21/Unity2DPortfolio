using define;
using JetBrains.Annotations;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EColossalBossPhase
{ 
    FirstPhase, // FIST, SPIN, BURST
    SecondUnder50PercentHpPhase, // FIST, SPIN, BURFED_BURST,
    Count
}

public enum EColossalBossState
{
    Wake,
    Run,
    FistMelleAttack,
    SpinMelleAttack,
    BurstMelleAttack,
    BurfedBurstMelleAttack,
    Burf,
    Hit,
    Die,
    Count
}
public class ColossalBossMonsterController : BaseMonsterController, IMelleAttackable, IZonePlayerDetetable
{
    static public UnityAction<EColossalBossState> ColossalChangeStateEventHandler;
    private readonly Vector2 COLOSSAL_KNOCKBACK_FORCE = new Vector2(12f, 7f);
    public EColossalBossPhase EColossalPhase { get; private set; } = EColossalBossPhase.FirstPhase;
    public EColossalBossState ECurrentState { get; private set; }
    protected StateMachine<ColossalBossMonsterController> _stateMachine;
    protected State<ColossalBossMonsterController>[] _states;

    public bool IsPlayerInSpinAttackZone { get; set; }
    public bool IsPlayerInFistAttackZone { get; set; }
    public bool IsPlayerInBurstAttackZone { get; set; }
    public bool IsWake { get; private set; }

    const string SFX_FIST_SOUND_PATH = "Sound/SFX_BossColossalFisting";
    const string SFX_BURST_SOUND_PATH = "Sound/SFX_BossColossalBurst";
    const string SFX_BURFED_BURST_SOUND_PATH = "Sound/SFX_BossColossalBurfedBurst";
    const string SFX_BURF_SOUND_PATH = "Sound/SFX_BossColossalBuff";
    const string SFX_DIE_PATH = "Sound/SFX_BossMonsterDie";
    const string SFX_WAKE_PATH = "Sound/SFX_BossMonsterWake";

    Coroutine _hitFlashCoOrNull;

    #region Lights
    [SerializeField] GameObject headLight;
    [SerializeField] GameObject fistLight;
    [SerializeField] GameObject bodyLight;
    #endregion

    #region AttackLights
    [SerializeField] LightController _fistAttackLightController;
    [SerializeField] LightController _spinAttackLightController;
    [SerializeField] LightController _burstAttackLightController;
    [SerializeField] LightController _burfedBurstLightController;
    [SerializeField] LightController _burfLightController;
    #endregion
    public override void Init()
    {
        base.Init();
        InitStates();
        //AllocateMelleAttackState();
        EMonsterType = EMonsterNames.BossColossal;
        Animator.enabled = false;
        #region AttackZoneDetectionEvent
        ColossalAttackZoneDetection.ColossalAttackZoneEnterEvnetHandler += OnPlayerEnterAttackZone;
        ColossalAttackZoneDetection.ColossalAttackZoneExitEvnetHandler += OnPlayerExitAttackZone;
        #endregion
        Stat.KnockbackForce = COLOSSAL_KNOCKBACK_FORCE;
        SetLightControllersTurnOffTimeInSec();
        HealthBar.gameObject.SetActive(false);
        SetActiveBodyLights(false);
    }

    protected override void SetLightControllersTurnOffTimeInSec()
    {
        _fistAttackLightController.TurnOffGraduallyLightTimeInSec = 0.4f;
        _spinAttackLightController.TurnOffGraduallyLightTimeInSec = 0.4f;
        _burfedBurstLightController.TurnOffGraduallyLightTimeInSec = 0.4f;
        _burstAttackLightController.TurnOffGraduallyLightTimeInSec = 0.4f;
        _burfLightController.TurnOffGraduallyLightTimeInSec = 1f;
    }
    private void OnDestroy()
    {
        ColossalChangeStateEventHandler = null;
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

    public void SetActiveBodyLights(bool isActive)
    {
        headLight.SetActive(isActive);
        fistLight.SetActive(isActive);
        bodyLight.SetActive(isActive);
    }

    public void ChangeState(EColossalBossState eChangingState)
    {
        if (ECurrentState == EColossalBossState.Die)
        {
            return;
        }
        if (_parallysisCoroutineOrNull != null)
        {
            StopCoroutine(_parallysisCoroutineOrNull);
            _parallysisCoroutineOrNull = null;
            StopCoroutine(_hitFlashCoOrNull);
            _hitFlashCoOrNull = null;
        }
        ECurrentState = eChangingState;
        if (ECurrentState == EColossalBossState.Die)
        {
            TurnOffAllLights();
        }
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
        ColossalChangeStateEventHandler?.Invoke(eChangingState);
    }
    public void AllocateMelleAttackState()
    {
        //_states[(uint)EColossalBossState.SPIN_MELLE_ATTACK] = new monster_states.BossColossalSpinAttack(this);
    }
    public override void DamagedFromPlayer(ECharacterLookDir eLookDir, int damage, EPlayerNoramlAttackType eAttackType)
    {
        if (ECurrentState != EColossalBossState.Die &&
            ECurrentState != EColossalBossState.Wake &&
            ECurrentState != EColossalBossState.Burf)
        {
            DecreasHpAndInvokeHitEvents(damage, eAttackType);
            if (Stat.HP <= 0)
            {
                ChangeState(EColossalBossState.Die);
            }
            else
            {
                // Change To Second Phase.
                if (Stat.HP < (int)(Stat.MaxHP * 0.5f) &&
                    EColossalPhase != EColossalBossPhase.SecondUnder50PercentHpPhase)
                {
                    ChangeState(EColossalBossState.Burf);
                    EColossalPhase = EColossalBossPhase.SecondUnder50PercentHpPhase;
                }
            }
        }
    }
    public void SetLookDir()
    {
        if (ECurrentState == EColossalBossState.Wake ||
            ECurrentState == EColossalBossState.Run)
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

    public override void OnPlayerBlockSuccess()
    {
        HittedByNormalAttackNoArgsEventHandler?.Invoke();
    }

    public override void OnHittedByPlayerSkill(EActiveSkillType eType)
    {
        // TODO : ColossalBoss 플레이어 스킬에 어떻게 반응할지 결정해주어야 한다. 완료!!
        data.SkillInfo info = Managers.PlayerSkill.GetCurrSkillLevelSkillInfo(eType);
        switch (eType)
        {
            case EActiveSkillType.Spawn_Reaper:
                ChangeState(EColossalBossState.Hit);
                Debug.Log("Hit By SpawnReaper");
                _parallysisCoroutineOrNull = StartCoroutine(PlayHitAnimForSeconds(info.parallysisTime));
                _hitFlashCoOrNull = StartCoroutine(PlayHitFlashForSeconds(info.parallysisTime));
                break;
            case EActiveSkillType.Spawn_Shooter:
                AddKnockbackForceOppossiteByPlayer(new Vector2(info.knockbackForceX * 0.5f, info.knockbackForceY * 0.5f));
                break;
            case EActiveSkillType.Cast_BlackFlame:
                DamagedFromPlayer(ELookDir, info.damage, EPlayerNoramlAttackType.Attack_3);
                break;
            case EActiveSkillType.Cast_SwordStrike:
                DamagedFromPlayer(ELookDir, info.damage, EPlayerNoramlAttackType.Attack_3);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }


    protected override void InitStates()
    {
        _stateMachine = new StateMachine<ColossalBossMonsterController>();
        _states = new State<ColossalBossMonsterController>[(uint)EColossalBossState.Count];

        _states[(uint)EColossalBossState.Wake]                  = new monster_states.BossColossalWake(this);
        _states[(uint)EColossalBossState.Run]                   = new monster_states.BossColossalRun(this);
        _states[(uint)EColossalBossState.SpinMelleAttack]     = new monster_states.BossColossalSpinAttack(this);
        _states[(uint)EColossalBossState.FistMelleAttack]     = new monster_states.BossColossalFistAttack(this);
        _states[(uint)EColossalBossState.BurstMelleAttack]    = new monster_states.BossColossalBurstAttack(this);
        _states[(uint)EColossalBossState.BurfedBurstMelleAttack]   = new monster_states.BossColossalBurfedBurstAttack(this);
        _states[(uint)EColossalBossState.Burf]                  = new monster_states.BossColossalBurf(this);
        _states[(uint)EColossalBossState.Hit]                  = new monster_states.BossColossalHit(this);
        _states[(uint)EColossalBossState.Die]                   = new monster_states.BossColossalDie(this);
        
        //_stateMachine.Init(this, _states[(uint)EColossalBossState.WAKE]);
    }




    #region AninEvents
    private void OnAnimFullyPlayed()
    {
        ((monster_states.BaseBossMonsterState)_states[(uint)ECurrentState]).OnAnimFullyPlayed();
    }

    void OnAttackAnimTurnOnLightTiming()
    {
        switch (ECurrentState)
        {
            case EColossalBossState.FistMelleAttack:
                _fistAttackLightController.TurnOnLight();
                break;
            case EColossalBossState.SpinMelleAttack:
                _spinAttackLightController.TurnOnLight();
                break;
            case EColossalBossState.BurstMelleAttack:
                _burstAttackLightController.TurnOnLight();
                break;
            case EColossalBossState.BurfedBurstMelleAttack:
                _burfedBurstLightController.TurnOnLight();
                break;
        }
    }
    void OnAttackAnimTurnOffLightTiming()
    {
        switch (ECurrentState)
        {
            case EColossalBossState.FistMelleAttack:
                _fistAttackLightController.TurnOffLightGradually();
                break;
            case EColossalBossState.SpinMelleAttack:
                _spinAttackLightController.TurnOffLightGradually();
                break;
            case EColossalBossState.BurstMelleAttack:
                _burstAttackLightController.TurnOffLightGradually();
                break;
            case EColossalBossState.BurfedBurstMelleAttack:
                _burfedBurstLightController.TurnOffLightGradually();
                break;
        }
    }

    void OnWakeSoundTiming()
    {
        Managers.Sound.Play(SFX_WAKE_PATH);
        Managers.Sound.Play(DataManager.SFX_BGM_COLOSSAL_BATTLE, define.ESoundType.Bgm);
    }
    void OnBurfAnimTurnOnLightTiming()
    {
        _burfLightController.TurnOnLight();
    }

    void OnBurfAnimTurnOffLightTiming()
    {
        _burfLightController.TurnOffLightGradually();
    }

    void OnFistAttackSoundTiming()
    {
        Managers.Sound.Play(SFX_FIST_SOUND_PATH);
    }

    void OnSpinAttack1SoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_1_PATH);
    }

    void OnSpinAttack2SoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_2_PATH);
    }

    void OnBurstAttackSoundTiming()
    {
        Managers.Sound.Play(SFX_BURST_SOUND_PATH);
    }

    void OnBurfedBurstAttackSoundTiming()
    {
        Managers.Sound.Play(SFX_BURFED_BURST_SOUND_PATH);
    }

    void OnBurfSoundTiming()
    {
        Managers.Sound.Play(SFX_BURF_SOUND_PATH);
    }

    void OnDieSoundTiming()
    {
        Managers.Sound.Play(SFX_DIE_PATH);
    }
    #endregion



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

    public void OnPlayerEnter(Collider2D collision)
    {
        IsWake = true;
        Animator.enabled = true;
        _stateMachine.Init(this, _states[(uint)EColossalBossState.Wake]);
    }

    public void OnPlayerStay(Collider2D collision)
    {
    }

    public void OnPlayerExit(Collider2D collision)
    {
        GameObject.Find("ColossalBossWakeZone")?.GetComponent<InteractBox>().Unactive();
    }

    
    void TurnOffAllLights()
    {
        headLight.SetActive(false);
        fistLight.SetActive(false);
        bodyLight.SetActive(false);
        _fistAttackLightController.ForceToStopCoroutineAndTurnOffLight();
        _spinAttackLightController.ForceToStopCoroutineAndTurnOffLight();
        _burstAttackLightController.ForceToStopCoroutineAndTurnOffLight();
        _burfedBurstLightController.ForceToStopCoroutineAndTurnOffLight();
        _burfLightController.ForceToStopCoroutineAndTurnOffLight();
    }
    IEnumerator PlayHitAnimForSeconds(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        ChangeState(EColossalBossState.Run);
    }

    IEnumerator PlayHitFlashForSeconds(float flashTotalTimeInSec)
    {
        _hitFlasher.StartDamageFlash();
        float timer = 0f;
        while (timer < flashTotalTimeInSec)
        {
            if (timer > 1f)
            {
                _hitFlasher.StartDamageFlash();
                timer -= 1f;
                flashTotalTimeInSec -= 1f;
            }
            yield return null;
            timer += Time.deltaTime;
        }
    }
}
