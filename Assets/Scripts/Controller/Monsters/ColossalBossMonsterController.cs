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
    private readonly Vector2 COLOSSAL_KNOCKBACK_FORCE = new Vector2(7f, 7f);
    public EColossalBossPhase EColossalPhase { get; private set; } = EColossalBossPhase.FirstPhase;
    public EColossalBossState ECurrentState { get; private set; }
    protected StateMachine<ColossalBossMonsterController> _stateMachine;
    protected State<ColossalBossMonsterController>[] _states;

    public bool IsPlayerInSpinAttackZone { get; set; }
    public bool IsPlayerInFistAttackZone { get; set; }
    public bool IsPlayerInBurstAttackZone { get; set; }

    public bool IsWake { get; private set; }

    Coroutine _hitFlashCoOrNull;


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
        if (ECurrentState == EColossalBossState.Die)
            return;
        if (_parallysisCoroutineOrNull != null)
        {
            StopCoroutine(_parallysisCoroutineOrNull);
            _parallysisCoroutineOrNull = null;
            StopCoroutine(_hitFlashCoOrNull);
            _hitFlashCoOrNull = null;
        }

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
            case EColossalBossState.FistMelleAttack:
                _fistAttackLight.OnMonsterAttackStart();
                break;
            case EColossalBossState.SpinMelleAttack:
                _spinAttackLight.OnMonsterAttackStart();
                break;
            case EColossalBossState.BurstMelleAttack:
                _burstAttackLight.OnMonsterAttackStart();
                break;
            case EColossalBossState.BurfedBurstMelleAttack:
                _burfedBurstLight.OnMonsterAttackStart();
                break;
        }
    }
    private void OnAttackAnimTurnOffLightTiming()
    {
        switch (ECurrentState)
        {
            case EColossalBossState.FistMelleAttack:
                _fistAttackLight.OnMonsterAttackEnd();
                break;
            case EColossalBossState.SpinMelleAttack:
                _spinAttackLight.OnMonsterAttackEnd();
                break;
            case EColossalBossState.BurstMelleAttack:
                _burstAttackLight.OnMonsterAttackEnd();
                break;
            case EColossalBossState.BurfedBurstMelleAttack:
                _burfedBurstLight.OnMonsterAttackEnd();
                break;
        }
    }
    #endregion

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
        // TODO : ColossalBoss 플레이어 스킬에 어떻게 반응할지 결정해주어야 한다. 완료!!
        ESkillType eType = (ESkillType)skillInfo.id;
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
            case ESkillType.Spawn_Reaper_LV2:
            case ESkillType.Spawn_Reaper_LV3:
                ChangeState(EColossalBossState.Hit);
                Debug.Log("Hit By SpawnReaper");
                _parallysisCoroutineOrNull = StartCoroutine(PlayHitAnimForSeconds(skillInfo.parallysisTime * 0.5f));
                _hitFlashCoOrNull = StartCoroutine(PlayHitFlashForSeconds(skillInfo.parallysisTime * 0.5f));
                break;
            case ESkillType.Spawn_Shooter_LV1:
            case ESkillType.Spawn_Shooter_LV2:
            case ESkillType.Spawn_Shooter_LV3:
                AddKnockbackForce(new Vector2(skillInfo.knockbackForceX * 0.5f, skillInfo.knockbackForceY * 0.5f));
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

    #endregion
    IEnumerator PlayHitAnimForSeconds(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        ChangeState(EColossalBossState.Run);
    }

    IEnumerator PlayHitFlashForSeconds(float flashTotalTimeInSec)
    {
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
