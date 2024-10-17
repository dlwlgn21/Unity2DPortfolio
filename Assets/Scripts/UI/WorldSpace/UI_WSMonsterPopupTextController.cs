using DG.Tweening;
using TMPro;
using UnityEngine;

public enum EPopupType
{ 
    DAMAGE,
    STATUS
}
public abstract class UI_WSMonsterPopupTextController : MonoBehaviour
{
    [SerializeField] protected EPopupType _ePopupType;
    protected TextMeshProUGUI _text;
    protected Transform _parentTransform;
    protected RectTransform _rectTransform;
    protected Vector3 _originalScale;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        Debug.Assert(_text != null);
        _rectTransform = GetComponent<RectTransform>();
        Debug.Assert(_rectTransform != null);
        _originalScale = _rectTransform.localScale;
        _parentTransform = transform.parent;
        Init();
    }
    protected abstract void Init();
    private void Update()
    {
        Vector3 currScale = _rectTransform.localScale;
        if (_parentTransform.localRotation.eulerAngles.y > 0f)
        {
            _rectTransform.localScale = new Vector3(-currScale.x, currScale.y, currScale.z);
        }
    }

    public void ShowPopup(int damage)
    {
        _rectTransform.localScale = _originalScale;
        _text.text = damage.ToString();
        _text.color = Color.white;
        StartTW();
    }

    public void ShowPopup(string status)
    {
        _rectTransform.localScale = _originalScale;
        _text.color = Color.white;
        _text.text = status;
        StartTW();
    }

    void StartTW()
    {
        _rectTransform
            .DOScale(_originalScale.z + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    void OnScaleTweenEnd()
    {
        _rectTransform.localScale = Vector3.zero;
    }
}
