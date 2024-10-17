using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Skill_SwordStrikeObject : Skill_BaseObject
{
    protected override void Init()
    {
        ESkillType = ESkillType.Cast_SwordStrike_LV1;
        _animKey = Managers.Data.SkillInfoDict[(int)ESkillType].animKey;
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.8f;
    }
    protected override void OnSoundPlayTiming()
    {
        Managers.Sound.Play(DataManager.SFX_SKILL_SWORD_STRIKE_PATH);
    }
}
