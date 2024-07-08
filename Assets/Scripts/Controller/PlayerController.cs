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
    HITTED_MELLE_ATTACK,
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

    public CamFollowObject CamFollowObject;
    public BoxCollider2D BoxCollider { get; set; }
    public PlayerStat Stat { get; private set; }
    public EPlayerState ECurrentState { get; private set; }
    public Transform LedgeHeadRayPoint { get; private set; }
    public Transform LedgeBodyRayPoint { get; private set; }
    public Transform SpawnReaperPoint { get; private set; }
    public Transform SpawnShooterPoint { get; private set; }

    private StateMachine<PlayerController> _stateMachine;
    private State<PlayerController>[] _states;

    public bool IsInvincible { get; set; } = false;

    public override void Init()
    {
        base.Init();
        Stat = gameObject.GetOrAddComponent<PlayerStat>();
        BoxCollider = gameObject.GetComponent<BoxCollider2D>();
        ELookDir = ECharacterLookDir.RIGHT;
        LedgeHeadRayPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeHeadRayPoint");
        LedgeBodyRayPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeBodyRayPoint");
        SpawnReaperPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnReaperPoint");
        SpawnShooterPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "SkillSpawnShooterPoint");


        #region SUBSCRIBE_EVENT
        MonsterKnockbackProjectile.KnockbackProjectileHitEventHandler += OnHittedByMonsterKnockbackProjectile;
        MonsterDamageProjectile.DamageProjectileHitEventHandler += OnHittedByMonsterDamageProjectile;
        MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr += OnHittedByMonsterMelleAttack;
        #endregion
    }

    private void OnDestroy()
    {
        MonsterKnockbackProjectile.KnockbackProjectileHitEventHandler -= OnHittedByMonsterKnockbackProjectile;
        MonsterDamageProjectile.DamageProjectileHitEventHandler -= OnHittedByMonsterDamageProjectile;
        MonsterMelleAttack.OnPlayerHittedByMonsterMelleAttackEventHandelr -= OnHittedByMonsterMelleAttack;
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
        #region SKILLS
        if (Input.GetKeyDown(KeyRoll))
        {
            PlayerSkillKeyDownEventHandler?.Invoke(EPlayerSkill.ROLL);
        }
        else if (Input.GetKeyDown(KeySpawnReaper))
        {
            PlayerSkillKeyDownEventHandler?.Invoke(EPlayerSkill.SPAWN_REAPER);
        }
        else if (Input.GetKeyDown(KeyLaunchBomb))
        {
            PlayerSkillKeyDownEventHandler?.Invoke(EPlayerSkill.SPAWN_SHOOTER);
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

    #endregion

    #region HITTED_BY_MONSTERS
    private void OnHittedByMonsterMelleAttack(int damage, BaseMonsterController monContorller)
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
            ProcessBlockSuccess();
            monContorller.OnPlayerBlockSuccess();
            return;
        }
        #endregion

        #region IGNORE
        if (ECurrentState == EPlayerState.BLOCK_SUCESS ||
            ECurrentState == EPlayerState.CLIMB ||
            ECurrentState == EPlayerState.ROLL ||
            ECurrentState == EPlayerState.CAST_SPAWN)
        {
            return;
        }
        #endregion

        ActualDamgedFromMonsterAttack(damage, monContorller.ELookDir);
    }


    public void OnHittedByMonsterKnockbackProjectile(ECharacterLookDir eLuanchDir, Vector2 force)
    {
        if (IsInvincibleOrBlockSucess())
        {
            return;
        }
        if (IsChangeStateToBlockSucess())
        {
            return;
        }

        ChangeState(EPlayerState.HITTED_PROJECTILE_KONCKBACK);
        Managers.TimeManager.OnPlayerHittedByMonster();

        HitEffectEventHandler?.Invoke(EPlayerState.HITTED_PROJECTILE_KONCKBACK);
        player_states.HittedProjectileKnockback state = (player_states.HittedProjectileKnockback)_states[(uint)EPlayerState.HITTED_PROJECTILE_KONCKBACK];
        if (eLuanchDir == ECharacterLookDir.LEFT)
        {
            state.AdjustKnockbackForce(new Vector2(-force.x, force.y));
        }
        else
        {
            state.AdjustKnockbackForce(force);
        }
    }

    public void OnHittedByMonsterDamageProjectile(ECharacterLookDir eLuanchDir, int damage)
    {
        if (IsInvincibleOrBlockSucess())
        {
            return;
        }
        if (IsChangeStateToBlockSucess())
        {
            return;
        }
        ActualDamgedFromMonsterAttack(damage, eLuanchDir);
    }
    #endregion

    public void ProcessStatusEffect(NormalMonsterAttackStatusEffect effct)
    {
        //effct.OnPlayerHitted(this);
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
                PlayMovementEffectAnimation(EPlayerMovementEffect.LAND, ELookDir, transform.position);
                break;
            case EPlayerState.CLIMB:
                break;
            case EPlayerState.FALL:
                break;
            case EPlayerState.FALL_TO_TWICE_JUMP:
                break;
            case EPlayerState.TWICE_JUMP_TO_FALL:
                PlayMovementEffectAnimation(EPlayerMovementEffect.LAND, ELookDir, transform.position);
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

    private void ActualDamgedFromMonsterAttack(int damge, ECharacterLookDir eMonsterLookDir)
    {
        #region ACTUAL_DAMAGE
        int beforeDamageHP;
        int afterDamageHP;
        Stat.OnHitted(damge, out beforeDamageHP, out afterDamageHP);
        HitUIEventHandler?.Invoke(damge, beforeDamageHP, afterDamageHP);
        HitEffectEventHandler?.Invoke(EPlayerState.HITTED_MELLE_ATTACK);
        Managers.TimeManager.OnPlayerHittedByMonster();
        if (Stat.HP <= 0)
        {
            ChangeState(EPlayerState.DIE);
        }
        else
        {
            ChangeState(EPlayerState.HITTED_MELLE_ATTACK);
        }
        player_states.BaseHitted state = (player_states.BaseHitted)_states[(uint)EPlayerState.HITTED_MELLE_ATTACK];

        Debug.Assert(state != null);
        if (eMonsterLookDir == ECharacterLookDir.LEFT)
        {
            state.AdjustKnockbackForce(new Vector2(-2, 2));
        }
        else
        {
            state.AdjustKnockbackForce(new Vector2(2, 2));
        }
        #endregion
    }

    private void ProcessBlockSuccess()
    {
        HitEffectEventHandler?.Invoke(EPlayerState.BLOCK_SUCESS);
        ChangeState(EPlayerState.BLOCK_SUCESS);
    }
    private bool IsChangeStateToBlockSucess()
    {
        if (ECurrentState == EPlayerState.BLOCKING)
        {
            ProcessBlockSuccess();
            return true;
        }
        return false;
    }

    private bool IsInvincibleOrBlockSucess()
    {
        if (IsInvincible || ECurrentState == EPlayerState.BLOCK_SUCESS)
        {
            return true;
        }
        return false;
    }

}