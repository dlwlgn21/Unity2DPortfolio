using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
public sealed class Skill_SpawnRepaerController : Skill_BaseController
{
    private Skill_SpawnReaperObject _spawnReaper;
    public override void Init()
    {
        InitByESkillType(ESkillType.Spawn_Reaper_LV1);
        if (_spawnReaper == null)
        {
            _spawnReaper = Managers.Resources.Instantiate<Skill_SpawnReaperObject>(Managers.Data.SkillInfoDict[(int)_eSkillType].objectPrefabPath);
            DontDestroyOnLoad(_spawnReaper.gameObject);
            _spawnReaper.gameObject.name = Managers.PlayerSkill.GetSkillObjectName(ESkillType.Spawn_Reaper_LV1);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnAnimValidTiming;
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnAnimValidTiming;
    }
    public override bool TryUseSkill()
    {
        if (IsValidStateToUseSkill())
        {
            _pc.ChangeState(EPlayerState.SKILL_SPAWN);
            StartCountdownCoolTime();
            _isUsingSkill = true;
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnAnimValidTiming;
    }
    void OnPlayerSpawnAnimValidTiming()
    {
        if (_isUsingSkill)
        {
             _spawnReaper.UseSkill(_pc.SpawnReaperPoint.position, _pc.ELookDir);
            _isUsingSkill = false;
        }
    }

    public override void LevelUpSkill(ESkillType eType)
    {
        base.LevelUpSkill(eType);
        _spawnReaper.ESkillType = eType;
    }
}
