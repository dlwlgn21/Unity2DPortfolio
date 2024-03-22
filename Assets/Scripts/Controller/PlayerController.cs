using define;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum EPlayerState
{
    IDLE,
    RUN,
    ROLL,
    NORMAL_ATTACK_1,
    NORMAL_ATTACK_2,
    NORMAL_ATTACK_3,
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
    public static KeyCode KeyRight = KeyCode.RightArrow;
    public static KeyCode KeyLeft = KeyCode.LeftArrow;
    public static KeyCode KeyAttack = KeyCode.Z;
    public static KeyCode KeyRoll = KeyCode.V;

    public PlayerStat Stat { get; private set; }
    StateMachine<PlayerController> mStateMachine;
    State<PlayerController>[] mStates;
    EPlayerState meCurrentState = EPlayerState.IDLE;
    public override void Init()
    {
        base.Init();
        Stat = gameObject.GetOrAddComponent<PlayerStat>();
        Managers.Input.KeyboardHandler -= OnKeyboardArrowPressed;
        Managers.Input.KeyboardHandler += OnKeyboardArrowPressed;
        ELookDir = ECharacterLookDir.RIGHT;
        NormalAttackRange = 1f;
    }


    void Update()
    {
        mStateMachine.Excute();
    }

    public void OnNoramlAttack1ValidSlashed() { ((player_states.NormalAttackState)mStates[(uint)EPlayerState.NORMAL_ATTACK_1]).IsHitMonsters(EPlayerNoramlAttackType.ATTACK_1); }
    public void OnNoramlAttack2ValidSlashed() { ((player_states.NormalAttackState)mStates[(uint)EPlayerState.NORMAL_ATTACK_1]).IsHitMonsters(EPlayerNoramlAttackType.ATTACK_2); }
    public void OnNoramlAttack3ValidSlashed() { ((player_states.NormalAttackState)mStates[(uint)EPlayerState.NORMAL_ATTACK_1]).IsHitMonsters(EPlayerNoramlAttackType.ATTACK_3); }
    public void OnKeyboardArrowPressed()
    {
        if (meCurrentState == EPlayerState.ROLL ||
            meCurrentState == EPlayerState.NORMAL_ATTACK_1 ||
            meCurrentState == EPlayerState.NORMAL_ATTACK_2 ||
            meCurrentState == EPlayerState.NORMAL_ATTACK_3)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NormalAttackPoint.localPosition = mCachedAttackPointLocalLeftPos;
            ELookDir = ECharacterLookDir.LEFT;
            mSpriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NormalAttackPoint.localPosition = mCachedAttackPointLocalRightPos;
            ELookDir = ECharacterLookDir.RIGHT;
            mSpriteRenderer.flipX = false;
        }
    }
    public void ChangeState(EPlayerState eChangingState)
    {
        meCurrentState = eChangingState;
        mStateMachine.ChangeState(mStates[(uint)eChangingState]);
        switch (eChangingState)
        {
            case EPlayerState.IDLE:
                Animator.Play("Idle");
                break;
            case EPlayerState.RUN:
                Animator.Play("Run");
                break;
            case EPlayerState.ROLL:
                Animator.Play("Roll");
                break;
            case EPlayerState.NORMAL_ATTACK_1:
                Animator.Play("NormalAttack1");
                break;
            case EPlayerState.NORMAL_ATTACK_2:
                Animator.Play("NormalAttack2");
                break;
            case EPlayerState.NORMAL_ATTACK_3:
                Animator.Play("NormalAttack3");
                break;
            default:
                Debug.Assert(false, "You must Cheking Swich case");
                break;
        }
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
        mStateMachine.Init(this, mStates[(uint)EPlayerState.IDLE]);
    }
}