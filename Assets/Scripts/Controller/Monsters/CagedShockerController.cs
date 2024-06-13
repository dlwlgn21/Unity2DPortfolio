using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagedShockerController : BaseMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
        MonsterType = EMonsterNames.CagedShoker;
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.CagedShoker);
        AwarenessRangeToAttack = 2.5f;
    }
}
