using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterController : NormalMonsterController, IMelleAttackable, IDeadBodyReamainable
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Blaster;
        EMonsterAttackType = ENormalMonsterAttackType.MelleAttack;
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.Blaster);
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
        InstantiateDeadBody("Prefabs/Monsters/DeadBody/BlasterDeadBody");
    }
    void OnAttackSoundTiming()
    {
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_MONSTER_BLASTER_WARDEN_ATTACK_PATH);
    }
}
