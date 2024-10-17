using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CagedShockerController : NormalMonsterController, IMelleAttackable, IDeadBodyReamainable
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.CagedShoker;
        EMonsterAttackType = ENormalMonsterAttackType.MelleAttack;
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.CagedShoker);
        Stat.KnockbackForce = new Vector2(9f, 9f);
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

    public void SpawnDeadBody()
    {
        InstantiateDeadBody("Prefabs/Monsters/DeadBody/CagedShockerDeadBody");
    }


    void OnAttackSwing1SoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_1_PATH);
    }
    void OnAttackSwing2SoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_2_PATH);
    }
}

