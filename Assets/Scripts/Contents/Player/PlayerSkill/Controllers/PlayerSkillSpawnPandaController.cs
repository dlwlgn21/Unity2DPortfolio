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
            _spawnPanda = Managers.Resources.Instantiate<PlayerSkillSpawnPandaObject>("Prefabs/Player/Skills/SkillSpawnPandaObject");
            DontDestroyOnLoad(_spawnPanda.gameObject);
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

    private void OnPlayerSpawnAnimValidTiming()
    {
        if (_isUsingSkill)
        {
            _spawnPanda.SpawnShooter(_pc.SpawnPandaPoint.position, _pc.ELookDir);
            _isUsingSkill = false;
        }
    }
}
