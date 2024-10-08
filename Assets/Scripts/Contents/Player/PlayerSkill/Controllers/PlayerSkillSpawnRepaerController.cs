using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
public class PlayerSkillSpawnRepaerController : BasePlayerSkillController
{
    private PlayerSkillSpawnReaperObject _spawnReaper;
    public override void Init()
    {
        InitByESkillType(ESkillType.Spawn_Reaper);
        if (_spawnReaper == null)
        {
            _spawnReaper = Managers.Resources.Instantiate<PlayerSkillSpawnReaperObject>("Prefabs/Player/Skills/SkillSpawnReaperObject");
            DontDestroyOnLoad(_spawnReaper.gameObject);
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
             _spawnReaper.SpawnReaper(_pc.SpawnReaperPoint.position, _pc.ELookDir);
            _isUsingSkill = false;
        }

    }
}
