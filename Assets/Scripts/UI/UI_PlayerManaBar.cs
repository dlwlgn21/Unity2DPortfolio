using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlayerManaBar : UI_PlayerStatBar
{
    protected override void Init()
    {
        PlayerStat.OnManaChangedEventHandler -= OnChangedMana;
        PlayerStat.OnManaChangedEventHandler += OnChangedMana;

    }

    void OnChangedMana(int currMana, int maxMana)
    {
        SetBarRatio((float)currMana / maxMana);
    }
}
