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
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.Warden);
    }
    protected override void InitStates()
    {
        base.InitStates();
        AssignAttackState<WardenAttack>();
    }

    protected override void AssignAttackState<WardenAttack>()
    {
        mStates[(uint)EMonsterState.ATTACK] = new monster_states.WardenAttack(this);
    }

    public void OnWardenValidAttack()
    {
        getAttack().OnWardenValidAttack();
    }

    WardenAttack getAttack()
    {
        WardenAttack state = (WardenAttack)mStates[(uint)EMonsterState.ATTACK];
        return state;
    }
}
