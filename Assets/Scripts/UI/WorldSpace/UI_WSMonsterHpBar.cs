using DG.Tweening;
using UnityEngine;

public class UI_WSMonsterHpBar : UI_HealthBar
{
    private const float SCALE_TW_DURATION = 0.2f;
    private Vector3 _originalLocalScale;
    private RectTransform _rectTransform;
    private Vector3 _originalRectTransformScale;

    bool _isDieState = false;
    private void Start()
    {
        SetFullHpBarRatio();
        if (_rectTransform == null)
            AssginComponentsAndInitVariables();
    }

    public void InitForRespawn()
    {
        Init();
        InitScale();
    }

    public override void Init()
    {
        if (_rectTransform == null)
            AssginComponentsAndInitVariables();
        _isDieState = false;
    }

    private void Update()
    {
        if (!_isDieState)
        {
            if (transform.parent.localRotation.eulerAngles.y > 0f)
            {
                transform.localScale = new Vector3(-1f, _originalLocalScale.y, _originalLocalScale.z);
            }
            else
            {
                transform.localScale = new Vector3(1f, _originalLocalScale.y, _originalLocalScale.z);
            }
        }
    }

    public void StartZeroScaleTW()
    {
        Managers.Tween.EndToZeroScaleTW(_rectTransform, SCALE_TW_DURATION);
        _isDieState = true;
    }
    private void AssginComponentsAndInitVariables()
    {
        _stat = transform.parent.gameObject.GetComponent<MonsterStat>();
        Debug.Assert(_stat != null);
        _rectTransform = GetComponent<RectTransform>();
        _originalLocalScale = transform.localScale;
        _originalRectTransformScale = _rectTransform.localScale;
        _rectTransform.localScale = _originalRectTransformScale;
    }

    private void InitScale()
    {
        transform.localScale = _originalLocalScale;
    }
}