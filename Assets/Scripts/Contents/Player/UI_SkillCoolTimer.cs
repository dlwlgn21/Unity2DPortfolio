using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_SkillCoolTimer : MonoBehaviour
{
    Image _coolTimeImg;
    Color _showColor;
    Color _hideColor;
    TextMeshProUGUI _coolTimeText;
    private void Awake()
    {
        _coolTimeImg = GetComponent<Image>();
        _showColor = _coolTimeImg.color;
        _hideColor = new Color(_showColor.r, _showColor.g, _showColor.b, 0f);
        _coolTimeImg.color = _hideColor;
        _coolTimeText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "CoolTimeText");
    }

    public void InitForNextSceneLoad()
    {
        OnFillAmountTWEnd();
    }

    public void StartCoolTime(float coolTime)
    {
        _coolTimeText.DOCounter((int)coolTime, 1, (int)coolTime).SetEase(Ease.Linear).SetLink(gameObject);
        _coolTimeImg.color = _showColor;
        _coolTimeImg.DOFillAmount(0f, coolTime).SetEase(Ease.Linear).OnComplete(OnFillAmountTWEnd).SetLink(gameObject);
    }

    private void OnFillAmountTWEnd()
    {
        _coolTimeText.text = "";
        _coolTimeImg.color = _hideColor;
        _coolTimeImg.fillAmount = 1f;
    }

}
