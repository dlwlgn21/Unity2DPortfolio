using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
public class PlayerSkillSpawnRepaerController : BasePlayerSkillController
{
    private const float SPAWN_REAPER_COOL_TIME_IN_SEC = 5f;
    private PlayerSkillSpawnReaperObject _spawnReaper;
    public override void Init()
    {
        _eSkillType = ESkillType.Spawn_Reaper;
        _initCoolTime = SPAWN_REAPER_COOL_TIME_IN_SEC;
        SkillCoolTimeInSec = SPAWN_REAPER_COOL_TIME_IN_SEC;
        IsCanUseSkill = true;
        Debug.Assert(_uiCoolTimerImg != null);
        if (_spawnReaper == null)
        {
            GameObject spawnReaper = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/SkillSpawnReaper");
            spawnReaper = Instantiate(spawnReaper);
            _spawnReaper = spawnReaper.GetComponent<PlayerSkillSpawnReaperObject>();
            DontDestroyOnLoad(spawnReaper);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnReaperAnimValidTiming;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnReaperAnimValidTiming;
    }
    private void Update()
    {
        if (Input.GetKeyDown(PlayerController.KeySpawnReaper))
        {
            if (IsValidStateToUseSkill())
            {
                UseSkill(ESkillType.Spawn_Reaper);
            }
        }
    }
    private void OnPlayerSpawnReaperAnimValidTiming(ESkillType eType)
    {
        if (eType == ESkillType.Spawn_Reaper)
        {
            _spawnReaper.SpawnReaper(_pc.SpawnReaperPoint.position, _pc.ELookDir);
        }
    }
}
