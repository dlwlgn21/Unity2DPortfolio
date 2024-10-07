using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;

public class PlayerSkillSpawnPandaController : BasePlayerSkillController
{
    private const float SPAWN_SHOOTER_COOL_TIME_IN_SEC = 3f;
    private PlayerSkillSpawnPandaObject _spawnPanda;
    public override void Init()
    {
        _eSkillType = ESkillType.Spawn_Panda;
        _initCoolTime = SPAWN_SHOOTER_COOL_TIME_IN_SEC;
        SkillCoolTimeInSec = SPAWN_SHOOTER_COOL_TIME_IN_SEC;
        IsCanUseSkill = true;
        Debug.Assert(_uiCoolTimerImg != null);
        if (_spawnPanda == null)
        {
            GameObject spawnPanda = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/SkillSpawnPanda");
            spawnPanda = Instantiate(spawnPanda);
            _spawnPanda = spawnPanda.GetComponent<PlayerSkillSpawnPandaObject>();
            DontDestroyOnLoad(spawnPanda);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnShooterAnimValidTiming;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnShooterAnimValidTiming;
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerController.KeyLaunchBomb))
        {
            if (IsValidStateToUseSkill())
            {
                UseSkill(ESkillType.Spawn_Panda);
            }
        }
    }
    private void OnPlayerSpawnShooterAnimValidTiming(ESkillType eType)
    {
        if (eType == ESkillType.Spawn_Panda)
        {
            _spawnPanda.SpawnShooter(_pc.SpawnShooterPoint.position, _pc.ELookDir);
        }
    }
}
