using define;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public sealed class Skill_SpawnReaperObject : Skill_BaseObject
{
    protected override void Init()
    {
        ESkillType = ESkillType.Spawn_Reaper_LV1;
        _animKey = Managers.Data.SkillInfoDict[(int)ESkillType].animKey;
    }
}