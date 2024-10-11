using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_PlayerExpBar : UI_PlayerStatBar
{
    static public UnityAction OnExpBarFillTWEndEventHandler;
    void OnAddExp(int exp, int needLevelUpExp)
    {
        Debug.Assert(exp <= needLevelUpExp);
        SetBarRatio((float)exp / needLevelUpExp, OnFillTWEnded);
    }
    protected override void Init()
    {
        PlayerStat.OnAddExpEventHandler -= OnAddExp;
        PlayerStat.OnAddExpEventHandler += OnAddExp;
        _barImg.fillAmount = 0;
    }

    void OnFillTWEnded()
    {
        // 이때 경험치 들어감.
        if (OnExpBarFillTWEndEventHandler != null)
            OnExpBarFillTWEndEventHandler.Invoke();
    }

}
