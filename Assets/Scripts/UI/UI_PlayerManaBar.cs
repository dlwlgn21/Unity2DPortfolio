using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class UI_PlayerManaBar : UI_PlayerStatBar
{

    TextMeshProUGUI _currManaText;
    TextMeshProUGUI _maxManaText;
    PlayerStat _stat;
    const float COUNTER_FILL_SPEED = 1f;
    protected override void Init()
    {
        PlayerStat.ManaChangedEventHandler -= OnChangedMana;
        PlayerStat.ManaChangedEventHandler += OnChangedMana;
        _currManaText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "CurrManaText");
        _maxManaText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "MaxManaText");
        _stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        _currManaText.text = _stat.Mana.ToString();
        _maxManaText.text = _stat.MaxMana.ToString();
    }

    void OnChangedMana(int beforeMana, int afterMana)
    {
        SetBarRatio((float)afterMana / _stat.MaxMana);
        _currManaText.DOCounter(beforeMana, afterMana, COUNTER_FILL_SPEED).SetLink(gameObject);
    }
}
