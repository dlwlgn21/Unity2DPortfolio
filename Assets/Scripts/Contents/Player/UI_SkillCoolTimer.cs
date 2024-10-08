using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_SkillCoolTimer : MonoBehaviour
{
    private Image _coolTimeImg;
    private Color _showColor;
    private Color _hideColor;
    private TextMeshProUGUI _coolTimeText;
    private float _coolTimer;
    private bool _isStartCoolTime = false;
    private void Start()
    {
        _coolTimeImg = GetComponent<Image>();
        _showColor = _coolTimeImg.color;
        _hideColor = new Color(_showColor.r, _showColor.g, _showColor.b, 0f);
        _coolTimeImg.color = _hideColor;
        _coolTimeText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "CoolTimeText");
    }


    private void Update()
    {
        if (_isStartCoolTime)
        {
            _coolTimer -= Time.deltaTime;
            _coolTimeText.text = (((int)_coolTimer) + 1).ToString();
        }
    }

    public void StartCoolTime(float coolTime)
    {
        _coolTimer = coolTime;
        _isStartCoolTime = true;

        _coolTimeImg.color = _showColor;
        _coolTimeImg.DOFillAmount(0f, coolTime).OnComplete(OnFillAmountTWEnd);
    }

    private void OnFillAmountTWEnd()
    {
        _coolTimer = 0f;
        _isStartCoolTime = false;

        _coolTimeText.text = "";
        _coolTimeImg.color = _hideColor;
        _coolTimeImg.fillAmount = 1f;
    }

}
