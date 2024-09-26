using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class PlayerSkillRollController : BasePlayerSkillController
{
    private const float ROLL_INIT_COOL_TIME_IN_SEC = 1f;
    public override void Init()
    {
        _eSkillType = EPlayerSkill.ROLL;
        _initCoolTime = ROLL_INIT_COOL_TIME_IN_SEC;
        SkillCoolTimeInSec = ROLL_INIT_COOL_TIME_IN_SEC;
        IsPossibleDoSkill = true;
        Debug.Assert(_uiCoolTimerImg != null);
    }
    private void Update()
    {
        if (Input.GetKeyDown(PlayerController.KeyRoll))
        {
            if (IsPosibbleValidStateToDoSkill())
            {
                DoSkill(EPlayerSkill.ROLL);
            }
        }
    }
}
