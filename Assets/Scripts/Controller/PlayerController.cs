using define;
using CameraShake;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;
using JetBrains.Annotations;
using System.Xml;
using DG.Tweening;

public enum EPlayerState
{
    IDLE,
    RUN,
    ROLL,
    JUMP,
    CLIMB,
    FALL,
    LAND,
    NORMAL_ATTACK_1,
    NORMAL_ATTACK_2,
    NORMAL_ATTACK_3,
    LAUNCH,
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
    ATTACK_3
}
public class PlayerController : BaseCharacterController
{
    public static KeyCode KeyUp = KeyCode.UpArrow;
    public static KeyCode KeyDown = KeyCode.DownArrow;
    public static KeyCode KeyRight = KeyCode.RightArrow;
    public static KeyCode KeyLeft = KeyCode.LeftArrow;
    public static KeyCode KeyAttack = KeyCode.Z;
    public static KeyCode KeyBlock = KeyCode.X;
    public static KeyCode KeyRoll = KeyCode.C;
    public static KeyCode KeyJump = KeyCode.Space;
    public static KeyCode KeyLaunchBomb = KeyCode.A;

    [SerializeField] private UIPlayerHpBar _hpBar;
    public UIPlayerCoolTimer RollCoolTimerImg;
    public UIPlayerCoolTimer BombCoolTimerImg;

    public CamFollowObject CamFollowObject;
    public BoxCollider2D BoxCollider { get; set; }
    public ParticleSystem JumpParticle { get; set; }
    public PlayerStat Stat { get; private set; }
    public EPlayerState ECurrentState { get; private set; }
    public Transform LedgeCheckPoint { get; private set; }
    public Transform LaunchPoint { get; set; }
    public Vector3 CachedLaunchPointLocalRightPos { get; set; }
    public Vector3 CachedLaunchPointLocalLeftPos { get; set; }

    private StateMachine<PlayerController> _stateMachine;
    private State<PlayerController>[] _states;


    private TestThrow _testThrow;

    // RollCoolTime
    public const float ROLL_INIT_COOL_TIME = 3f;
    public float RollCollTime { get; private set; } = ROLL_INIT_COOL_TIME;
    public float RollCollTimer { get; set; } = ROLL_INIT_COOL_TIME;
    public bool IsPossibleRoll { get; set; } = true;


    // BombCoolTime
    public const float BOMB_INIT_COOL_TIME = 2f;
    public float BombCollTime { get; private set; } = BOMB_INIT_COOL_TIME;
    public float BombCollTimer { get; set; } = BOMB_INIT_COOL_TIME;
    public bool IsPossibleLaunchBomb { get; set; } = true;

    public override void Init()
    {
        base.Init();
        Stat = gameObject.GetOrAddComponent<PlayerStat>();
        BoxCollider = gameObject.GetComponent<BoxCollider2D>();
        ELookDir = ECharacterLookDir.RIGHT;
        NormalAttackRange = 1f;
        LedgeCheckPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LedgeCheckPoint");
        JumpParticle = Utill.GetComponentInChildrenOrNull<ParticleSystem>(gameObject, "JumpParticle");
        Debug.Assert(LedgeCheckPoint != null && JumpParticle != null);
        Debug.Assert(_hpBar != null);
        
        // LaunchPoint
        LaunchPoint = transform.Find("LaunchPoint").gameObject.transform;
        CachedLaunchPointLocalRightPos = LaunchPoint.localPosition;
        Vector3 leftPos = LaunchPoint.localPosition;
        leftPos.x = -leftPos.x;
        CachedLaunchPointLocalLeftPos = leftPos;

        // Throw
        _testThrow = GetComponent<TestThrow>();
    }
    void FixedUpdate()
    {
        if (IsSkipThisFrame())
        {
            RigidBody.velocity = Vector2.zero;
            ChangeState(EPlayerState.IDLE);
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

        if (IsSkipThisFrame())
        {
            ChangeState(EPlayerState.IDLE);
            return;
        }
        _stateMachine.Excute();
    }

    public void ShakeCamera(EHitCameraShake eShakeType)
    {
        // TODO : 시네마신 카메라 세팅 완료 후, 반드시 롤백 되어야 함.
        switch (eShakeType)
        {
            case EHitCameraShake.WEAK_SHAKE_2D:
                CameraShaker.Presets.ShortShake2D();
                break;
            case EHitCameraShake.STRONG_SHAKE_2D:
                CameraShaker.Presets.Explosion2D();
                break;
            case EHitCameraShake.WEAK_SHAKE_3D:
                CameraShaker.Presets.ShortShake3D();
                break;
            case EHitCameraShake.STRONG_SHAKE_3D:
                CameraShaker.Presets.Explosion3D();
                break;
        }
    }

    #region ANIM_CALL_BACK
    public void OnNoramlAttack1ValidSlashed() { Debug.Assert(ECurrentState == EPlayerState.NORMAL_ATTACK_1); ((player_states.NormalAttackState)_states[(uint)EPlayerState.NORMAL_ATTACK_1]).DamageHittedMonsters(); }
    public void OnNoramlAttack2ValidSlashed() { Debug.Assert(ECurrentState == EPlayerState.NORMAL_ATTACK_2); ((player_states.NormalAttackState)_states[(uint)EPlayerState.NORMAL_ATTACK_2]).DamageHittedMonsters(); }
    public void OnNoramlAttack3ValidSlashed() { Debug.Assert(ECurrentState == EPlayerState.NORMAL_ATTACK_3); ((player_states.NormalAttackState)_states[(uint)EPlayerState.NORMAL_ATTACK_3]).DamageHittedMonsters(); }

    public void OnValidLaunched() 
    { 
        Debug.Assert(ECurrentState == EPlayerState.LAUNCH);
        _testThrow.LauchBomb(ELookDir, LaunchPoint.position);
    }

    public void OnHittedAnimFullyPlayed()
    {
        player_states.Hitted hittedState = (player_states.Hitted)_states[(uint)EPlayerState.HITTED];
        hittedState.OnHittedAnimFullyPlayed();
    }
    public void OnRollAnimFullyPlayed()
    {
        player_states.Roll rollState = (player_states.Roll)_states[(uint)EPlayerState.ROLL];
        rollState.OnRollAnimFullyPlayed(this);
        FootDustParticle.Play();
    }
    public void OnPlayerClimbAnimFullyPlayed()
    {
        player_states.Climb climbState = (player_states.Climb)_states[(uint)EPlayerState.CLIMB];
        climbState.OnClimbAnimFullyPlayed();
    }

    public void OnPlayerLaunchAnimFullyPlayed()
    {
        player_states.Launch launchState = (player_states.Launch)_states[(uint)EPlayerState.LAUNCH];
        launchState.OnLaunchAnimFullyPlayed();
    }

    public void OnPlayerFootStep()
    {
        FootDustParticle.Play();
    }

    public void OnPlayerRoll()
    {
        FootDustParticle.Play();
    }


    #endregion

    public void OnHitted(int damage, BaseMonsterController monContorller) 
    {
        // Blocking Section
        if (ECurrentState == EPlayerState.BLOCKING && ELookDir != monContorller.ELookDir)
        {
            HitEffectAniamtor.gameObject.SetActive(true);
            HitEffectAniamtor.Play(HIT_EFFECT_3_KEY, -1, 0f);
            ChangeState(EPlayerState.BLOCK_SUCESS);
            monContorller.OnPlayerBlockSuccess();
            return;
        }
        if (ECurrentState == EPlayerState.BLOCK_SUCESS || 
            ECurrentState == EPlayerState.CLIMB || 
            ECurrentState == EPlayerState.ROLL)
            return;

        // Damage Section
        int actualDamage = Mathf.Max(0, damage - Stat.Defence);
        _hpBar.DecraseHP(Stat.HP, Stat.HP - actualDamage);
        Stat.HP -= actualDamage;
        
        if (Stat.HP <= 0)
            ChangeState(EPlayerState.DIE);
        else
            ChangeState(EPlayerState.HITTED);
        DamageText.ShowPopup(damage);
        Managers.HitParticle.Play(transform.position);
    }
    public void ChangeState(EPlayerState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
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
        _states[(uint)EPlayerState.FALL] = new player_states.Fall(this);
        _states[(uint)EPlayerState.LAND] = new player_states.Land(this);
        _states[(uint)EPlayerState.NORMAL_ATTACK_1] = new player_states.NormalAttack1(this);
        _states[(uint)EPlayerState.NORMAL_ATTACK_2] = new player_states.NormalAttack2(this);
        _states[(uint)EPlayerState.NORMAL_ATTACK_3] = new player_states.NormalAttack3(this);
        _states[(uint)EPlayerState.LAUNCH] = new player_states.Launch(this);
        _states[(uint)EPlayerState.BLOCKING] = new player_states.Blocking(this);
        _states[(uint)EPlayerState.BLOCK_SUCESS] = new player_states.BlockSuccess(this);
        _states[(uint)EPlayerState.HITTED] = new player_states.Hitted(this);
        _states[(uint)EPlayerState.DIE] = new player_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EPlayerState.IDLE]);
    }

    private bool IsSkipThisFrame()
    {
        if (Managers.Pause.IsPaused || Managers.Dialog.IsTalking)
            return true;
        return false;
    }
    
}