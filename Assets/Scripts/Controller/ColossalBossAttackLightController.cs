using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColossalBossAttackLightController : LightController
{
    public void OnMonsterAttackStart()
    {
        TurnOnLight();
    }

    public void OnMonsterAttackEnd()
    {
        TurnOffLightGradually();
    }
}
