using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using define;
using data;

public sealed class Skill_RollController : Skill_BaseController
{
    public override void Init()
    {
        _eSkillType = EActiveSkillType.Roll;
        InitByEActiveSkillType();
    }
    protected override void InitByEActiveSkillType()
    {
        base.InitByEActiveSkillType();
        CurrSkillLevel += 1;
        _skillInfo = Managers.Data.ActiveSkillInfoDict[_eSkillType][CurrSkillLevel - 1];
        _initCoolTimeInSec = Managers.Data.ActiveSkillInfoDict[_eSkillType][CurrSkillLevel - 1].coolTime;
        SkillCoolTimeInSec = _initCoolTimeInSec;
    }
    public override bool TryUseSkill()
    {
        if (IsValidStateAndManaToUseSkill())
        {
            _pc.ChangeState(EPlayerState.Roll);
            StartCountdownCoolTime();
            return true;
        }
        return false;
    }
}
