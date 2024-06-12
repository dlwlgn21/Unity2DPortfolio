using DG.Tweening;
using TMPro;
using UnityEngine;

public class UITextPopup : MonoBehaviour
{
    public TextMeshPro Text { get; private set; }
    private RectTransform _rectTransform;
    private Vector3 _originalScale;
    private void Start()
    {
        Text = GetComponent<TextMeshPro>();
        Debug.Assert(Text != null);
        _rectTransform = GetComponent<RectTransform>();
        Debug.Assert(_rectTransform != null);
        _originalScale = _rectTransform.localScale;
    }

    public void ShowPopup(int damage)
    {
        _rectTransform.localScale = _originalScale;
        Text.text = damage.ToString();
        Text.color = Color.white;
        _rectTransform
            .DOScale(_originalScale.x + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void ShowBackAttackPopup(int damage)
    {
        Debug.Log("UITextPopup.ShowBackAttackPopup()!!");
        _rectTransform.localScale = _originalScale;
        Text.color = Color.red;
        Text.text = damage.ToString();
        _rectTransform
            .DOScale(_originalScale.x + 0.4f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void ShowPopup(string status)
    {
        _rectTransform.localScale = _originalScale;
        Text.color = Color.white;
        Text.text = status;
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
