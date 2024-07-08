using define;
using UnityEngine;

public enum EPlayerMovementEffect
{ 
    JUMP,
    ROLL,
    NORMAL_ATTACK_1,
    NORMAL_ATTACK_LAND,
    LAND,
}

public class PlayerMovementEffectController : WorldSpaceEffectController
{
    private const string JUMP_LAND_KEY = "JumpEffect";
    private const string ROLL_KEY = "RollEffect";
    private const string DASH_ATTACK_KEY = "DashAttackEffect";
    private const string DASH_ATTACK_LAND_KEY = "DashAttackStopEffect";
    private const float DASH_ATTACK_LAND_X_OFFSET = 0.4f;
    private const float LAND_Y_OFFSET = 0.3f;

    private void Awake()
    {
        PlayerController.MovementEffectEventHandler += OnPlayerMovemnt;
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
    }

    private void OnPlayerMovemnt(EPlayerMovementEffect eType, ECharacterLookDir eLookDir, Vector2 pos)
    {
        SetComponentsEnabled(true);
        transform.position = pos;
        _spriteRenderer.flipX = false;

        switch (eType)
        {
            case EPlayerMovementEffect.JUMP:
                _animator.Play(JUMP_LAND_KEY, -1, 0f);
                break;
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
                if (eLookDir == ECharacterLookDir.LEFT)
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
                _animator.Play(JUMP_LAND_KEY, -1, 0f);
                break;
        }
    }

    private void FlipSpriteIfLeft(ECharacterLookDir eLookDir)
    {
        if (eLookDir == ECharacterLookDir.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
    }

}
