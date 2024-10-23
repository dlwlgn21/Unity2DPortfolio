using define;
using player_states;
using UnityEngine;

public enum EPlayerMovementEffect
{ 
    Jump,
    Roll,
    NormalAttack_1,
    NormalAttackLand,
    Land,
}

public sealed class PlayerMovementAnimController : WorldSpaceAnimController
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
        SetPosAndComponenetsEnableToTrue(pos);
        _animator.Play(JUMP_AND_LAND_KEY, -1, 0f);
    }
    private void OnPlayerMovemnt(EPlayerMovementEffect eType, ECharacterLookDir eLookDir, Vector2 pos)
    {
        SetPosAndComponenetsEnableToTrue(pos);
        switch (eType)
        {
            case EPlayerMovementEffect.Roll:
                FlipSpriteIfLeft(eLookDir);
                _animator.Play(ROLL_KEY, -1, 0f);
                break;
            case EPlayerMovementEffect.NormalAttack_1:
                FlipSpriteIfLeft(eLookDir);
                _animator.Play(DASH_ATTACK_KEY, -1, 0f);
                break;
            case EPlayerMovementEffect.NormalAttackLand:
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
            case EPlayerMovementEffect.Land:
                transform.position = new Vector2(pos.x, pos.y - LAND_Y_OFFSET);
                _animator.Play(JUMP_AND_LAND_KEY, -1, 0f);
                break;
        }
    }

    void FlipSpriteIfLeft(ECharacterLookDir eLookDir)
    {
        if (eLookDir == ECharacterLookDir.Left)
        {
            _spriteRenderer.flipX = true;
        }
    }


    void SetPosAndComponenetsEnableToTrue(Vector2 pos)
    {
        SetComponentsEnabled(true);
        transform.position = pos;
        _spriteRenderer.flipX = false;
    }

    protected override void SetSpriteFlip(ECharacterLookDir eLookDir)
    {
    }
}
