using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhoulController : NormalMonsterController, IMelleAttackable
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.RedGhoul;
        EMonsterAttackType = ENormalMonsterAttackType.MelleAttack;
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.RedGhoul);
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateMelleAttackState();
    }

    protected override void SetLightControllersTurnOffTimeInSec()
    {
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.3f;
        _dieController.TurnOffGraduallyLightTimeInSec = 1f;
    }
    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MelleAttack] = new monster_states.MelleAttack(this);
    }
    void OnAttackSoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_RED_GHOUL_ATTACK_PATH);
    }

    void OnDieSoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_DIE_EXPOLOSION_1);
    }
}


