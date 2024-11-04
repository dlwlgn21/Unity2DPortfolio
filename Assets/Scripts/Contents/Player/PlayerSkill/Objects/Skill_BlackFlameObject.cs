using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public sealed class Skill_BlackFlameObject : Skill_BaseObject
{
    protected override void Init()
    {
        ESkillType = EActiveSkillType.Cast_BlackFlame;
        _animKey = Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Cast_BlackFlame][0].animKey;
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.3f;
    }

    protected override void OnSoundPlayTiming()
    {
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_SKILL_BLACK_FLAME_PATH);
    }
}