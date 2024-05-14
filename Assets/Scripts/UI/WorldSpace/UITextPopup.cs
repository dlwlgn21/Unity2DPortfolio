using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextPopup : MonoBehaviour
{
    private TextMeshPro _text;
    private RectTransform _rectTransform;
    private Vector3 _originalScale;
    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        Debug.Assert(_text != null);
        _rectTransform = GetComponent<RectTransform>();
        Debug.Assert(_rectTransform != null);
        _originalScale = _rectTransform.localScale;
    }

    public void ShowPopup(int damage)
    {
        _rectTransform.localScale = _originalScale;
        _text.text = damage.ToString();
        _text.color = Color.white;
        _rectTransform
            .DOScale(_originalScale.x + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void ShowBackAttackPopup(int damage)
    {
        Debug.Log("UITextPopup.ShowBackAttackPopup()!!");
        _rectTransform.localScale = _originalScale;
        _text.color = Color.red;
        _text.text = damage.ToString();
        _rectTransform
            .DOScale(_originalScale.x + 0.4f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void ShowPopup(string status)
    {
        _rectTransform.localScale = _originalScale;
        _text.color = Color.white;
        _text.text = status;
        _rectTransform
            .DOScale(_originalScale.x + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void OnScaleTweenEnd()
    {
        _rectTransform.localScale = Vector3.zero;
    }
}
