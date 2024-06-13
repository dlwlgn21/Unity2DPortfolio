using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenController : BaseMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
        MonsterType = EMonsterNames.Warden;
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.Warden);
        AwarenessRangeToAttack = 2f;
    }
}
