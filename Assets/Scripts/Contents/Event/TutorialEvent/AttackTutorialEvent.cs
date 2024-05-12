using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckTutorialEvent : TutorialEvent
{
    public override void OnDialogEnd()
    {
        base.Init();
        Managers.UIKeyTutorial.ActiveAttackKeyTutorial();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (!_isTutorialStart)
            return;
        if (_mc.Stat.HP <= 0)
        {
            Managers.UIKeyTutorial.UnactiveAttackKeyTutorial();
            Managers.UIKeyTutorial.ActiveSuccessText(ETutorialTraning.ATTACK_TRAINING);
            SwitchCamToMain();
            gameObject.SetActive(false);
        }
    }
}
