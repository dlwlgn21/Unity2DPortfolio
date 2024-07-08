using UnityEngine;

enum EDoorState
{ 
    IDLE,
    OPENING,
    OPEN,
    CLOSING,
}

public class DoorController : BaseInteractableController
{
    private Animator _animator;
    private EDoorState _eDoorState;
    private bool _isDoorOpenOnce = false;
    private bool _isDoorClosed = false;
    private void Start()
    {
        base.Init();
        _animator = GetComponent<Animator>();
        _eDoorState = EDoorState.IDLE;
    }

    private void ChangeState(EDoorState eState)
    {
        _eDoorState = eState;
        switch (_eDoorState)
        {
            case EDoorState.IDLE:
                _animator.Play("DoorIdle");
                break;
            case EDoorState.OPENING:
                _animator.Play("DoorOpening");
                break;
            case EDoorState.OPEN:
                _animator.Play("DoorOpen");
                break;
            case EDoorState.CLOSING:
                _animator.Play("DoorClosing");
                break;
        }
    }



    public override void OnPlayerEnter(Collider2D collision)
    {
        if (IsPlayerLeftFromDoor(collision.transform.position))
        {
            // Open
            if (_isDoorOpenOnce)
            {
                return;
            }
            _interactKey.ActiveInteractKey();
        }
        else
        {
            // Close
            if (_isDoorClosed)
            {
                return;
            }
            ChangeState(EDoorState.CLOSING);
            _isDoorClosed = true;
        }
    }

    public override void OnPlayerStay(Collider2D collision)
    {
        if (IsPlayerLeftFromDoor(collision.transform.position))
        {
            // Open
            if (_isDoorOpenOnce)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E))
            {
                _interactKey.UnactiveInteractKey();
                ChangeState(EDoorState.OPENING);
                _isDoorOpenOnce = true;
            }
        }
    }

    public override void OnPlayerExit(Collider2D collision)
    {
        if (IsPlayerLeftFromDoor(collision.transform.position))
        {
            // Open
            if (_isDoorOpenOnce)
            {
                return;
            }
            _interactKey.UnactiveInteractKey();
        }
    }

    private bool IsPlayerLeftFromDoor(Vector2 playerPos)
    {
        if (Mathf.Abs(playerPos.x) - Mathf.Abs(transform.position.x) < 0f)
        {
            return true;
        }
        return false;
    }
    private void OnDoorOpeningAnimEnd()
    { ChangeState(EDoorState.OPEN); }
    private void OnDoorClosingAnimEnd()
    {
        ChangeState(EDoorState.IDLE);
        _isDoorOpenOnce = true;
    }
}
