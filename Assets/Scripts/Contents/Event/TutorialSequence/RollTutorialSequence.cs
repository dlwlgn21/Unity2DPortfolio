using UnityEngine;

public class RollTutorialSequence : TutorialSequence
{
    private int _playerRollCount = 0;
    private const int ROLL_TRANING_PASS_COUNT = 5;
    private bool _isPlayerChangeStateToRoll = false;
    public override void OnDialogEnd()
    {
        base.Init();
        SpawnMonster();
        _tutorialManager.ActiveRollTutorialUIObjects();
        SetPlayerAttackToZero();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (_isTutorialStart)
        {
            if (_pc.ECurrentState == EPlayerState.Roll && _isPlayerChangeStateToRoll == false)
            {
                ++_playerRollCount;
                _tutorialManager.IncreaseCountText(ETutorialCountText.Roll, _playerRollCount);
                _isPlayerChangeStateToRoll = true;
            }
            if (_pc.ECurrentState != EPlayerState.Roll)
            {
                _isPlayerChangeStateToRoll = false;
            }

            if (_playerRollCount >= ROLL_TRANING_PASS_COUNT)
            {
                RollBackPlayerAttack();
                _spawnendMonsterController.ChangeState(ENormalMonsterState.Die);
                _tutorialManager.UnactiveRollTutorialUIObjects();
                _tutorialManager.ActiveSuccessText(ETutorialTraning.RollTraning);
                SwitchCamToMain();
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
