using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEditor.Experimental.GraphView.GraphView;


enum EDoorState
{ 
    IDLE,
    OPENING,
    OPEN,
    CLOSING,
}

public class DoorController : MonoBehaviour
{
    [SerializeField] private float _closeDistance;
    private Transform _eKey;
    private OpenInteractBox _openInteractBox;
    private CloseInteractBox _closeinteractBox;
    private Animator _animator;
    private EDoorState _eDoorState;
    private bool _isDoorOpenOnce = false;
    private void Start()
    {
        _eKey = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "EKey");
        Debug.Assert(_eKey != null);
        _eKey.gameObject.SetActive(false);
        _openInteractBox = Utill.GetComponentInChildrenOrNull<OpenInteractBox>(gameObject, "OpenInteractBox");
        Debug.Assert(_openInteractBox != null);
        _closeinteractBox = Utill.GetComponentInChildrenOrNull<CloseInteractBox>(gameObject, "CloseInteractBox");
        Debug.Assert(_closeinteractBox != null);
        _animator = GetComponent<Animator>();
        _eDoorState = EDoorState.IDLE;
    }

    private void Update()
    {
        if (_isDoorOpenOnce)
            return;

        switch (_eDoorState)
        { 
            case EDoorState.IDLE:
                ProcessIdleState();
                break;
        }
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
    private void ProcessIdleState()
    {
        if (_openInteractBox.IsPressEKey)
            ChangeState(EDoorState.OPENING);
    }

    public void OnPlayerEnterCloseInteractBox()     { ChangeState(EDoorState.CLOSING); }
    public void OnDoorOpeningAnimEnd()              { ChangeState(EDoorState.OPEN); }
    public void OnDoorClosingAnimEnd()      
    { 
        ChangeState(EDoorState.IDLE);
        _isDoorOpenOnce = true;
    }
    public void OnPlayerEnterInteractBox()          { _eKey.gameObject.SetActive(true); }
    public void OnPlayerExitInteractBox()           { _eKey.gameObject.SetActive(false); }
}
