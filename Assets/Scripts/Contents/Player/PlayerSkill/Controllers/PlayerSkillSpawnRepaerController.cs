using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSpawnRepaerController : BasePlayerSkillController
{
    private const float SPAWN_REAPER_COOL_TIME_IN_SEC = 5f;
    private PlayerSkillSpawnReaperObject _spawnReaper;
    public override void Init()
    {
        _eSkillType = EPlayerSkill.SPAWN_REAPER;
        _initCoolTime = SPAWN_REAPER_COOL_TIME_IN_SEC;
        SkillCoolTime = SPAWN_REAPER_COOL_TIME_IN_SEC;
        IsPossibleDoSkill = true;
        Debug.Assert(_uiCoolTimerImg != null);
        if (_spawnReaper == null)
        {
            GameObject spawnReaper = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/SkillSpawnReaper");
            spawnReaper = Instantiate(spawnReaper);
            _spawnReaper = spawnReaper.GetComponent<PlayerSkillSpawnReaperObject>();
            DontDestroyOnLoad(spawnReaper);
        }
        PlayerController.PlayerSkillValidAnimTimingEventHandler += OnPlayerSpawnReaperAnimValidTiming;
        PlayerController.PlayerSkillKeyDownEventHandler += OnPlayerSpawnReaperKeyDown;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerSkillValidAnimTimingEventHandler -= OnPlayerSpawnReaperAnimValidTiming;
        PlayerController.PlayerSkillKeyDownEventHandler -= OnPlayerSpawnReaperKeyDown;
    }

    private void OnPlayerSpawnReaperKeyDown(EPlayerSkill eType)
    {
        if (eType == EPlayerSkill.SPAWN_REAPER)
        {
            if (IsPosibbleValidStateToDoSkill())
            {
                _pc.ChangeState(EPlayerState.CAST_SPAWN);
                _uiCoolTimerImg.StartCoolTime(SkillCoolTime);
                IsPossibleDoSkill = false;
                StartCoroutine(AfterGivenCoolTimePossibleDoSkillCo(SkillCoolTime));
            }
        }
    }

    private void OnPlayerSpawnReaperAnimValidTiming(EPlayerSkill eType)
    {
        if (eType == EPlayerSkill.SPAWN_REAPER)
        {
            _spawnReaper.SpawnReaper(_pc.SpawnReaperPoint.position, _pc.ELookDir);
        }
    }
}
