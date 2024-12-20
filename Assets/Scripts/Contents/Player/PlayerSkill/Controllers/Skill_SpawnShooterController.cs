using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;

public sealed class Skill_SpawnShooterController : Skill_BaseController
{
    private Skill_SpawnShooterObject _spawnShooter;
    public override void Init()
    {
        _eSkillType = EActiveSkillType.Spawn_Shooter;
        InitByEActiveSkillType();
        if (_spawnShooter == null)
        {
            _spawnShooter = Managers.Resources.Instantiate<Skill_SpawnShooterObject>(Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Spawn_Shooter][0].objectPrefabPath);
            DontDestroyOnLoad(_spawnShooter.gameObject);
            _spawnShooter.gameObject.name = Managers.PlayerSkill.GetSkillObjectName(EActiveSkillType.Spawn_Shooter);

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

    private void OnPlayerSpawnAnimValidTiming()
    {
        if (_isUsingSkill)
        {
            _spawnShooter.SpawnShooter(_pc.SpawnShooterPoint.position, _pc.ELookDir);
            _isUsingSkill = false;
        }
    }
}
