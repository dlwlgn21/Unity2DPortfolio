using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public sealed class Skill_BlackFlameObject : Skill_BaseObject
{
    protected override void Init()
    {
        ESkillType = ESkillType.Cast_BlackFlame_LV1;
        _animKey = Managers.Data.SkillInfoDict[(int)ESkillType].animKey;
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.3f;
    }
}