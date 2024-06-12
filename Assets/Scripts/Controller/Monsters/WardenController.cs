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
    protected override void InitStates()
    {
        base.InitStates();
        AssignAttackState<WardenAttack>();
    }

    protected override void AssignAttackState<WardenAttack>()
    {
        _states[(uint)EMonsterState.ATTACK] = new monster_states.WardenAttack(this);
    }

    public void OnWardenValidAttack()
    {
        getAttack().OnWardenValidAttack();
    }

    WardenAttack getAttack()
    {
        WardenAttack state = (WardenAttack)_states[(uint)EMonsterState.ATTACK];
        return state;
    }
}
