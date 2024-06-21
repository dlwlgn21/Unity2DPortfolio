using DG.Tweening;
using TMPro;
using UnityEngine;

public enum EPopupType
{ 
    DAMAGE,
    STATUS
}

public abstract class UITextPopup : MonoBehaviour
{
    [SerializeField] protected EPopupType _ePopupType;
    public TextMeshProUGUI Text { get; private set; }
    private Transform _parentTransform;
    private RectTransform _rectTransform;
    private Vector3 _originalScale;
    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
        Debug.Assert(Text != null);
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
        Text.text = damage.ToString();
        Text.color = Color.white;
        _rectTransform
            .DOScale(_originalScale.z + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    //public void ShowBackAttackPopup(int damage)
    //{
    //    Debug.Log("UITextPopup.ShowBackAttackPopup()!!");
    //    _rectTransform.localScale = _originalScale;
    //    Text.color = Color.red;
    //    Text.text = damage.ToString();
    //    _rectTransform
    //        .DOScale(_originalScale.z + 0.4f, 0.5f)
    //        .SetEase(Ease.OutElastic)
    //        .OnComplete(OnScaleTweenEnd);
    //}


    public void ShowPopup(string status)
    {
        _rectTransform.localScale = _originalScale;
        Text.color = Color.white;
        Text.text = status;
        _rectTransform
            .DOScale(_originalScale.z + 0.2f, 0.5f)
            .SetEase(Ease.OutElastic)
            .OnComplete(OnScaleTweenEnd);
    }
    public void OnScaleTweenEnd()
    {
        _rectTransform.localScale = Vector3.zero;
    }
}
