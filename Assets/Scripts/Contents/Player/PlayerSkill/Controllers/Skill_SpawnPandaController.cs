using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;

public sealed class Skill_SpawnShooterController : Skill_BaseController
{
    private Skill_SpawnShooterObject _spawnShooter;
    public override void Init()
    {
        InitByESkillType(ESkillType.Spawn_Shooter_LV1);
        if (_spawnShooter == null)
        {
            _spawnShooter = Managers.Resources.Instantiate<Skill_SpawnShooterObject>(Managers.Data.SkillInfoDict[(int)_eSkillType].objectPrefabPath);
            DontDestroyOnLoad(_spawnShooter.gameObject);
            _spawnShooter.gameObject.name = Managers.PlayerSkill.GetSkillObjectName(ESkillType.Spawn_Shooter_LV1);

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

    public override void LevelUpSkill(ESkillType eType)
    {
        base.LevelUpSkill(eType);
        _spawnShooter.ESkillType = eType;
    }
}
