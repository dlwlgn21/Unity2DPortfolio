using data;
using DG.Tweening;
using System;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_HealthBar : MonoBehaviour
{
    [SerializeField] protected Gradient _gradient;
    protected BaseStat _stat;
    protected Image _healthBarImg;
    protected Image _damagedBarImg;
    protected TextMeshProUGUI _currHpText;
    protected TextMeshProUGUI _maxHpText;
    protected float _fillSpeed = 0.5f;
    protected Color _damagedColor;

    public abstract void Init();
    private void Awake()
    {
        Init();
        _healthBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "HealthBar");
        Debug.Assert(_healthBarImg != null);

        _currHpText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(_healthBarImg.gameObject, "CurrHpText");
        _maxHpText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(_healthBarImg.gameObject, "MaxHpText");
        _currHpText.text = _stat.HP.ToString();
        _maxHpText.text = _stat.MaxHP.ToString();

        _damagedBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "DamagedBar");
        _damagedColor = _damagedBarImg.color;
        _damagedColor.a = 0f;
        _damagedBarImg.color = _damagedColor;
        Debug.Assert(_damagedBarImg != null);
    }

    public void DecraseHP(int beforeDamageHp, int afterDamgeHp)
    {
        float ratio = (float)afterDamgeHp / _stat.MaxHP;
        _damagedBarImg.fillAmount = _healthBarImg.fillAmount;
        _damagedColor.a = 1f;
        _damagedBarImg.color = _damagedColor;
        _damagedBarImg.DOFade(0f, _fillSpeed).SetEase(Ease.Flash);

        //if (ratio < 0.5f && _currHpText.color != Color.white)
        //{
        //    _currHpText.DOColor(Color.white, _fillSpeed);
        //    _maxHpText.DOColor(Color.white, _fillSpeed);
        //}

        DoCounterHp(beforeDamageHp, Math.Max(afterDamgeHp, 0));
        SetHealthBarRatio(ratio);
    }
    public void SetFullHpBarAndText()
    {
        IncraseHPRatio(1f);
        _maxHpText.text = _stat.MaxHP.ToString();
        _currHpText.text = _stat.MaxHP.ToString();
    }

    protected void IncraseHPRatio(float ratio)
    {
        SetHealthBarRatio(ratio);
    }

    protected void DoCounterHp(int fromValue, int endValue)
    {
        _currHpText.DOCounter(fromValue, endValue, _fillSpeed);
    }

    protected void SetHealthBarRatio(float ratio)
    {
        _healthBarImg.DOFillAmount(ratio, _fillSpeed);
        _healthBarImg.DOColor(_gradient.Evaluate(ratio), _fillSpeed);
    }

}
