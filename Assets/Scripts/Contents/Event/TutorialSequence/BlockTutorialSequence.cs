using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTutorialSequence : TutorialSequence
{
    private int _playerBlockCount = 0;
    private const int BLOCK_TRANING_PASS_COUNT = 3;
    private bool _isPlayerChangeStateToBlockSuccess = false;

    public override void OnDialogEnd()
    {
        base.Init();
        _tutorialManager.ActiveBlockKeyTutorial();
        SetPlayerAttackToZero();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (_isTutorialStart)
        {
            if (_pc.ECurrentState == EPlayerState.BLOCK_SUCESS && _isPlayerChangeStateToBlockSuccess == false)
            {
                ++_playerBlockCount;
                _tutorialManager.IncreaseCountText(ETutorialCountText.BLOCK, _playerBlockCount);
                _isPlayerChangeStateToBlockSuccess = true;
            }
            if (_pc.ECurrentState != EPlayerState.BLOCK_SUCESS)
            {
                _isPlayerChangeStateToBlockSuccess = false;
            }

            if (_playerBlockCount >= BLOCK_TRANING_PASS_COUNT)
            {
                RollBackPlayerAttack();
                _spawnendMonsterController.ChangeState(ENormalMonsterState.DIE);
                _tutorialManager.UnactiveBlockKeyTutorial();
                _tutorialManager.ActiveSuccessText(ETutorialTraning.BLOCK_TRAINING);
                SwitchCamToMain();
                gameObject.SetActive(false);
                return;
            }
        }
    }


}