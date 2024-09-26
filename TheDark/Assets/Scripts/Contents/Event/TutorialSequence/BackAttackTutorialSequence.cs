using UnityEngine;

public class BackAttackTutorialSequence : TutorialSequence
{
    private int _playerBackAttackCount = 0;
    private const int BACK_ATTACK_TRANING_PASS_COUNT = 3;

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
            if (!_isSucessBackAttack && _pc.ELookDir == _spawnendMonsterController.ELookDir && _pc.ECurrentState == EPlayerState.NORMAL_ATTACK_1)
            {
                // TODO : 이곳 고쳐야 함. 여러가지 방법이 있을 것.
                //if (_pc.StatusText.Text.text == BACK_ATTACK_STATUS_STRING)
                //{
                //    ++_playerBackAttackCount;
                //    _tutorialManager.IncreaseCountText(ETutorialCountText.BACK_ATTACK, _playerBackAttackCount);
                //    _isSucessBackAttack = true;
                //}
                ++_playerBackAttackCount;
                _tutorialManager.IncreaseCountText(ETutorialCountText.BACK_ATTACK, _playerBackAttackCount);
                _isSucessBackAttack = true;
            }


            if (_playerBackAttackCount >= BACK_ATTACK_TRANING_PASS_COUNT)
            {
                RollBackPlayerAttack();
                _spawnendMonsterController.ChangeState(ENormalMonsterState.DIE);
                _tutorialManager.UnactiveBackAttackTutorial();
                _tutorialManager.ActiveSuccessText(ETutorialTraning.BACK_ATTACK_TRAINING);
                SwitchCamToMain();
                gameObject.SetActive(false);
                return;
            }
        }
    }
}