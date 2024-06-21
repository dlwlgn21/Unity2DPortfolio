using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class PlayerSkillRoll : PlayerSkillController
{
    private const float ROLL_INIT_COOL_TIME_IN_SEC = 1f;
    public override void Init()
    {
        _eSkillType = EPlayerSkill.ROLL;
        _initCoolTime = ROLL_INIT_COOL_TIME_IN_SEC;
        SkillCoolTime = ROLL_INIT_COOL_TIME_IN_SEC;
        IsPossibleDoSkill = true;
        PlayerController.PlayerSkillKeyDownEventHandler += OnPlayerRollKeyDown;
        Debug.Assert(_uiCoolTimerImg != null);
    }

    private void OnDestroy()
    {
        PlayerController.PlayerSkillKeyDownEventHandler -= OnPlayerRollKeyDown;
    }

    private void OnPlayerRollKeyDown(EPlayerSkill eType)
    {
        if (eType == EPlayerSkill.ROLL)
        {
            if (IsPossibleDoSkill)
            {
                _pc.ChangeState(EPlayerState.ROLL);
                _uiCoolTimerImg.StartCoolTime(SkillCoolTime);
                IsPossibleDoSkill = false;
                StartCoroutine(AfterGivenCoolTimePossibleDoSkillCo(SkillCoolTime));
            }
        }
    }
}
