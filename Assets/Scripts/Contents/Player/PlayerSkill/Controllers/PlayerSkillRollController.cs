using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using define;
public class PlayerSkillRollController : BasePlayerSkillController
{
    private const float ROLL_INIT_COOL_TIME_IN_SEC = 1f;
    public override void Init()
    {
        _eSkillType = ESkillType.Roll;
        _initCoolTime = ROLL_INIT_COOL_TIME_IN_SEC;
        SkillCoolTimeInSec = ROLL_INIT_COOL_TIME_IN_SEC;
        IsCanUseSkill = true;
        Debug.Assert(_uiCoolTimerImg != null);
    }
    private void Update()
    {
        if (Input.GetKeyDown(PlayerController.KeyRoll))
        {
            if (IsValidStateToUseSkill())
            {
                UseSkill(ESkillType.Roll);
            }
        }
    }
}
