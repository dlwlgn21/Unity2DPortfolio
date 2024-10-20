using System;
using UnityEngine;
using UnityEngine.Events;

enum EDoorState
{ 
    Idle,
    Opnening,
    Open,
    Closing,
}

public sealed class DoorController : BaseInteractableController
{
    static public UnityAction DeniedDoorOpenEventHandler;
    Animator _animator;
    EDoorState _eDoorState;
    bool _isDoorOpenOnce = false;
    bool _isDoorClosed = false;

    Func<bool> _condition; 

    private void Start()
    {
        base.Init();
        _animator = GetComponent<Animator>();
        _eDoorState = EDoorState.Idle;
    }

    private void ChangeState(EDoorState eState)
    {
        _eDoorState = eState;
        switch (_eDoorState)
        {
            case EDoorState.Idle:
                _animator.Play("DoorIdle");
                break;
            case EDoorState.Opnening:
                Managers.Sound.Play(DataManager.SFX_ENV_DOOR_OPEN);
                _animator.Play("DoorOpening");
                break;
            case EDoorState.Open:
                _animator.Play("DoorOpen");
                break;
            case EDoorState.Closing:
                _animator.Play("DoorClosing");
                break;
        }
    }

    public void SetConditionFunc(Func<bool> condition)
    {
        if (_condition == null)
            _condition = condition;
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
            ChangeState(EDoorState.Closing);
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_condition != null)
                {
                    if (_condition.Invoke())
                    {
                        OpenDoor();
                    }
                    else
                    {
                        if (DeniedDoorOpenEventHandler != null)
                            DeniedDoorOpenEventHandler.Invoke();
                    }
                }
                else
                {
                     OpenDoor();
                }
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
    { ChangeState(EDoorState.Open); }
    private void OnDoorClosingAnimEnd()
    {
        ChangeState(EDoorState.Idle);
        _isDoorOpenOnce = true;
    }
    void OpenDoor()
    {
        _interactKey.UnactiveInteractKey();
        ChangeState(EDoorState.Opnening);
        _isDoorOpenOnce = true;
    }
    private void OnDestroy()
    {
        if (_condition != null)
            _condition = null;
    }
}
