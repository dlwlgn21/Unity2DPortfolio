using define;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
public class CamFollowObject : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private PlayerController _pc;
    private const float FLIP_Y_ROTATION_TIME_IN_SEC = 0.3f;

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
        transform.position = _playerTransform.position;
    }
    private Vector3 DetermineEndRotation()
    {
        if (_pc.ELookDir == ECharacterLookDir.LEFT)
        {
            return new Vector3(0f, 180f, 0f);
        }
        return Vector3.zero;
    }
}
