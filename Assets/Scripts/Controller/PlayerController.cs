using data;
using define;
using DG.DemiLib;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum EPlayerState
{
    Idle,
    Run,
    Roll,
    Jump,
    Climb,
    Fall,
    FallToTwiceJump,
    TwiceJumpToFall,
    Land,
    NormalAttack_1,
    NormalAttack_2,
    NormalAttack_3,
    SkillCast,
    SkillSpawn,
    HitByMelleAttack,
    HitByStatusParallysis,
    HitByProjectileKnockback,
    Block,
    BlockSucces,
    Die,
    Count
}

public enum EPlayerNoramlAttackType
{
    Attack_1,
    Attack_2,
    Attack_3,
    BackAttack,
}
public sealed class PlayerController : BaseCharacterController
{
    #region Events
    public static UnityAction<EPlayerState> PlayerChangeStateEventHandler;
    public static UnityAction<EPlayerState> HitEffectEventHandler;
    public static UnityAction<EPlayerMovementEffect, ECharacterLookDir, Vector2> MovementEffectEventHandler;
    public static UnityAction<int, int, int> HitUIEventHandler;
    public static UnityAction<EActiveSkillType> PlayerSkillKeyDownEventHandler;
    public static UnityAction PlayerSkillValidAnimTimingEventHandler;
    public static UnityAction<BaseMonsterController> PlayerStatusEffectEventHandler;
    public static UnityAction PlayerDieEventHandelr;
    #endregion
    #region KnockForce And DamageCoeff Figure
    public static Vector2 sNormalAttackKnockbackForce { get; private set; }
    public static Vector2 sNormalAttack1DashForce { get; private set; }
    public static Vector2 sBlockSuccesKnockbackForce { get; private set; }
    public static float sBitAttackKnockbackForceCoeff { get; private set; }
    public static float sNormalAttack2DamageCoeff { get; private set; }
    public static int sNormalAttack3DamageCoeff { get; private set; }
    public static int sBackAttackDamageCoeff { get; private set; }
    #endregion
    #region Keys
    public const KeyCode KeyUp = KeyCode.UpArrow;
    public const KeyCode KeyDown = KeyCode.DownArrow;
    public const KeyCode KeyRight = KeyCode.RightArrow;
    public const KeyCode KeyLeft = KeyCode.LeftArrow;
    public const KeyCode KeyAttack = KeyCode.Z;
    public const KeyCode KeyBlock = KeyCode.X;
    public const KeyCode KeyRoll = KeyCode.C;
    public const KeyCode KeyJump = KeyCode.Space;
    public const KeyCode KeySkillA = KeyCode.A;
    public const KeyCode KeySkillS = KeyCode.S;
    #endregion
    public CapsuleCollider2D CapsuleCollider { get; set; }
    public PlayerStat Stat { get; private set; }
    public EPlayerState ECurrentState { get; private set; }
    public Transform LedgeHeadRayPoint { get; private set; }
    public Transform LedgeBodyRayPoint { get; private set; }
    public Transform SpawnReaperPoint { get; private set; }
    public Transform SpawnShooterPoint { get; private set; }
    public Transform CastBlackFlamePoint { get; private set; }
    public Transform CastSwordStrikePoint { get; private set; }
    public GameObject HeadLight { get; private set; }

    public bool IsInvincible { get; set; } = false;
    public bool IsSlowState { get; set; } = false;
    public bool IsBurned { get; set; } = false;

    StateMachine<PlayerController> _stateMachine;
    State<PlayerController>[] _states;

    public override void Init()
    {
        base.Init();
        if (Stat == null)
        {
            Stat = gameObject.GetOrAddComponent<PlayerStat>();
            CapsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
            ELookDir = ECharacterLookDir.Right;
            LedgeHeadRayPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeHeadRayPoint");
            LedgeBodyRayPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeBodyRayPoint");
            SpawnReaperPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnReaperPoint");
            SpawnShooterPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnShooterPoint");
            CastBlackFlamePoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillBlackFlamePoint");
            CastSwordStrikePoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSwordStrikePoint");
            HeadLight = transform.Find("HeadLight").gameObject;
            #region SUBSCRIBE_EVENT
            MonsterProjectileController.MonsterProjectileHitPlayerEventHandelr -= OnHittedByMonsterAttack;
            MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr -= OnHittedByMonsterAttack;
            MonsterProjectileController.MonsterProjectileHitPlayerEventHandelr += OnHittedByMonsterAttack;
            MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr += OnHittedByMonsterAttack;
            FallDeadZone.PlayerFallDeadZoneEventHandler += OnPlayerFallToDeadZone;
            #endregion
            #region KnockbackForce And DamageCoeff
            PlayerFigureContainer container = Managers.Data.PlayerFigureContainer;
            sNormalAttackKnockbackForce = new Vector2(container.NormalAttackKnockbackForceX, container.NormalAttackKnockbackForceY);
            sNormalAttack1DashForce = new Vector2(container.NormalAttack1DashForceX, container.NormalAttack1DashForceY);
            sBlockSuccesKnockbackForce = new Vector2(container.BlockSuccessKnockbackForceX, container.BlockSuccessKnockbackForceY);
            sBitAttackKnockbackForceCoeff = container.BigAttackForceCoeff;
            sNormalAttack2DamageCoeff = container.NormalAttack2DamageCoeff;
            sNormalAttack3DamageCoeff = (int)container.NormalAttack3DamageCoeff;
            sBackAttackDamageCoeff = (int)container.BackAttackDamageCoeff;
            #endregion
            DontDestroyOnLoad(this);
        }
    }

    private void OnDestroy()
    {
        MonsterProjectileController.MonsterProjectileHitPlayerEventHandelr -= OnHittedByMonsterAttack;
        MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr -= OnHittedByMonsterAttack;
        FallDeadZone.PlayerFallDeadZoneEventHandler -= OnPlayerFallToDeadZone;
        PlayerChangeStateEventHandler = null;
        HitEffectEventHandler = null;
        MovementEffectEventHandler = null;
        HitUIEventHandler = null;
        PlayerSkillKeyDownEventHandler = null;
        PlayerSkillValidAnimTimingEventHandler = null;
        PlayerStatusEffectEventHandler = null;
        PlayerDieEventHandelr = null;
    }

    void FixedUpdate()
    {
        #region PAUSE
        if (Managers.Pause.IsPaused)
        {
            return;
        }
        #endregion
        _stateMachine.FixedExcute();
    }

    void Update()
    {
        #region PAUSE
        if (Managers.Pause.IsPaused)
        {
            return;
        }
        #endregion
        if (Managers.Dialog.IsTalking)
        {
            if (ECurrentState != EPlayerState.Idle)
                ChangeState(EPlayerState.Idle);
            return;
        }
        _stateMachine.Excute();
    }

    #region Public
    public bool IsValidStateToChangeHitState()
    {
        if (ECurrentState == EPlayerState.BlockSucces ||
            ECurrentState == EPlayerState.Climb ||
            ECurrentState == EPlayerState.Roll ||
            ECurrentState == EPlayerState.SkillSpawn)
        {
            return false;
        }
        return true;
    }
    public void ChangeState(EPlayerState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
        PlayerChangeStateEventHandler?.Invoke(eChangingState);
        switch (eChangingState)
        {
            case EPlayerState.Idle:
                break;
            case EPlayerState.Run:
                break;
            case EPlayerState.Roll:
                FootDustParticle.Play();
                break;
            case EPlayerState.Jump:
                PlayMovementEffectAnimation(EPlayerMovementEffect.Jump, ELookDir, transform.position);
                break;
            case EPlayerState.Climb:
                break;
            case EPlayerState.Fall:
                break;
            case EPlayerState.FallToTwiceJump:
                break;
            case EPlayerState.TwiceJumpToFall:
                break;
            case EPlayerState.Land:
                PlayMovementEffectAnimation(EPlayerMovementEffect.Land, ELookDir, transform.position);
                FootDustParticle.Play();
                break;
            case EPlayerState.NormalAttack_1:
                PlayMovementEffectAnimation(EPlayerMovementEffect.NormalAttack_1, ELookDir, transform.position);
                break;
            case EPlayerState.NormalAttack_2:
                break;
            case EPlayerState.NormalAttack_3:
                break;
            case EPlayerState.SkillCast:
                break;
            case EPlayerState.SkillSpawn:
                break;
            case EPlayerState.HitByMelleAttack:
                break;
            case EPlayerState.Block:
                break;
            case EPlayerState.BlockSucces:
                break;
            case EPlayerState.Die:
                break;
            case EPlayerState.Count:
                break;
            default:
                break;
        }
    }

    public void AddOppositeForceByLookDir(Vector2 force)
    {
        // TODO : 나중에 Player에게 달자.
        force = ELookDir == ECharacterLookDir.Right ? new Vector2(-force.x, force.y) : new Vector2(force.x, force.y);
        RigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActualDamgedFromMonsterAttack(int damge)
    {
        #region ACTUAL_DAMAGE
        int beforeDamageHP;
        int afterDamageHP;
        int actualDamage = Stat.DecreaseHpAndGetActualDamageAmount(damge, out beforeDamageHP, out afterDamageHP);
        // TODO : 이부분 나중에 따로 뺄거임.
        #endregion

        InvokePlayerHitEvent(actualDamage, beforeDamageHP, afterDamageHP);

    }

    #region ItemEquipOrConsume

    public void OnCousumableItemUsed(EItemConsumableType eType, int amount)
    {
        switch (eType)
        {
            case EItemConsumableType.Hp:
                {
                    Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_PLAYER_HEALD);
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

    #endregion

    #region Protected
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<PlayerController>();
        _states = new State<PlayerController>[(uint)EPlayerState.Count];
        _states[(uint)EPlayerState.Idle] = new player_states.Idle(this);
        _states[(uint)EPlayerState.Run] = new player_states.Run(this);
        _states[(uint)EPlayerState.Roll] = new player_states.Roll(this);
        _states[(uint)EPlayerState.Jump] = new player_states.Jump(this);
        _states[(uint)EPlayerState.Climb] = new player_states.Climb(this);
        _states[(uint)EPlayerState.Fall] = new player_states.FallCanTwiceJump(this);
        _states[(uint)EPlayerState.FallToTwiceJump] = new player_states.FallToTwiceJump(this);
        _states[(uint)EPlayerState.TwiceJumpToFall] = new player_states.TwiceJumpToFall(this);
        _states[(uint)EPlayerState.Land] = new player_states.Land(this);
        _states[(uint)EPlayerState.NormalAttack_1] = new player_states.NormalAttack1(this);
        _states[(uint)EPlayerState.NormalAttack_2] = new player_states.NormalAttack2(this);
        _states[(uint)EPlayerState.NormalAttack_3] = new player_states.NormalAttack3(this);
        _states[(uint)EPlayerState.SkillCast] = new player_states.CastLaunch(this);
        _states[(uint)EPlayerState.SkillSpawn] = new player_states.CastSpawn(this);
        _states[(uint)EPlayerState.Block] = new player_states.Blocking(this);
        _states[(uint)EPlayerState.BlockSucces] = new player_states.BlockSuccess(this);
        _states[(uint)EPlayerState.HitByMelleAttack] = new player_states.HittedMelleAttack(this);
        _states[(uint)EPlayerState.HitByStatusParallysis] = new player_states.HittedParallysis(this);
        _states[(uint)EPlayerState.HitByProjectileKnockback] = new player_states.HittedProjectileKnockback(this);
        _states[(uint)EPlayerState.Die] = new player_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EPlayerState.Idle]);

        // 6.10 FallState에서도 이단점프 할 수 있도록 하기 위해 추가.
        ((player_states.FallCanTwiceJump)_states[(uint)EPlayerState.Fall]).SubscribeTwiceJumpEventHandler((player_states.Jump)_states[(uint)EPlayerState.Jump]);
    }
    #endregion

    #region Private
    void OnAnimFullyPlayed()
    {
        ((player_states.BasePlayerState)_states[(uint)ECurrentState]).OnAnimFullyPlayed();
    }

    #region AnimCallbackTiming

    #region SkillEvents
    private void OnValidSkillCastTypeTiming() 
    { 
        Debug.Assert(ECurrentState == EPlayerState.SkillCast);
        if (ECurrentState == EPlayerState.SkillCast)
        {
            PlayerSkillValidAnimTimingEventHandler?.Invoke();
        }
    }
    private void OnValidSkillSpawnTypeTiming()
    {
        if (ECurrentState == EPlayerState.SkillSpawn)
        {
            PlayerSkillValidAnimTimingEventHandler?.Invoke();
        }
    }
    #endregion
    void OnValidRollEffectTiming()
    {
        PlayMovementEffectAnimation(EPlayerMovementEffect.Roll, ELookDir, transform.position);
    }

    void OnPlayerNormalAttack1ValidStopEffectTiming()
    {
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_PLAYER_LAND_PATH);
        PlayMovementEffectAnimation(EPlayerMovementEffect.NormalAttackLand, ELookDir, transform.position);
    }
    void OnPlayerFootStep()
    {
        FootDustParticle.Play();
        int rand = Random.Range(0, 2);
        if (rand == 0)
            Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_PLAYER_FOOT_STEP_1_PATH);
        else
            Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_PLAYER_FOOT_STEP_2_PATH);
    }

    void OnAttackLightTurnOnTiming()
    {

    }

    #endregion

    void OnHittedByMonsterAttack(BaseMonsterController mc)
    {
        if (IsInvincible)
        {
            return;
        }
        if (ECurrentState == EPlayerState.Block && ELookDir != mc.ELookDir)
        {
            HitEffectEventHandler?.Invoke(EPlayerState.BlockSucces);
            ChangeState(EPlayerState.BlockSucces);
            mc.OnPlayerBlockSuccess();
            return;
        }
        if (!IsValidStateToChangeHitState())
        {
            return;
        }
        ActualDamgedFromMonsterAttack(mc.Stat.Attack);
        ChangeHitOrDieState();
        if (PlayerStatusEffectEventHandler != null)
            PlayerStatusEffectEventHandler.Invoke(mc);
    }
    void PlayMovementEffectAnimation(EPlayerMovementEffect eEffectType, ECharacterLookDir eLookDir, Vector2 pos)
    {
        MovementEffectEventHandler?.Invoke(eEffectType, eLookDir, pos);
    }


    void InvokePlayerHitEvent(int damge, int beforeDamageHP, int afterDamageHP)
    {
        HitUIEventHandler?.Invoke(damge, beforeDamageHP, afterDamageHP);
        HitEffectEventHandler?.Invoke(EPlayerState.HitByMelleAttack);
    }
    void ChangeHitOrDieState()
    {
        if (Stat.HP <= 0)
        {
            ChangeState(EPlayerState.Die);
        }
        else
        {
            ChangeState(EPlayerState.HitByMelleAttack);
        }
    }

    void OnPlayerFallToDeadZone()
    {
        ChangeState(EPlayerState.Die);
    }
    #endregion
}