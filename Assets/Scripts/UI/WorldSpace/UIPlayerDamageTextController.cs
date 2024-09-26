using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerDamageTextController : UITextPopup
{
    protected override void Init()
    {
        _ePopupType = EPopupType.DAMAGE;
        PlayerController.HitUIEventHandler += OnPlayerHittedByMonsterNormalAttack;
    }

    private void OnDestroy()
    {
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
    }

    public void OnPlayerHittedByMonsterNormalAttack(int actualDamage, int beforeDamageHp, int afterDamageHp)
    {
        ShowPopup(actualDamage);
    }
}
