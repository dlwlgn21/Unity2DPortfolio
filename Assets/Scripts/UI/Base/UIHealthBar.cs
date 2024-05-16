using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    protected Gradient _gradient;
    private Image _healthBarImg;
    private Image _damagedBarImg;
    private float _fillSpeed = 0.5f;
    private Color _damagedColor;

    protected abstract void Init();
    private void Start()
    {
        _healthBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "HealthBar");
        Debug.Assert(_healthBarImg != null);
        _damagedBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "DamagedBar");
        _damagedColor = _damagedBarImg.color;
        _damagedColor.a = 0f;
        _damagedBarImg.color = _damagedColor;
        Debug.Assert(_damagedBarImg != null);
        SetHealthBarRatio(1f);
        Init();
    }

    public void DecraseHP(float ratio)
    {
        _damagedBarImg.fillAmount = _healthBarImg.fillAmount;
        _damagedColor.a = 1f;
        _damagedBarImg.color = _damagedColor;
        _damagedBarImg.DOFade(0f, _fillSpeed).SetEase(Ease.Flash);
        SetHealthBarRatio(ratio);
    }
    public void IncraseHP(float ratio)
    {
        SetHealthBarRatio(ratio);
    }
    private void SetHealthBarRatio(float ratio)
    {
        _healthBarImg.DOFillAmount(ratio, _fillSpeed);
        _healthBarImg.DOColor(_gradient.Evaluate(ratio), _fillSpeed);
    }

}
