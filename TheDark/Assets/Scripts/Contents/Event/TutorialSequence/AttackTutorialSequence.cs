
public class AttackTutorialSequence : TutorialSequence
{
    public override void OnDialogEnd()
    {
        base.Init();
        _tutorialManager.ActiveAttackKeyTutorial();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (!_isTutorialStart)
        {
            return;
        }
        if (_spawnendMonsterController.Stat.HP <= 0)
        {
            _tutorialManager.UnactiveAttackKeyTutorial();
            _tutorialManager.ActiveSuccessText(ETutorialTraning.ATTACK_TRAINING);
            SwitchCamToMain();
            gameObject.SetActive(false);
        }
    }
}
