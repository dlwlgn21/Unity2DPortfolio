using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WardenController : NormalMonsterController, IMelleAttackable
{

    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Warden;
        EMonsterAttackType = ENormalMonsterAttackType.MelleAttack;
    }

    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.Warden);
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateMelleAttackState();
    }

    protected override void SetLightControllersTurnOffTimeInSec()
    {
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.4f;
        _dieController.TurnOffGraduallyLightTimeInSec = 1f;
    }

    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MelleAttack] = new monster_states.MelleAttack(this);
    }

    void OnAttackSoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_BLASTER_WARDEN_ATTACK_PATH);
    }
}
