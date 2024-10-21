using define;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public sealed class Skill_SpawnReaperObject : Skill_BaseObject
{
    protected override void Init()
    {
        ESkillType = EActiveSkillType.Spawn_Reaper;
        _animKey = Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Spawn_Reaper][0].animKey;
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.7f;
    }

    protected override void OnSoundPlayTiming()
    {
        Managers.Sound.Play(DataManager.SFX_SKILL_SPAWN_REAPER_PATH);
    }
}