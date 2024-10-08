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
            _spawnReaper = Managers.Resources.Instantiate<PlayerSkillSpawnReaperObject>("Prefabs/Player/Skills/SkillSpawnReaper");
            DontDestroyOnLoad(_spawnReaper.gameObject);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnReaperAnimValidTiming;
    }
    public override bool TryUseSkill()
    {
        if (IsValidStateToUseSkill())
        {
            _pc.ChangeState(EPlayerState.CAST_SPAWN);
            ProcessSkillLogic();
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnReaperAnimValidTiming;
    }
    void OnPlayerSpawnReaperAnimValidTiming(ESkillType eType)
    {
        if (eType == ESkillType.Spawn_Reaper)
        {
            _spawnReaper.SpawnReaper(_pc.SpawnReaperPoint.position, _pc.ELookDir);
        }
    }
}
