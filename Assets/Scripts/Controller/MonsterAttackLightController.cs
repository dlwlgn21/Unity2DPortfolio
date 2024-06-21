using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackLightController : AttackLightController
{
    BaseMonsterController _mc;
    public override void Init()
    {
        base.Init();
        _mc = transform.parent.GetComponent<BaseMonsterController>();
        monster_states.Attack.MonsterAttackStartEventHandler += OnMonsterAttackStart;
        monster_states.Attack.MonsterAttackEndEventHandler += OnMonsterAttackEnd;
    }

    private void OnDestroy()
    {
        monster_states.Attack.MonsterAttackStartEventHandler -= OnMonsterAttackStart;
        monster_states.Attack.MonsterAttackEndEventHandler -= OnMonsterAttackEnd;
    }

    public void OnMonsterAttackStart()
    {
        if (_mc.ECurrentState == EMonsterState.ATTACK)
        {
            TurnOnLight();
        }
    }

    public void OnMonsterAttackEnd()
    {
        if (_mc.ECurrentState != EMonsterState.ATTACK)
        {
            TurnOffLightGradually();
        }
    }

}
