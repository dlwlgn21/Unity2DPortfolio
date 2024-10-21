using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class UI_WSPlayerPopupTextController : MonoBehaviour
{
    const float PUNCH_COEFF = 0.25f;
    const float PUNCH_TIME_IN_SEC = 0.75f;
    protected TextMeshProUGUI _text;
    protected RectTransform _rectTransform;
    protected Vector3 _originalScale;
    protected PlayerController _pc;
    protected abstract void Init();

    private void Awake()
    {
        AssignComponentsIfComponentsIsNull();
        _originalScale = _rectTransform.localScale;
    }
    private void Start()
    {
        AssignComponentsIfComponentsIsNull();
        _originalScale = _rectTransform.localScale;
        Init();
    }

    private void Update()
    {
        if (_pc.ELookDir == define.ECharacterLookDir.Left)
            transform.localScale = new Vector3(-_originalScale.x, _originalScale.y, _originalScale.z);
        else
            transform.localScale = _originalScale;
    }

    protected void StartTW()
    {
        AssignComponentsIfComponentsIsNull();
        _text.enabled = true;
        _rectTransform.localScale = _originalScale;
        Managers.Tween.StartDoPunchPos(_rectTransform, Vector3.up * PUNCH_COEFF, PUNCH_TIME_IN_SEC, OnPunchTweenEnd);
    }
    protected void OnPunchTweenEnd()
    {
        AssignComponentsIfComponentsIsNull();
        _text.enabled = false;
    }

    void AssignComponentsIfComponentsIsNull()
    {
        if (_text == null)
        {
            _text = GetComponent<TextMeshProUGUI>();
            _rectTransform = GetComponent<RectTransform>();
            _pc = transform.parent.gameObject.GetComponent<PlayerController>();
        }
    }
}
