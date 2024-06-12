using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAttackTutorialEvent : TutorialEvent
{
    private int _playerBackAttackCount = 0;
    private const int BACK_ATTACK_TRANING_PASS_COUNT = 3;
    private const string BACK_ATTACK_STATUS_STRING = "╧И╬Нец!";

    private bool _isSucessBackAttack = false;
    private const float RECOUNTING_TIME = 2f;
    private float _recountingTimer = RECOUNTING_TIME;
    public override void OnDialogEnd()
    {
        base.Init();
        _tutorialManager.ActiveBackAttackTutorial();
        SetPlayerAttackToZero();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (_isTutorialStart)
        {
            if (_isSucessBackAttack)
            {
                _recountingTimer -= Time.deltaTime;
                if (_recountingTimer < 0f)
                {
                    _recountingTimer = RECOUNTING_TIME;
                    _isSucessBackAttack = false;
                }
            }
            if (!_isSucessBackAttack && _pc.ELookDir == _mc.ELookDir && _pc.ECurrentState == EPlayerState.NORMAL_ATTACK_1)
            {
                if (_pc.StatusText.Text.text == BACK_ATTACK_STATUS_STRING)
                {
                    ++_playerBackAttackCount;
                    _tutorialManager.IncreaseCountText(ETutorialCountText.BACK_ATTACK, _playerBackAttackCount);
                    _isSucessBackAttack = true;
                }
            }


            if (_playerBackAttackCount >= BACK_ATTACK_TRANING_PASS_COUNT)
            {
                RollBackPlayerAttack();
                _mc.ChangeState(EMonsterState.DIE);
                _tutorialManager.UnactiveBackAttackTutorial();
                _tutorialManager.ActiveSuccessText(ETutorialTraning.BACK_ATTACK_TRAINING);
                SwitchCamToMain();
                gameObject.SetActive(false);
                return;
            }
        }
    }
}