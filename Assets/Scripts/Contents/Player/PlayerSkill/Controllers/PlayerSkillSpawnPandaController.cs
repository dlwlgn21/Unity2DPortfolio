using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;

public class PlayerSkillSpawnPandaController : BasePlayerSkillController
{
    private PlayerSkillSpawnPandaObject _spawnPanda;
    public override void Init()
    {
        InitByESkillType(ESkillType.Spawn_Panda);
        if (_spawnPanda == null)
        {
            _spawnPanda = Managers.Resources.Instantiate<PlayerSkillSpawnPandaObject>("Prefabs/Player/Skills/SkillSpawnPanda");
            //_uiCoolTimerImg = GameObject.Find("UI_PlayerHUD").gameObject
            DontDestroyOnLoad(_spawnPanda.gameObject);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnShooterAnimValidTiming;
    }

    public override bool TryUseSkill()
    {
        if (IsValidStateToUseSkill())
        {
            _pc.ChangeState(EPlayerState.CAST_LAUNCH);
            ProcessSkillLogic();
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnShooterAnimValidTiming;
    }

    private void OnPlayerSpawnShooterAnimValidTiming(ESkillType eType)
    {
        if (eType == ESkillType.Spawn_Panda)
        {
            _spawnPanda.SpawnShooter(_pc.SpawnShooterPoint.position, _pc.ELookDir);
        }
    }
}
