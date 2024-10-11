using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using define;
public sealed class Skill_RollController : Skill_BaseController
{
    public override void Init()
    {
        InitByESkillType(ESkillType.Roll);
    }

    public override bool TryUseSkill()
    {
        if (IsValidStateAndManaToUseSkill())
        {
            _pc.ChangeState(EPlayerState.ROLL);
            StartCountdownCoolTime();
            return true;
        }
        return false;
    }
}
