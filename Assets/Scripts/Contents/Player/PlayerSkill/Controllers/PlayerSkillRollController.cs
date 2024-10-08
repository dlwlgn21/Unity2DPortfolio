using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using define;
public class PlayerSkillRollController : BasePlayerSkillController
{
    public override void Init()
    {
        InitByESkillType(ESkillType.Roll);
    }

    public override bool TryUseSkill()
    {
        if (IsValidStateToUseSkill())
        {
            _pc.ChangeState(EPlayerState.ROLL);
            ProcessSkillLogic();
            return true;
        }
        return false;
    }
}
