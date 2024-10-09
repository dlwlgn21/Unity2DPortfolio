using define;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public sealed class Skill_SpawnReaperObject : Skill_BaseObject
{
    protected override void Init()
    {
        _eSkillType = ESkillType.Spawn_Reaper;
        _animKey = Managers.Data.SkillInfoDict[(int)_eSkillType].animKey;
    }
}
