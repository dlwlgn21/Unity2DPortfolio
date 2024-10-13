
public class AttackTutorialSequence : TutorialSequence
{
    public override void OnDialogEnd()
    {
        base.Init();
        SpawnMonster();
        _tutorialManager.ActiveAttackTutorialUIObjects();
        _isTutorialStart = true;
    }
    private void Update()
    {
        if (_isTutorialStart && _spawnendMonsterController.Stat.HP <= 0)
        {
            _tutorialManager.UnactiveAttackTutorialUIObjects();
            _tutorialManager.ActiveSuccessText(ETutorialTraning.AttackTraning);
            SwitchCamToMain();
            gameObject.SetActive(false);
        }
    }
}
