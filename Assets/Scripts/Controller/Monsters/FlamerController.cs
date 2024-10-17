using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamerController : NormalMonsterController, IMelleAttackable
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Flamer;
        EMonsterAttackType = ENormalMonsterAttackType.MelleAttack;
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.Flamer);
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateMelleAttackState();
    }
    protected override void SetLightControllersTurnOffTimeInSec()
    {
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.2f;
        _dieController.TurnOffGraduallyLightTimeInSec = 1f;
    }

    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MelleAttack] = new monster_states.MelleAttack(this);
    }

    void OnAttckSoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_FLAMER_ATTACK_PATH);
    }
}
