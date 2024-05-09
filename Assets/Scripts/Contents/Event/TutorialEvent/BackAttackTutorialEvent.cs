using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAttackTutorialEvent : TutorialEvent
{
    private int _playerBackAttackCount = 0;
    private const int BACK_ATTACK_TRANING_PASS_COUNT = 3;
    private bool _isMonsterChangeStateToHitted = false;
    public override void OnDialogEnd()
    {
        base.Init();
        Managers.UIKeyTutorial.ActiveBackAttackTutorial();
        SetPlayerAttackToZero();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (_isTutorialStart)
        {
            if (!_isMonsterChangeStateToHitted && _mc.ECurrentState == EMonsterState.HITTED && _pc.ELookDir == _mc.ELookDir)
            {
                ++_playerBackAttackCount;
                Managers.UIKeyTutorial.IncreaseCountText(ETutorialCountText.BACK_ATTACK, _playerBackAttackCount);
                _isMonsterChangeStateToHitted = true;
            }
            if (_mc.ECurrentState != EMonsterState.HITTED)
            {
                _isMonsterChangeStateToHitted = false;
            }

            if (_playerBackAttackCount >= BACK_ATTACK_TRANING_PASS_COUNT)
            {
                RollBackPlayerAttack();
                _mc.ChangeState(EMonsterState.DIE);
                Managers.UIKeyTutorial.UnactiveBackAttackTutorial();
                Managers.UIKeyTutorial.ActiveSuccessText(ETutorialTraning.BACK_ATTACK_TRAINING);
                gameObject.SetActive(false);
                return;
            }
        }
    }
}