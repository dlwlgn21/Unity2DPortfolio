using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkillTutorialSequence : TutorialSequence
{
    bool _isSuccessUseSkill = false;
    public override void OnDialogEnd()
    {
        base.Init();
        PlayerStat stat = _pc.gameObject.GetComponent <PlayerStat>();
        _tutorialManager.ActiveSkillTutorialUIObjects();
        stat.Exp += Managers.Data.PlayerStatDict[1].totalExp;
    }

    private void Update()
    {
        if (_isSuccessUseSkill)
            return;
        if (_pc.ECurrentState == EPlayerState.SkillSpawn || _pc.ECurrentState == EPlayerState.SkillCast)
        {
            _isSuccessUseSkill = true;
            _tutorialManager.ActiveSuccessText(ETutorialTraning.SkillTraning);
            _tutorialManager.UnactiveSkillTutorialUIObjects();
            StartCoroutine(LoadSceneAfterSeconds(3f));
        }
    }


    IEnumerator LoadSceneAfterSeconds(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        Managers.Scene.LoadScene(define.ESceneType.AbandonLoadScene);
    }

}
