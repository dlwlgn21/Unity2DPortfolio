using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWSMonsterHpBar : MonoBehaviour
{
    [SerializeField]
    protected Gradient _gradient;

    protected BaseStat _stat;

    private Transform _parentTransform;
    private float _yMargin;


    private Image _healthBarImg;
    private Image _damagedBarImg;
    private float _fillSpeed = 0.5f;
    private Color _damagedColor;


    private void Start()
    {
        _parentTransform = transform.parent;
        _stat = transform.parent.GetComponent<MonsterStat>();
        _yMargin = transform.parent.GetComponent<Collider2D>().bounds.size.y + 0.15f;

        _healthBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "HealthBar");
        Debug.Assert(_healthBarImg != null);
        _damagedBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "DamagedBar");
        _damagedColor = _damagedBarImg.color;
        _damagedColor.a = 0f;
        _damagedBarImg.color = _damagedColor;
        Debug.Assert(_damagedBarImg != null);
        SetHealthBarRatio(1f);
    }
    private void Update()
    {
        transform.position = _parentTransform.position + Vector3.up * _yMargin;
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
