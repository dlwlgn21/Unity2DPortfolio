using data;
using DG.Tweening;
using System;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIHealthBar : MonoBehaviour
{
    [SerializeField] protected Gradient _gradient;
    [SerializeField] protected BaseStat _stat;
    private Image _healthBarImg;
    private Image _damagedBarImg;
    private TextMeshProUGUI _currHpText;
    private TextMeshProUGUI _maxHpText;
    private float _fillSpeed = 0.5f;
    private Color _damagedColor;

    protected abstract void Init();
    private void Start()
    {
        Debug.Assert(_stat != null);
        _healthBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "HealthBar");
        Debug.Assert(_healthBarImg != null);

        _currHpText = _healthBarImg.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _maxHpText = _healthBarImg.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _currHpText.text = _stat.HP.ToString();
        ChangeMaxHpText();

        _damagedBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "DamagedBar");
        _damagedColor = _damagedBarImg.color;
        _damagedColor.a = 0f;
        _damagedBarImg.color = _damagedColor;
        Debug.Assert(_damagedBarImg != null);
        SetHealthBarRatio(1f);
        Init();
    }

    public void DecraseHP(int beforeDamageHp, int afterDamgeHp)
    {
        float ratio = (float)afterDamgeHp / _stat.MaxHP;
        _damagedBarImg.fillAmount = _healthBarImg.fillAmount;
        _damagedColor.a = 1f;
        _damagedBarImg.color = _damagedColor;
        _damagedBarImg.DOFade(0f, _fillSpeed).SetEase(Ease.Flash);
        if (ratio < 0.5f && _currHpText.color != Color.white)
        {
            _currHpText.DOColor(Color.white, _fillSpeed);
            _maxHpText.DOColor(Color.white, _fillSpeed);
        }
        _currHpText.DOCounter(beforeDamageHp, Math.Max(afterDamgeHp, 0), _fillSpeed);
        SetHealthBarRatio(ratio);
    }
    public void IncraseHP(float ratio)
    {
        SetHealthBarRatio(ratio);
    }

    public void ChangeMaxHpText()
    {
        _maxHpText.text = _stat.MaxHP.ToString();
    }
    private void SetHealthBarRatio(float ratio)
    {
        if (ratio <= 0f)
        {
            _currHpText.color = Color.black;
            _maxHpText.color = Color.black;
        }
        _healthBarImg.DOFillAmount(ratio, _fillSpeed);
        _healthBarImg.DOColor(_gradient.Evaluate(ratio), _fillSpeed);
    }

}
