using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class UI_WSPlayerPopupDamageController : UI_WSPlayerPopupTextController
{
    protected override void Init()
    {
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
        PlayerController.HitUIEventHandler += OnPlayerHittedByMonsterNormalAttack;
    }
    void OnPlayerHittedByMonsterNormalAttack(int actualDamage, int beforeDamageHp, int afterDamageHp)
    {
        _text.text = actualDamage.ToString();
        StartTW();
    }
}
