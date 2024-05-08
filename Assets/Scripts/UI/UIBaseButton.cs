using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    Transform _transform;
    Vector3 _originalScale;
    float _scaleDuration = 0.5f;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _originalScale = _transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)  { DoScaleTween(_originalScale.x + 0.05f, Ease.OutElastic); }
    public void OnPointerExit(PointerEventData eventData)   { DoScaleTween(_originalScale.x, Ease.OutElastic); }
    public void OnSelect(BaseEventData eventData)           { DoScaleTween(_originalScale.x + 0.05f, Ease.OutElastic); }
    public void OnDeselect(BaseEventData eventData)         { DoScaleTween(_originalScale.x, Ease.OutElastic); }

    protected void DoScaleTween(float endScale, Ease ease)
    {
        _transform
            .DOScale(endScale, _scaleDuration)
            .SetEase(ease);
    }

}
