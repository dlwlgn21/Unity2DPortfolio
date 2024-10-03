using define;
using DG.DemiLib;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum EPlayerState
{
    IDLE,
    RUN,
    ROLL,
    JUMP,
    CLIMB,
    FALL,
    FALL_TO_TWICE_JUMP,
    TWICE_JUMP_TO_FALL,
    LAND,
    NORMAL_ATTACK_1,
    NORMAL_ATTACK_2,
    NORMAL_ATTACK_3,
    CAST_LAUNCH,
    CAST_SPAWN,
    HITTED_MELLE_ATTACK,
    HITTED_STATUS_PARALLYSIS,
    HITTED_PROJECTILE_DAMAGE,
    HITTED_PROJECTILE_KONCKBACK,
    HITTED_PROJECTILE_STUN,
    BLOCKING,
    BLOCK_SUCESS,
    DIE,
    COUNT
}

public enum EPlayerNoramlAttackType
{
    ATTACK_1,
    ATTACK_2,
    ATTACK_3,
    BACK_ATTACK,
}
public class PlayerController : BaseCharacterController
{
    public static UnityAction<EPlayerState> PlayerChangeStateEventHandler;
    public static UnityAction<EPlayerState> HitEffectEventHandler;
    public static UnityAction<EPlayerMovementEffect, ECharacterLookDir, Vector2> MovementEffectEventHandler;
    public static UnityAction<int, int, int> HitUIEventHandler;
    public static UnityAction<EPlayerSkill> PlayerSkillKeyDownEventHandler;
    public static UnityAction<EPlayerSkill> PlayerSkillValidAnimTimingEventHandler;
    public static UnityAction<EMonsterStatusEffect, float> PlayerStatusEffectEventHandler;
    public static UnityAction PlayerDieEventHandelr;
    public static UnityAction<int, int> PlayerIncreaseHpEventHandler; // UIPlayerHPBar

    public readonly static Vector2 NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE = new(2f, 1f);
    public readonly static Vector2 NORMAL_ATTACK_LEFT_KNOCKBACK_FORCE = new(-NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE.x, NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE.y);
    public readonly static Vector2 NORMAL_ATTACK_1_DASH_FORCE = new(3f, 2f);

    public readonly static float NORMAL_ATTACK_2_FORCE_COEFF = 1.1f;
    public readonly static float NORMAL_ATTACK_3_FORCE_COEFF = 1.2f;

    public readonly static float NORMAL_ATTACK_2_DAMAGE_COEFF = 1.5f;
    public readonly static int  NORMAL_ATTACK_3_DAMAGE_COEFF = 2;
    public readonly static int  BACK_ATTACK_DAMAGE_COEFF = 3;

    public readonly static float BACK_ATTACK_FORCE_COEFF = 1.5f;
    public readonly static float BLOCK_SUCCESS_KNOCKBACK_X_FORCE = 5f;
    public readonly static float KNOCKBACK_BOMB_FORCE = 12f;

    public readonly static KeyCode KeyUp = KeyCode.UpArrow;
    public readonly static KeyCode KeyDown = KeyCode.DownArrow;
    public readonly static KeyCode KeyRight = KeyCode.RightArrow;
    public readonly static KeyCode KeyLeft = KeyCode.LeftArrow;
    public readonly static KeyCode KeyAttack = KeyCode.Z;
    public readonly static KeyCode KeyBlock = KeyCode.X;
    public readonly static KeyCode KeyRoll = KeyCode.C;
    public readonly static KeyCode KeyJump = KeyCode.Space;
    public readonly static KeyCode KeyLaunchBomb = KeyCode.A;
    public readonly static KeyCode KeySpawnReaper = KeyCode.S;

    private const float BURN_TIME_IN_SEC = 2.0f;

    public CamFollowObject CamFollowObject;
    public CapsuleCollider2D CapsuleCollider { get; set; }
    public PlayerStat Stat { get; private set; }
    public EPlayerState ECurrentState { get; private set; }
    public Transform LedgeHeadRayPoint { get; private set; }
    public Transform LedgeBodyRayPoint { get; private set; }
    public Transform SpawnReaperPoint { get; private set; }
    public Transform SpawnShooterPoint { get; private set; }

    private StateMachine<PlayerController> _stateMachine;
    private State<PlayerController>[] _states;

    public bool IsInvincible { get; set; } = false;
    public bool IsSlowState { get; set; } = false;

    public bool IsBurned { get; set; } = false;

    private int _lastBurnedDamage;
    public override void Init()
    {
        base.Init();
        Stat = gameObject.GetOrAddComponent<PlayerStat>();
        CapsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
        ELookDir = ECharacterLookDir.RIGHT;
        LedgeHeadRayPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeHeadRayPoint");
        LedgeBodyRayPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeBodyRayPoint");
        SpawnReaperPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnReaperPoint");
        SpawnShooterPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnShooterPoint");


        #region SUBSCRIBE_EVENT
        MonsterProjectileController.MonsterProjectileHitPlayerEventHandelr += OnHittedByMonsterAttack;
        MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr += OnHittedByMonsterAttack;
        PlayerFallDeadZone.PlayerFallDeadZoneEventHandler += OnPlayerFallToDeadZone;
        #endregion
    }


    #region ItemEquipOrConsume
    public void OnCousumableItemUsed(EItemConsumableType eType, int amount)
    {
        switch (eType)
        {
            case EItemConsumableType.Hp:
                {
                    PlayerIncreaseHpEventHandler?.Invoke(Stat.HP, Mathf.Clamp(Stat.HP + amount, 1, Stat.MaxHP));
                    Stat.IncreaseHp(amount);
                    break;
                }
            default:
                Debug.Assert(false);
                break;
        }
    }
    public void OnItemEqiuped(ItemInfo itemInfo)
    {
        switch (itemInfo.EEquippableType)
        {
            case EItemEquippableType.Helmet:
                {
                    data.HelmetInfo info = Managers.Data.HelmetItemDict[itemInfo.ItemId];
                    Debug.Assert(info != null);
                    Stat.HelmetPlusDefence = info.defence;
                    break;
                }
            case EItemEquippableType.Armor:
                {
                    data.ArmorInfo info = Managers.Data.ArmorItemDict[itemInfo.ItemId];
                    Debug.Assert(info != null);
                    Stat.ArmorPlusDefence = info.defence;
                    break;
                }
            case EItemEquippableType.Sword:
                {
                    data.SwordInfo info = Managers.Data.SwordItemDict[itemInfo.ItemId];
                    Debug.Assert(info != null);
                    Stat.SwordPlusDamage = info.damage;
                    break;
                }
            default:
                Debug.Assert(false);
                break;
        }
    }
    public void OnItemUnequiped(EItemEquippableType eType)
    {
        switch (eType)
        {
            case EItemEquippableType.Helmet:
                {
                    Stat.HelmetPlusDefence = 0;
                    break;
                }
            case EItemEquippableType.Armor:
                {
                    Stat.ArmorPlusDefence = 0;
                    break;
                }
            case EItemEquippableType.Sword:
                {
                    Stat.SwordPlusDamage = 0;
                    break;
                }
            default:
                Debug.Assert(false);
                break;
        }
    }
    #endregion
    private void OnDestroy()
    {
        MonsterProjectileController.MonsterProjectileHitPlayerEventHandelr -= OnHittedByMonsterAttack;
        MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr -= OnHittedByMonsterAttack;
        PlayerFallDeadZone.PlayerFallDeadZoneEventHandler -= OnPlayerFallToDeadZone;
        PlayerChangeStateEventHandler = null;
        HitEffectEventHandler = null;
        MovementEffectEventHandler = null;
        HitUIEventHandler = null;
        PlayerSkillKeyDownEventHandler = null;
        PlayerSkillValidAnimTimingEventHandler = null;
        PlayerStatusEffectEventHandler = null;
        PlayerDieEventHandelr = null;
        PlayerIncreaseHpEventHandler = null;
    }

    void FixedUpdate()
    {
        #region PAUSE
        if (IsSkipThisFrame())
        {
            Vector2 velo = RigidBody.velocity;
            RigidBody.velocity = new Vector2(0f, velo.y);
            return;
        }
        #endregion
        _stateMachine.FixedExcute();
    }

    void Update()
    {
        #region PAUSE
        if (IsSkipThisFrame())
        {
            if (ECurrentState != EPlayerState.IDLE)
            {
                if (ECurrentState == EPlayerState.JUMP)
                {
                    ChangeState(EPlayerState.FALL);
                }
                else
                {
                    ChangeState(EPlayerState.IDLE);
                }
            }
            return;
        }
        #endregion
        _stateMachine.Excute();
    }

    private void OnAnimFullyPlayed()
    {
        ((player_states.BasePlayerState)_states[(uint)ECurrentState]).OnAnimFullyPlayed();
    }

    #region ANIM_CALL_BACK_TIMING
    private void OnValidLaunchTiming() 
    { 
        Debug.Assert(ECurrentState == EPlayerState.CAST_LAUNCH);
        if (ECurrentState == EPlayerState.CAST_LAUNCH)
        {
            PlayerSkillValidAnimTimingEventHandler?.Invoke(EPlayerSkill.SPAWN_SHOOTER);
        }
    }
    private void OnValidSpawnReaperTiming()
    {
        if (ECurrentState == EPlayerState.CAST_SPAWN)
        {
            PlayerSkillValidAnimTimingEventHandler?.Invoke(EPlayerSkill.SPAWN_REAPER);
        }
    }

    private void OnValidRollEffectTiming()
    {
        PlayMovementEffectAnimation(EPlayerMovementEffect.ROLL, ELookDir, transform.position);
    }

    private void OnPlayerNormalAttack1ValidStopEffectTiming()
    {
        PlayMovementEffectAnimation(EPlayerMovementEffect.NORMAL_ATTACK_LAND, ELookDir, transform.position);
    }
    private void OnPlayerFootStep()
    {
        FootDustParticle.Play();
    }

    private void OnAttackLightTurnOnTiming()
    {

    }

    #endregion

    private void OnHittedByMonsterAttack(BaseMonsterController mc)
    {
        if (IsInvincible)
        {
            return;
        }
        if (ECurrentState == EPlayerState.BLOCKING && ELookDir != mc.ELookDir)
        {
            HitEffectEventHandler?.Invoke(EPlayerState.BLOCK_SUCESS);
            ChangeState(EPlayerState.BLOCK_SUCESS);
            mc.OnPlayerBlockSuccess();
            return;
        }
        if (!IsValidStateToChangeHitState())
        {
            return;
        }
        ActualDamgedFromMonsterAttack(mc.Stat.Attack);
        ChangeHitOrDieState();
        ProcessStatusEffect(mc);
    }
    public void ChangeState(EPlayerState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
        PlayerChangeStateEventHandler?.Invoke(eChangingState);
        switch (eChangingState)
        {
            case EPlayerState.IDLE:
                break;
            case EPlayerState.RUN:
                break;
            case EPlayerState.ROLL:
                FootDustParticle.Play();
                break;
            case EPlayerState.JUMP:
                PlayMovementEffectAnimation(EPlayerMovementEffect.JUMP, ELookDir, transform.position);
                break;
            case EPlayerState.CLIMB:
                break;
            case EPlayerState.FALL:
                break;
            case EPlayerState.FALL_TO_TWICE_JUMP:
                break;
            case EPlayerState.TWICE_JUMP_TO_FALL:
                break;
            case EPlayerState.LAND:
                PlayMovementEffectAnimation(EPlayerMovementEffect.LAND, ELookDir, transform.position);
                FootDustParticle.Play();
                break;
            case EPlayerState.NORMAL_ATTACK_1:
                PlayMovementEffectAnimation(EPlayerMovementEffect.NORMAL_ATTACK_1, ELookDir, transform.position);
                break;
            case EPlayerState.NORMAL_ATTACK_2:
                break;
            case EPlayerState.NORMAL_ATTACK_3:
                break;
            case EPlayerState.CAST_LAUNCH:
                break;
            case EPlayerState.CAST_SPAWN:
                break;
            case EPlayerState.HITTED_MELLE_ATTACK:
                break;
            case EPlayerState.BLOCKING:
                break;
            case EPlayerState.BLOCK_SUCESS:
                break;
            case EPlayerState.DIE:
                break;
            case EPlayerState.COUNT:
                break;
            default:
                break;
        }
    }
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<PlayerController>();
        _states = new State<PlayerController>[(uint)EPlayerState.COUNT];
        _states[(uint)EPlayerState.IDLE] = new player_states.Idle(this);
        _states[(uint)EPlayerState.RUN] = new player_states.Run(this);
        _states[(uint)EPlayerState.ROLL] = new player_states.Roll(this);
        _states[(uint)EPlayerState.JUMP] = new player_states.Jump(this);
        _states[(uint)EPlayerState.CLIMB] = new player_states.Climb(this);
        _states[(uint)EPlayerState.FALL] = new player_states.FallCanTwiceJump(this);
        _states[(uint)EPlayerState.FALL_TO_TWICE_JUMP] = new player_states.FallToTwiceJump(this);
        _states[(uint)EPlayerState.TWICE_JUMP_TO_FALL] = new player_states.TwiceJumpToFall(this);
        _states[(uint)EPlayerState.LAND] = new player_states.Land(this);
        _states[(uint)EPlayerState.NORMAL_ATTACK_1] = new player_states.NormalAttack1(this);
        _states[(uint)EPlayerState.NORMAL_ATTACK_2] = new player_states.NormalAttack2(this);
        _states[(uint)EPlayerState.NORMAL_ATTACK_3] = new player_states.NormalAttack3(this);
        _states[(uint)EPlayerState.CAST_LAUNCH] = new player_states.CastLaunch(this);
        _states[(uint)EPlayerState.CAST_SPAWN] = new player_states.CastSpawn(this);
        _states[(uint)EPlayerState.BLOCKING] = new player_states.Blocking(this);
        _states[(uint)EPlayerState.BLOCK_SUCESS] = new player_states.BlockSuccess(this);
        _states[(uint)EPlayerState.HITTED_MELLE_ATTACK] = new player_states.HittedMelleAttack(this);
        _states[(uint)EPlayerState.HITTED_STATUS_PARALLYSIS] = new player_states.HittedParallysis(this);
        _states[(uint)EPlayerState.HITTED_PROJECTILE_KONCKBACK] = new player_states.HittedProjectileKnockback(this);
        _states[(uint)EPlayerState.DIE] = new player_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EPlayerState.IDLE]);

        // 6.10 FallState에서도 이단점프 할 수 있도록 하기 위해 추가.
        ((player_states.FallCanTwiceJump)_states[(uint)EPlayerState.FALL]).SubscribeTwiceJumpEventHandler((player_states.Jump)_states[(uint)EPlayerState.JUMP]);
    }
    private void PlayMovementEffectAnimation(EPlayerMovementEffect eEffectType, ECharacterLookDir eLookDir, Vector2 pos)
    {
        MovementEffectEventHandler?.Invoke(eEffectType, eLookDir, pos);
    }

    private bool IsSkipThisFrame()
    {
        if (Managers.Pause.IsPaused || Managers.Dialog.IsTalking)
            return true;
        return false;
    }

    private void ActualDamgedFromMonsterAttack(int damge)
    {
        #region ACTUAL_DAMAGE
        int beforeDamageHP;
        int afterDamageHP;
        int actualDamage = Stat.DecreaseHpAndGetActualDamageAmount(damge, out beforeDamageHP, out afterDamageHP);
        // TODO : 이부분 나중에 따로 뺄거임.
        InvokePlayerHitEvent(actualDamage, beforeDamageHP, afterDamageHP);
        #endregion
    }
    private void InvokePlayerHitEvent(int damge, int beforeDamageHP, int afterDamageHP)
    {
        HitUIEventHandler?.Invoke(damge, beforeDamageHP, afterDamageHP);
        HitEffectEventHandler?.Invoke(EPlayerState.HITTED_MELLE_ATTACK);
        Managers.TimeManager.OnPlayerHittedByMonster();
    }
    private void ChangeHitOrDieState()
    {
        if (Stat.HP <= 0)
        {
            ChangeState(EPlayerState.DIE);
        }
        else
        {
            ChangeState(EPlayerState.HITTED_MELLE_ATTACK);
        }
    }

    private void ProcessStatusEffect(BaseMonsterController mc)
    {
        switch (mc.Stat.EStatusEffectType)
        {
            case EMonsterStatusEffect.NONE:
            case EMonsterStatusEffect.KNOCKBACK:
                player_states.BaseHitted state = (player_states.BaseHitted)_states[(uint)EPlayerState.HITTED_MELLE_ATTACK];
                Debug.Assert(state != null);
                Vector2 knockbackForce = mc.Stat.KnockbackForce;
                if (mc.ELookDir == ECharacterLookDir.LEFT)
                {
                    state.AdjustKnockbackForce(new(-knockbackForce.x, knockbackForce.y));
                }
                else
                {
                    state.AdjustKnockbackForce(knockbackForce);
                }
                break;
            case EMonsterStatusEffect.BLIND:
                Managers.FullScreenEffect.StartFullScreenEffect(EFullScreenEffectType.MONSTER_BLIND_EFFECT);
                break;
            case EMonsterStatusEffect.BURN:
                if (!IsBurned)
                {
                    _lastBurnedDamage = mc.Stat.Attack;
                    StartCoroutine(BurnPlayerCo());
                    PlayerStatusEffectEventHandler?.Invoke(EMonsterStatusEffect.BURN, BURN_TIME_IN_SEC);
                }
                break;
            case EMonsterStatusEffect.SLOW:
                if (!IsSlowState)
                {
                    StartCoroutine(StartSlowStateCountdownCo(mc.Stat.SlowTimeInSec));
                    PlayerStatusEffectEventHandler?.Invoke(EMonsterStatusEffect.SLOW, mc.Stat.SlowTimeInSec);
                }
                break;
            case EMonsterStatusEffect.PARALLYSIS:
                ChangeState(EPlayerState.HITTED_STATUS_PARALLYSIS);
                break;
        }
    }
    private void OnPlayerFallToDeadZone()
    {
        ChangeState(EPlayerState.DIE);
    }
    public bool IsValidStateToChangeHitState()
    {
        if (ECurrentState == EPlayerState.BLOCK_SUCESS ||
            ECurrentState == EPlayerState.CLIMB ||
            ECurrentState == EPlayerState.ROLL ||
            ECurrentState == EPlayerState.CAST_SPAWN)
        {
            return false;
        }
        return true;
    }
    private IEnumerator StartSlowStateCountdownCo(float slowTimeInSec)
    {
        IsSlowState = true;
        yield return new WaitForSeconds(slowTimeInSec);
        IsSlowState = false;
    }
    private IEnumerator BurnPlayerCo()
    {
        IsBurned = true;
        yield return new WaitForSeconds(1f);
        ActualDamgedFromMonsterAttack(Mathf.Max((int)(_lastBurnedDamage * 0.5f), 1));
        yield return new WaitForSeconds(1f);
        ActualDamgedFromMonsterAttack(Mathf.Max((int)(_lastBurnedDamage * 0.5f), 1));
        IsBurned = false;
    }
}