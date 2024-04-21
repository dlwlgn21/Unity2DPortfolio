using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextPopup : MonoBehaviour
{
    TextMeshPro mText;
    RectTransform mRectTransform;
    Vector3 mOriginalScale;

    private void Start()
    {
        mText = GetComponent<TextMeshPro>();
        Debug.Assert(mText != null);
        mRectTransform = GetComponent<RectTransform>();
        Debug.Assert(mRectTransform != null);
        mOriginalScale = mRectTransform.localScale;
    }

    public void ShowPopup(int damage)
    {
        mRectTransform.localScale = mOriginalScale;
        mText.text = damage.ToString();
        mRectTransform
            .DOScale(mOriginalScale.x + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void ShowPopup(string status)
    {
        mRectTransform.localScale = mOriginalScale;
        mText.text = status;
        mRectTransform
            .DOScale(mOriginalScale.x + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void OnScaleTweenEnd()
    {
        mRectTransform.localScale = Vector3.zero;
    }

}
