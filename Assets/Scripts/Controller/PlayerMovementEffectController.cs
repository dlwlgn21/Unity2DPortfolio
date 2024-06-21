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

    private PlayerController _pc;

    private void Awake()
    {
        PlayerController.MovementEventHandler += OnPlayerMovemnt;
    }
    public void Start()
    {
        AssignComponents();
        SetComponentsEnabled(false);
        _pc = transform.parent.GetComponent<PlayerController>();
    }

    private void Update()
    {
        FixPosition();
    }

    private void OnDestroy()
    {
        PlayerController.MovementEventHandler -= OnPlayerMovemnt;
    }

    private void OnPlayerMovemnt(EPlayerMovementEffect eType)
    {
        SetComponentsEnabled(true);
        Vector2 pos = _pc.transform.position;
        _fixedWorldPos = pos;
        ECharacterLookDir eLookDir = _pc.ELookDir;
        switch (eType)
        {
            case EPlayerMovementEffect.JUMP:
                _animator.Play(JUMP_LAND_KEY, -1, 0);
                break;
            case EPlayerMovementEffect.ROLL:
                _animator.Play(ROLL_KEY, -1, 0);
                break;
            case EPlayerMovementEffect.NORMAL_ATTACK_1:
                _animator.Play(DASH_ATTACK_KEY, -1, 0);
                break;
            case EPlayerMovementEffect.NORMAL_ATTACK_LAND:
                if (eLookDir == ECharacterLookDir.LEFT)
                {
                    _fixedWorldPos = new Vector2(pos.x - DASH_ATTACK_LAND_X_OFFSET, pos.y);
                }
                else
                {
                    _fixedWorldPos = new Vector2(pos.x + DASH_ATTACK_LAND_X_OFFSET, pos.y);
                }
                _animator.Play(DASH_ATTACK_LAND_KEY, -1, 0);
                break;
            case EPlayerMovementEffect.LAND:
                _fixedWorldPos = new Vector2(pos.x, pos.y - LAND_Y_OFFSET);
                _animator.Play(JUMP_LAND_KEY, -1, 0);
                break;
        }
    }
}
