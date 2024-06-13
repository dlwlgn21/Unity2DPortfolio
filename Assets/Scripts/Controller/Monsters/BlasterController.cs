using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterController : BaseMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
        MonsterType = EMonsterNames.Blaster;
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.Blaster);
        AwarenessRangeToAttack = 5f;
    }
    protected override void InitStates()
    {
        base.InitStates();
    }
}
