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
        EMonsterAttackType = ENormalMonsterAttackType.MELLE_ATTACK;
    }
    public override void InitStat()
    {
        Stat.Init(EMonsterNames.RedGhoul);
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


