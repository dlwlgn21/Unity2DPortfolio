using define;
using player_states;
using UnityEngine;

public enum EPlayerMovementEffect
{ 
    JUMP,
    ROLL,
    NORMAL_ATTACK_1,
    NORMAL_ATTACK_LAND,
    LAND,
}

public class PlayerMovementAnimController : WorldSpaceAnimController
{
    private const string JUMP_AND_LAND_KEY = "JumpEffect";
    private const string ROLL_KEY = "RollEffect";
    private const string DASH_ATTACK_KEY = "DashAttackEffect";
    private const string DASH_ATTACK_LAND_KEY = "DashAttackStopEffect";
    private const float DASH_ATTACK_LAND_X_OFFSET = 0.4f;
    private const float LAND_Y_OFFSET = 0.215f;

    [SerializeField] private float _jumpYOffset;
    private void Awake()
    {
        PlayerController.MovementEffectEventHandler += OnPlayerMovemnt;
        InAir.JumpEventHandler += OnPlayerJump;
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        AssignComponents();
        SetComponentsEnabled(false);
    }

    private void OnDestroy()
    {
        PlayerController.MovementEffectEventHandler -= OnPlayerMovemnt;
        InAir.JumpEventHandler -= OnPlayerJump;
    }

    private void OnPlayerJump(Vector2 pos)
    {
        SetForPlayAnimation(pos);
        _animator.Play(JUMP_AND_LAND_KEY, -1, 0f);
    }
    private void OnPlayerMovemnt(EPlayerMovementEffect eType, ECharacterLookDir eLookDir, Vector2 pos)
    {
        SetForPlayAnimation(pos);
        switch (eType)
        {
            case EPlayerMovementEffect.ROLL:
                FlipSpriteIfLeft(eLookDir);
                _animator.Play(ROLL_KEY, -1, 0f);
                break;
            case EPlayerMovementEffect.NORMAL_ATTACK_1:
                FlipSpriteIfLeft(eLookDir);
                _animator.Play(DASH_ATTACK_KEY, -1, 0f);
                break;
            case EPlayerMovementEffect.NORMAL_ATTACK_LAND:
                FlipSpriteIfLeft(eLookDir);
                if (eLookDir == ECharacterLookDir.Left)
                {
                    transform.position = new Vector2(pos.x - DASH_ATTACK_LAND_X_OFFSET, pos.y);
                }
                else
                {
                    transform.position = new Vector2(pos.x + DASH_ATTACK_LAND_X_OFFSET, pos.y);
                }
                _animator.Play(DASH_ATTACK_LAND_KEY, -1, 0f);
                break;
            case EPlayerMovementEffect.LAND:
                transform.position = new Vector2(pos.x, pos.y - LAND_Y_OFFSET);
                _animator.Play(JUMP_AND_LAND_KEY, -1, 0f);
                break;
        }
    }

    private void FlipSpriteIfLeft(ECharacterLookDir eLookDir)
    {
        if (eLookDir == ECharacterLookDir.Left)
        {
            _spriteRenderer.flipX = true;
        }
    }


    private void SetForPlayAnimation(Vector2 pos)
    {
        SetComponentsEnabled(true);
        transform.position = pos;
        _spriteRenderer.flipX = false;
    }
}
