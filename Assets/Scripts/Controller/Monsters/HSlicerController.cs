using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSlicerController : BaseMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
        MonsterType = EMonsterNames.HeabySlicer;
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.HeabySlicer);
        AwarenessRangeToAttack = 2.5f;
    }
}
