using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
public sealed class Skill_SpawnRepaerController : Skill_BaseController
{
    private Skill_SpawnReaperObject _spawnReaper;
    public override void Init()
    {
        _eSkillType = EActiveSkillType.Spawn_Reaper;
        InitByEActiveSkillType();
        if (_spawnReaper == null)
        {
            _spawnReaper = Managers.Resources.Instantiate<Skill_SpawnReaperObject>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Spawn_Reaper][0].objectPrefabPath);
            DontDestroyOnLoad(_spawnReaper.gameObject);
            _spawnReaper.gameObject.name = Managers.PlayerSkill.GetSkillObjectName(EActiveSkillType.Spawn_Reaper);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnAnimValidTiming;
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnAnimValidTiming;
    }
    public override bool TryUseSkill()
    {
        if (IsValidStateAndManaToUseSkill())
        {
            _pc.ChangeState(EPlayerState.SkillSpawn);
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
}
