using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenController : NormalMonsterController, IMelleAttackable
{

    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Warden;
        EMonsterAttackType = ENormalMonsterAttackType.MELLE_ATTACK;
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
    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MELLE_ATTACK] = new monster_states.MelleAttack(this);
    }
}
