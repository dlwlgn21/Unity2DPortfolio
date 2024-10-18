using define;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
public class CamFollowObject : MonoBehaviour
{
    private Transform _playerTransform;
    private PlayerController _pc;
    private const float FLIP_Y_ROTATION_TIME_IN_SEC = 0.15f;
    private const float LOOK_UP_OR_DOWN_Y_DIST = 2f;
    public void CallTurn()
    {
        transform.DORotate(DetermineEndRotation(), FLIP_Y_ROTATION_TIME_IN_SEC).SetEase(Ease.InOutSine);
    }
    private void Start()
    {
        if (_pc == null)
        {
            _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _playerTransform = _pc.gameObject.transform;
        }
    }
  
    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector2(_playerTransform.position.x, _playerTransform.position.y - LOOK_UP_OR_DOWN_Y_DIST);
            return;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector2(_playerTransform.position.x, _playerTransform.position.y + LOOK_UP_OR_DOWN_Y_DIST);
            return;
        }

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
}
