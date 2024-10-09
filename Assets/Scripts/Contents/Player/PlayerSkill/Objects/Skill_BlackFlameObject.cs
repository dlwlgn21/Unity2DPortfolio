using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Skill_BlackFlameObject : Skill_BaseObject
{
    protected override void Init()
    {
        _eSkillType = ESkillType.Cast_BlackFlame;
        _animKey = Managers.Data.SkillInfoDict[(int)_eSkillType].animKey;
    }
}
