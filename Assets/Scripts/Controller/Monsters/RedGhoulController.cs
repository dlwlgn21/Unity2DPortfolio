using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhoulController : BaseMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
        MonsterType = EMonsterNames.RedGhoul;
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.RedGhoul);
        AwarenessRangeToAttack = 2f;
    }
}
