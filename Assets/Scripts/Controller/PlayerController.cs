using define;
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
    HITTED,
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
    public static UnityAction HitEventHandler;
    public static UnityAction<EPlayerState> HitEffectEventHandler;
    public static UnityAction<EPlayerMovementEffect> MovementEventHandler;
    public static UnityAction<int, int, int> HitUIEventHandler;

    public readonly static Vector2 NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE = new Vector2(2f, 1f);
    public readonly static Vector2 NORMAL_ATTACK_LEFT_KNOCKBACK_FORCE = new Vector2(-NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE.x, NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE.y);
    public readonly static Vector2 NORMAL_ATTACK_1_DASH_FORCE = new Vector2(4f, 2f);

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

    public UIPlayerCoolTimer RollCoolTimerImg;
    public UIPlayerCoolTimer BombCoolTimerImg;
    public UIPlayerCoolTimer SpawnReaperCoolTimerImg;
    public CamFollowObject CamFollowObject;

    public BoxCollider2D BoxCollider { get; set; }
    public PlayerStat Stat { get; private set; }
    public EPlayerState ECurrentState { get; private set; }
    public Transform LedgeCheckPoint { get; private set; }
    public Transform SpawnReaperPoint { get; private set; }
    public Transform SpawnShooterPoint { get; private set; }

    private StateMachine<PlayerController> _stateMachine;
    private State<PlayerController>[] _states;

    // RollCoolTime
    public const float ROLL_INIT_COOL_TIME = 1f;
    public float RollCollTime { get; private set; } = ROLL_INIT_COOL_TIME;
    public float RollCollTimer { get; set; } = ROLL_INIT_COOL_TIME;
    public bool IsPossibleRoll { get; set; } = true;


    // BombCoolTime
    public const float BOMB_INIT_COOL_TIME = 3f;
    public float BombCollTime { get; private set; } = BOMB_INIT_COOL_TIME;
    public float BombCollTimer { get; set; } = BOMB_INIT_COOL_TIME;
    public bool IsPossibleLaunchBomb { get; set; } = true;


    // SpawnReaperCoolTime
    public const float SPAWN_REAPER_INIT_COOL_TIME = 5f;
    public float SpawnReaperCollTime { get; private set; } = SPAWN_REAPER_INIT_COOL_TIME;
    public float SpawnReaperCollTimer { get; set; } = SPAWN_REAPER_INIT_COOL_TIME;
    public bool IsPossibleSpawnReaper { get; set; } = true;

    public bool IsInvincible { get; set; } = false;

    public override void Init()
    {
        base.Init();
        Stat = gameObject.GetOrAddComponent<PlayerStat>();
        BoxCollider = gameObject.GetComponent<BoxCollider2D>();
        ELookDir = ECharacterLookDir.RIGHT;
        LedgeCheckPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeCheckPoint");
        SpawnReaperPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnReaperPoint");
        SpawnShooterPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnShooterPoint");
    }
    void FixedUpdate()
    {
        if (IsSkipThisFrame())
        {
            Vector2 velo = RigidBody.velocity;
            RigidBody.velocity = new Vector2(0f, velo.y);
            return;
        }
        _stateMachine.FixedExcute();
    }

    void Update()
    {
        #region ROLL_COOL_TIME
        if (!IsPossibleRoll)
        {
            RollCollTimer -= Time.deltaTime;
            if (RollCollTimer <= 0f)
            {
                RollCollTimer = RollCollTime;
                IsPossibleRoll = true;
            }
        }
        #endregion
        #region BOMB_COOL_TIME
        if (!IsPossibleLaunchBomb)
        {
            BombCollTimer -= Time.deltaTime;
            if (BombCollTimer <= 0f)
            {
                BombCollTimer = BombCollTime;
                IsPossibleLaunchBomb = true;
            }
        }
        #endregion
        #region SPAWN_REAPER_COOL_TIME
        if (!IsPossibleSpawnReaper)
        {
            SpawnReaperCollTimer -= Time.deltaTime;
            if (SpawnReaperCollTimer <= 0f)
            {
                SpawnReaperCollTimer = SpawnReaperCollTime;
                IsPossibleSpawnReaper = true;
            }
        }
        #endregion

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
        _stateMachine.Excute();
    }


    #region ANIM_CALL_BACK
    public void OnNormalAttack1AnimFullyPlayed()    { Debug.Assert(ECurrentState == EPlayerState.NORMAL_ATTACK_1); ((player_states.NormalAttackState)_states[(uint)EPlayerState.NORMAL_ATTACK_1]).OnAttackAnimFullyPlayed(); }
    public void OnNormalAttack2AnimFullyPlayed()    { Debug.Assert(ECurrentState == EPlayerState.NORMAL_ATTACK_2); ((player_states.NormalAttackState)_states[(uint)EPlayerState.NORMAL_ATTACK_2]).OnAttackAnimFullyPlayed(); }
    public void OnNormalAttack3AnimFullyPlayed()    { Debug.Assert(ECurrentState == EPlayerState.NORMAL_ATTACK_3); ((player_states.NormalAttackState)_states[(uint)EPlayerState.NORMAL_ATTACK_3]).OnAttackAnimFullyPlayed(); }
    
    public void OnValidLaunchTiming() 
    { 
        Debug.Assert(ECurrentState == EPlayerState.CAST_LAUNCH);
        if (ECurrentState == EPlayerState.CAST_LAUNCH)
        {
            Managers.Skill.CastSpawnShooter(SpawnShooterPoint.position, ELookDir);
        }
    }
    public void OnValidSpawnReaperTiming()
    {
        if (ECurrentState == EPlayerState.CAST_SPAWN)
        {
            Managers.Skill.CastSpawnReaper(SpawnReaperPoint.position, ELookDir);
        }
    }

    public void OnValidRollEffectTiming()
    {
        PlayMovementEffectAnimation(EPlayerMovementEffect.ROLL);
    }

    public void OnHittedAnimFullyPlayed()
    {
        player_states.Hitted hittedState = (player_states.Hitted)_states[(uint)EPlayerState.HITTED];
        hittedState.OnHittedAnimFullyPlayed();
    }
    public void OnRollAnimFullyPlayed()
    {
        player_states.Roll rollState = (player_states.Roll)_states[(uint)EPlayerState.ROLL];
        rollState.OnRollAnimFullyPlayed();
        FootDustParticle.Play();
    }
    public void OnPlayerClimbAnimFullyPlayed()
    {
        player_states.Climb climbState = (player_states.Climb)_states[(uint)EPlayerState.CLIMB];
        climbState.OnClimbAnimFullyPlayed();
    }

    public void OnPlayerLaunchAnimFullyPlayed()
    {
        player_states.CastLaunch launchState = (player_states.CastLaunch)_states[(uint)EPlayerState.CAST_LAUNCH];
        launchState.OnLaunchAnimFullyPlayed();
    }

    public void OnPlayerSpawnReaperAnimFullyPlayed()
    {
        player_states.CastSpawn spawnState = (player_states.CastSpawn)_states[(uint)EPlayerState.CAST_SPAWN];
        spawnState.OnSpawnAnimFullyPlayed();
    }

    public void OnPlayerNormalAttack1ValidStopEffectTiming()
    {
        PlayMovementEffectAnimation(EPlayerMovementEffect.NORMAL_ATTACK_LAND);
    }
    public void OnPlayerFootStep()
    {
        FootDustParticle.Play();
    }

    #endregion

    public void OnHitted(int damage, BaseMonsterController monContorller) 
    {
        #region INVINCIBLE
        if (IsInvincible)
        {
            return;
        }
        #endregion

        #region BLOCKING
        if (ECurrentState == EPlayerState.BLOCKING && ELookDir != monContorller.ELookDir)
        {
            HitEffectEventHandler?.Invoke(EPlayerState.BLOCK_SUCESS);
            ChangeState(EPlayerState.BLOCK_SUCESS);
            monContorller.OnPlayerBlockSuccess();
            return;
        }
        #endregion

        #region IGNORE_ACODDING_PLAYER_STATE
        if (ECurrentState == EPlayerState.BLOCK_SUCESS || 
            ECurrentState == EPlayerState.CLIMB || 
            ECurrentState == EPlayerState.ROLL ||
            ECurrentState == EPlayerState.CAST_SPAWN)
        {
            return;
        }
        #endregion

        #region ACTUAL_DAMAGE
        int beforeDamageHP;
        int afterDamageHP;
        Stat.OnHitted(damage, out beforeDamageHP, out afterDamageHP);
        HitUIEventHandler?.Invoke(damage, beforeDamageHP, afterDamageHP);
        HitEffectEventHandler?.Invoke(EPlayerState.HITTED);
        HitEventHandler?.Invoke();
        if (Stat.HP <= 0)
        {
            ChangeState(EPlayerState.DIE);
        }
        else
        {
            ChangeState(EPlayerState.HITTED);
        }
        #endregion
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
                PlayMovementEffectAnimation(EPlayerMovementEffect.LAND);
                FootDustParticle.Play();
                break;
            case EPlayerState.NORMAL_ATTACK_1:
                PlayMovementEffectAnimation(EPlayerMovementEffect.NORMAL_ATTACK_1);
                break;
            case EPlayerState.NORMAL_ATTACK_2:
                break;
            case EPlayerState.NORMAL_ATTACK_3:
                break;
            case EPlayerState.CAST_LAUNCH:
                break;
            case EPlayerState.CAST_SPAWN:
                break;
            case EPlayerState.HITTED:
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

    public void PlayMovementEffectAnimation(EPlayerMovementEffect eEffectType)
    {
        MovementEventHandler?.Invoke(eEffectType);
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
        _states[(uint)EPlayerState.HITTED] = new player_states.Hitted(this);
        _states[(uint)EPlayerState.DIE] = new player_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EPlayerState.IDLE]);

        // 6.10 FallState에서도 이단점프 할 수 있도록 하기 위해 추가.
        ((player_states.FallCanTwiceJump)_states[(uint)EPlayerState.FALL]).SubscribeTwiceJumpEventHandler((player_states.Jump)_states[(uint)EPlayerState.JUMP]);
    }

    private bool IsSkipThisFrame()
    {
        if (Managers.Pause.IsPaused || Managers.Dialog.IsTalking)
            return true;
        return false;
    }

}