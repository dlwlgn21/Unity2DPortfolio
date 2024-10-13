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
        SpawnMonster();
        _tutorialManager.ActiveBlockTutorialUIObjects();
        SetPlayerAttackToZero();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (_isTutorialStart)
        {
            if (_pc.ECurrentState == EPlayerState.BlockSucces && _isPlayerChangeStateToBlockSuccess == false)
            {
                ++_playerBlockCount;
                _tutorialManager.IncreaseCountText(ETutorialCountText.Block, _playerBlockCount);
                _isPlayerChangeStateToBlockSuccess = true;
            }
            if (_pc.ECurrentState != EPlayerState.BlockSucces)
            {
                _isPlayerChangeStateToBlockSuccess = false;
            }

            if (_playerBlockCount >= BLOCK_TRANING_PASS_COUNT)
            {
                RollBackPlayerAttack();
                _spawnendMonsterController.ChangeState(ENormalMonsterState.Die);
                _tutorialManager.UnactiveBlockTutorialUIObjects();
                _tutorialManager.ActiveSuccessText(ETutorialTraning.BlockTraning);
                SwitchCamToMain();
                gameObject.SetActive(false);
                return;
            }
        }
    }


}