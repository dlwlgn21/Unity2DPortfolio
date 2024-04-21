using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    Transform mTransform;
    Vector3 mOriginalScale;
    float mScaleDuration = 0.5f;
    private void Awake()
    {
        mTransform = GetComponent<Transform>();
        mOriginalScale = mTransform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)  { DoScaleTween(mOriginalScale.x + 0.05f, Ease.OutElastic); }
    public void OnPointerExit(PointerEventData eventData)   { DoScaleTween(mOriginalScale.x, Ease.OutElastic); }
    public void OnSelect(BaseEventData eventData)           { DoScaleTween(mOriginalScale.x + 0.05f, Ease.OutElastic); }
    public void OnDeselect(BaseEventData eventData)         { DoScaleTween(mOriginalScale.x, Ease.OutElastic); }

    protected void DoScaleTween(float endScale, Ease ease)
    {
        mTransform
            .DOScale(endScale, mScaleDuration)
            .SetEase(ease);
    }

}
