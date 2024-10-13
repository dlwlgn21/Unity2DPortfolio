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
        EMonsterAttackType = ENormalMonsterAttackType.MelleAttack;
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.CagedShoker);
        Stat.KnockbackForce = new Vector2(6f, 6f);
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateMelleAttackState();
    }
    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MelleAttack] = new monster_states.MelleAttack(this);
    }
}

