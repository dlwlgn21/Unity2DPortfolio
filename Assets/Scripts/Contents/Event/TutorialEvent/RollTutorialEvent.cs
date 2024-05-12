using UnityEngine;

public class RollTutorialEvent : TutorialEvent
{
    private int _playerRollCount = 0;
    private const int ROLL_TRANING_PASS_COUNT = 10;
    private bool _isPlayerChangeStateToRoll = false;
    public override void OnDialogEnd()
    {
        base.Init();
        Managers.UIKeyTutorial.ActiveRollKeyTutorial();
        SetPlayerAttackToZero();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (_isTutorialStart)
        {
            if (_pc.ECurrentState == EPlayerState.ROLL && _isPlayerChangeStateToRoll == false)
            {
                ++_playerRollCount;
                Managers.UIKeyTutorial.IncreaseCountText(ETutorialCountText.ROLL, _playerRollCount);
                _isPlayerChangeStateToRoll = true;
            }
            if (_pc.ECurrentState != EPlayerState.ROLL)
            {
                _isPlayerChangeStateToRoll = false;
            }

            if (_playerRollCount >= ROLL_TRANING_PASS_COUNT)
            {
                RollBackPlayerAttack();
                _mc.ChangeState(EMonsterState.DIE);
                Managers.UIKeyTutorial.UnactiveRollKeyTutorial();
                Managers.UIKeyTutorial.ActiveSuccessText(ETutorialTraning.ROLL_TRAINING);
                SwitchCamToMain();
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
