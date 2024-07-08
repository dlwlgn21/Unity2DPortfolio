using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagedShockerController : NormalMonsterController, IMelleAttackable
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.CagedShoker;
        EMonsterAttackType = ENormalMonsterAttackType.MELLE_ATTACK;
    }
    public override void InitStat()
    {
        Stat.Init(EMonsterNames.CagedShoker);
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

