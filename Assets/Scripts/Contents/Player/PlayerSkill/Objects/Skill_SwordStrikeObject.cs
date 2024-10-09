using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Skill_SwordStrikeObject : Skill_BaseObject
{
    protected override void Init()
    {
        _eSkillType = ESkillType.Cast_SwordStrike;
        _animKey = Managers.Data.SkillInfoDict[(int)_eSkillType].animKey;
    }
}
