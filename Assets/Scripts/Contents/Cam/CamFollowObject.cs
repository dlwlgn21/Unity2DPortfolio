using define;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
public class CamFollowObject : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private PlayerController _pc;
    private const float FLIP_Y_ROTATION_TIME_IN_SEC = 0.15f;
    //private const float LOOK_UP_OR_DOWN_Y_DIST = 3f;

    //bool _isLookUpOrDown = false;
    //bool _isLookUpOrDownCancled = false;
    public void CallTurn()
    {
        transform.DORotate(DetermineEndRotation(), FLIP_Y_ROTATION_TIME_IN_SEC).SetEase(Ease.InOutSine);
    }
    private void Awake()
    {
        _pc = _playerTransform.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        //if (_isLookUpOrDownCancled)
        //    return;
        //if (_isLookUpOrDown)
        //{
        //    if (_pc.ECurrentState != EPlayerState.Idle)
        //    {
        //        Debug.Log("DOTween.Pause(transform)");
        //        transform.DOKill();
        //        transform.DOMove(_playerTransform.position, 0.2f).OnComplete(OnCompleteCancleLookUpOrDown);
        //        _isLookUpOrDown = false;
        //        _isLookUpOrDownCancled = true;
        //    }
        //    return;
        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    if (_pc.ECurrentState == EPlayerState.Idle && !_isLookUpOrDown)
        //    {
        //        DoMoveYTW(_playerTransform.position.y + LOOK_UP_OR_DOWN_Y_DIST);
        //        return;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    if (_pc.ECurrentState == EPlayerState.Idle && !_isLookUpOrDown)
        //    {
        //        DoMoveYTW(_playerTransform.position.y - LOOK_UP_OR_DOWN_Y_DIST);
        //        return;
        //    }
        //}

        transform.position = _playerTransform.position;
    }
    Vector3 DetermineEndRotation()
    {
        if (_pc.ELookDir == ECharacterLookDir.Left)
        {
            return new Vector3(0f, 180f, 0f);
        }
        return Vector3.zero;
    }

    //void DoMoveYTW(float endValue)
    //{
    //    transform.DOMoveY(endValue, 1f).SetEase(Ease.InOutSine).OnComplete(OnCompleteLookUpOrDownTW);
    //    _isLookUpOrDown = true;
    //}

    //void OnCompleteLookUpOrDownTW()
    //{
    //    transform.DOMove(_playerTransform.position, 0.5f).OnComplete(OnCompleteRestorePostionTW);
    //}
    //void OnCompleteRestorePostionTW()
    //{
    //    _isLookUpOrDown = false;
    //}

    //void OnCompleteCancleLookUpOrDown()
    //{
    //    _isLookUpOrDownCancled = false;
    //}
}
