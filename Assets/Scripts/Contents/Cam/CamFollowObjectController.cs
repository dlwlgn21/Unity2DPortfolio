using define;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
public class CamFollowObjectController : MonoBehaviour
{
    private Transform _playerTransform;
    private PlayerController _pc;
    private const float FLIP_Y_ROTATION_TIME_IN_SEC = 0.15f;
    private const float LOOK_UP_DOWN_Y_DIST = 2f;
    private const float LOOK_UP_DOWN_LERP_SPEED = 0.2f;
    bool _isEnterEnableLookUpDownZone = false;
    private void Awake()
    {
        CamLookUpDownZoneDetectionController.PlayerEnterEventHandler -= OnEnterEnableLookUpDownZone;
        CamLookUpDownZoneDetectionController.PlayerEnterEventHandler += OnEnterEnableLookUpDownZone;
        CamLookUpDownZoneDetectionController.PlayerExitEventHandler -= OnExitEnableLookUpDownZone;
        CamLookUpDownZoneDetectionController.PlayerExitEventHandler += OnExitEnableLookUpDownZone;
    }
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
        if (_isEnterEnableLookUpDownZone)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position = Lerp(-LOOK_UP_DOWN_Y_DIST);
                return;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position = Lerp(LOOK_UP_DOWN_Y_DIST);
                return;
            }
        }
        transform.position = _playerTransform.position;
    }


    void OnEnterEnableLookUpDownZone()
    {
        _isEnterEnableLookUpDownZone = true;
    }
    void OnExitEnableLookUpDownZone()
    {
        _isEnterEnableLookUpDownZone = false;
    }
    Vector3 DetermineEndRotation()
    {
        if (_pc.ELookDir == ECharacterLookDir.Left)
        {
            return new Vector3(0f, 180f, 0f);
        }
        return Vector3.zero;
    }

    Vector2 Lerp(float yDiff)
    {
        return Vector2.Lerp(transform.position, new Vector2(_playerTransform.position.x, _playerTransform.position.y + yDiff), LOOK_UP_DOWN_LERP_SPEED);
    }
}
