using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterController : NormalMonsterController, IMelleAttackable
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Blaster;
        EMonsterAttackType = ENormalMonsterAttackType.MELLE_ATTACK;
    }
    public override void InitStat()
    {
        Stat.Init(EMonsterNames.Blaster);
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateMelleAttackState();
    }
    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MELLE_ATTACK] = new monster_states.MelleAttack(this);
    }
}
