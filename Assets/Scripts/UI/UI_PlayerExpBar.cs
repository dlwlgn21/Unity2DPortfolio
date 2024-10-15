using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_PlayerExpBar : UI_PlayerStatBar
{
    PlayerStat _stat;
    Coroutine _currCoOrNull;
    protected override void Init()
    {
        PlayerStat.OnAddExpEventHandler -= OnAddExp;
        PlayerStat.OnAddExpEventHandler += OnAddExp;
        _barImg.fillAmount = 0;
    }
    void OnAddExp(int exp, int needLevelUpExp)
    {
        Debug.Assert(exp <= needLevelUpExp);
        SetBarRatio((float)exp / needLevelUpExp);
        if (exp == needLevelUpExp && _currCoOrNull == null)
            _currCoOrNull = StartCoroutine(SetBarRatioAfterSeconds(3f));
    }

    IEnumerator SetBarRatioAfterSeconds(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        if (_stat == null)
            _stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        SetBarRatio((float)_stat.Exp / Managers.Data.PlayerStatDict[_stat.Level].totalExp);
        _currCoOrNull = null;
    }
}
