using DG.Tweening;
using UnityEngine;

public class UIWSMonsterHpBar : UIHealthBar
{
    private const float SCALE_TW_DURATION = 0.2f;
    private Transform _parentTransform;
    private Vector3 _originalLocalScale;
    private RectTransform _rectTransform;
    private Vector3 _originalRectTransformScale;
    private void Start()
    {
        SetFullHpBarRatio();
        if (_rectTransform == null)
        {
            AssginComponentsAndInitVariables();
        }
    }
    public override void Init()
    {
        if (_rectTransform == null)
        {
            _parentTransform = transform.parent;
            AssginComponentsAndInitVariables();
        }
        SetFullHpBarRatio();
    }

    private void Update()
    {
        if (_parentTransform.localRotation.eulerAngles.y > 0f)
        {
            transform.localScale = new Vector3(-1f, _originalLocalScale.y, _originalLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1f, _originalLocalScale.y, _originalLocalScale.z);
        }
    }

    public void OnDie()
    {
        _rectTransform.DOScale(0f, SCALE_TW_DURATION).SetEase(Ease.OutElastic);
    }

    private void AssginComponentsAndInitVariables()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalLocalScale = transform.localScale;
        _originalRectTransformScale = _rectTransform.localScale;
        _rectTransform.localScale = _originalRectTransformScale;
    }
}
