using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSpawnShooterController : BasePlayerSkillController
{
    private const float SPAWN_SHOOTER_COOL_TIME_IN_SEC = 3f;
    private PlayerSkillSpawnShooterObject _spawnShooter;
    public override void Init()
    {
        _eSkillType = EPlayerSkill.SPAWN_REAPER;
        _initCoolTime = SPAWN_SHOOTER_COOL_TIME_IN_SEC;
        SkillCoolTime = SPAWN_SHOOTER_COOL_TIME_IN_SEC;
        IsPossibleDoSkill = true;
        Debug.Assert(_uiCoolTimerImg != null);
        if (_spawnShooter == null)
        {
            GameObject spawnShooter = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/SkillSpawnShooter");
            spawnShooter = Instantiate(spawnShooter);
            _spawnShooter = spawnShooter.GetComponent<PlayerSkillSpawnShooterObject>();
            DontDestroyOnLoad(spawnShooter);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnShooterAnimValidTiming;
        PlayerController.PlayerSkillKeyDownEventHandler += OnPlayerSpawnShooterKeyDown;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnShooterAnimValidTiming;
        PlayerController.PlayerSkillKeyDownEventHandler -= OnPlayerSpawnShooterKeyDown;
    }

    private void OnPlayerSpawnShooterKeyDown(EPlayerSkill eType)
    {
        if (eType == EPlayerSkill.SPAWN_SHOOTER)
        {
            if (IsPossibleDoSkill)
            {
                _pc.ChangeState(EPlayerState.CAST_LAUNCH);
                _uiCoolTimerImg.StartCoolTime(SkillCoolTime);
                IsPossibleDoSkill = false;
                StartCoroutine(AfterGivenCoolTimePossibleDoSkillCo(SkillCoolTime));
            }
        }
    }

    private void OnPlayerSpawnShooterAnimValidTiming(EPlayerSkill eType)
    {
        if (eType == EPlayerSkill.SPAWN_SHOOTER)
        {
            _spawnShooter.SpawnShooter(_pc.SpawnShooterPoint.position, _pc.ELookDir);
        }
    }
}
