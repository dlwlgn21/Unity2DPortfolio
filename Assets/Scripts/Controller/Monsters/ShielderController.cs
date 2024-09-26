using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderController : NormalMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Shielder;
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.Shielder);
    }
}
