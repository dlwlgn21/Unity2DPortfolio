using define;
using CameraShake;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public enum EPlayerState
{
    IDLE,
    RUN,
    ROLL,
    NORMAL_ATTACK_1,
    NORMAL_ATTACK_2,
    NORMAL_ATTACK_3,
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
    public static KeyCode KeyRoll = KeyCode.V;

    
    public PlayerStat Stat { get; private set; }
    public EPlayerState meCurrentState { get; private set; }
    
    StateMachine<PlayerController> mStateMachine;
    State<PlayerController>[] mStates;

    public override void Init()
    {
        base.Init();
        Stat = gameObject.GetOrAddComponent<PlayerStat>();
        Managers.Input.KeyboardHandler -= OnKeyboardArrowPressed;
        Managers.Input.KeyboardHandler += OnKeyboardArrowPressed;
        ELookDir = ECharacterLookDir.RIGHT;
        NormalAttackRange = 1f;
        mHealthBar = Utill.GetComponentInChildrenOrNull<UIPlayerHPBar>(gameObject, "PlayerHpBar");
    }
    void Update()
    {
        mStateMachine.Excute();
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
    public void OnNoramlAttack1ValidSlashed() { Debug.Assert(meCurrentState == EPlayerState.NORMAL_ATTACK_1); ((player_states.NormalAttackState)mStates[(uint)EPlayerState.NORMAL_ATTACK_1]).DamageHittedMonsters(); }
    public void OnNoramlAttack2ValidSlashed() { Debug.Assert(meCurrentState == EPlayerState.NORMAL_ATTACK_2); ((player_states.NormalAttackState)mStates[(uint)EPlayerState.NORMAL_ATTACK_2]).DamageHittedMonsters(); }
    public void OnNoramlAttack3ValidSlashed() { Debug.Assert(meCurrentState == EPlayerState.NORMAL_ATTACK_3); ((player_states.NormalAttackState)mStates[(uint)EPlayerState.NORMAL_ATTACK_3]).DamageHittedMonsters(); }


    public void OnHittedAnimFullyPlayed()
    {
        player_states.Hitted hittedState = (player_states.Hitted)mStates[(uint)EPlayerState.HITTED];
        hittedState.OnHittedAnimFullyPlayed(this);
    }
    public void OnRollAnimFullyPlayed()
    {
        player_states.Roll rollState = (player_states.Roll)mStates[(uint)EPlayerState.ROLL];
        rollState.OnRollAnimFullyPlayed(this);
        FootDustParticle.Play();
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


    public void OnKeyboardArrowPressed()
    {
        if (meCurrentState == EPlayerState.NORMAL_ATTACK_1 ||
            meCurrentState == EPlayerState.NORMAL_ATTACK_2 ||
            meCurrentState == EPlayerState.NORMAL_ATTACK_3)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NormalAttackPoint.localPosition = mCachedAttackPointLocalLeftPos;
            ELookDir = ECharacterLookDir.LEFT;
            SpriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NormalAttackPoint.localPosition = mCachedAttackPointLocalRightPos;
            ELookDir = ECharacterLookDir.RIGHT;
            SpriteRenderer.flipX = false;
        }
    }

    public void OnHitted(int damage) 
    {
        // Blocking Section
        if (meCurrentState == EPlayerState.BLOCKING)
        {
            HitEffectAniamtor.gameObject.SetActive(true);
            HitEffectAniamtor.Play(HIT_EFFECT_3_KEY, -1, 0f);
            ChangeState(EPlayerState.BLOCK_SUCESS);
            return;
        }
        if (meCurrentState == EPlayerState.BLOCK_SUCESS)
            return;

        // Damage Section
        int actualDamage = Mathf.Max(1, damage - Stat.Defence);
        Stat.HP -= actualDamage;
        if (Stat.HP <= 0)
            ChangeState(EPlayerState.DIE);
        else
            ChangeState(EPlayerState.HITTED);
        ShakeCamera(EHitCameraShake.STRONG_SHAKE_2D);
        ShowDamagePopup(damage);
        Managers.HitParticle.Play(transform.position);
    }


    public void ChangeState(EPlayerState eChangingState)
    {
        meCurrentState = eChangingState;
        mStateMachine.ChangeState(mStates[(uint)eChangingState]);
    }


    protected override void initStates()
    {
        mStateMachine = new StateMachine<PlayerController>();
        mStates = new State<PlayerController>[(uint)EPlayerState.COUNT];
        mStates[(uint)EPlayerState.IDLE] = new player_states.Idle();
        mStates[(uint)EPlayerState.RUN] = new player_states.Run();
        mStates[(uint)EPlayerState.ROLL] = new player_states.Roll();
        mStates[(uint)EPlayerState.NORMAL_ATTACK_1] = new player_states.NormalAttack1();
        mStates[(uint)EPlayerState.NORMAL_ATTACK_2] = new player_states.NormalAttack2();
        mStates[(uint)EPlayerState.NORMAL_ATTACK_3] = new player_states.NormalAttack3();
        mStates[(uint)EPlayerState.BLOCKING] = new player_states.Blocking();
        mStates[(uint)EPlayerState.BLOCK_SUCESS] = new player_states.BlockSuccess();
        mStates[(uint)EPlayerState.HITTED] = new player_states.Hitted();
        mStates[(uint)EPlayerState.DIE] = new player_states.Die();
        mStateMachine.Init(this, mStates[(uint)EPlayerState.IDLE]);
    }


}