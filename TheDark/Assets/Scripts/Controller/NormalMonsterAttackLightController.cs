using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterAttackLightController : LightController
{
    NormalMonsterController _mc;
    public override void Init()
    {
        base.Init();
        _mc = transform.parent.GetComponent<NormalMonsterController>();
        NormalMonsterController.MonsterAttackStartEventHandler += OnMonsterAttackStart;
        NormalMonsterController.MonsterAttackEndEventHandler += OnMonsterAttackEnd;
        
    }

    private void OnDestroy()
    {
        NormalMonsterController.MonsterAttackStartEventHandler -= OnMonsterAttackStart;
        NormalMonsterController.MonsterAttackEndEventHandler -= OnMonsterAttackEnd;
    }

    private void OnMonsterAttackStart()
    {
        Debug.Log("OnMonsterAttackStart");
        if (_mc.ECurrentState == ENormalMonsterState.MELLE_ATTACK || 
            _mc.ECurrentState == ENormalMonsterState.LAUNCH_ATTACK)
        {
            TurnOnLight();
        }
    }

    private void OnMonsterAttackEnd()
    {
        Debug.Log("OnMonsterAttackEnd");
        if (_mc.ECurrentState == ENormalMonsterState.MELLE_ATTACK ||
            _mc.ECurrentState == ENormalMonsterState.LAUNCH_ATTACK)
        {
            TurnOffLightGradually();
        }
    }
}
