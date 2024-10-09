using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_PlayerExpBar : MonoBehaviour
{
    static public UnityAction OnExpBarFillTWEndEventHandler;
    [SerializeField] protected Gradient _gradient;
    private Image _expBarImg;
    private const float FILL_SPEED = 0.2f;

    private void Awake()
    {
        _expBarImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "ExpBar");
        Debug.Assert(_expBarImg != null);
        _expBarImg.fillAmount = 0f;
        PlayerStat.OnAddExpEventHandler -= OnAddExp;
        PlayerStat.OnAddExpEventHandler += OnAddExp;
    }

    void OnAddExp(int exp, int needLevelUpExp)
    {
        Debug.Assert(exp <= needLevelUpExp);
        SetExpBarRatio((float)exp / needLevelUpExp);
    }

    void SetExpBarRatio(float ratio)
    {
        _expBarImg.DOFillAmount(ratio, FILL_SPEED).OnComplete(OnFillTWEnded);
        _expBarImg.DOColor(_gradient.Evaluate(ratio), FILL_SPEED);
    }

    void OnFillTWEnded()
    {
        if (OnExpBarFillTWEndEventHandler != null)
            OnExpBarFillTWEndEventHandler.Invoke();
    }
}
