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
    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MelleAttack] = new monster_states.MelleAttack(this);
    }
}
