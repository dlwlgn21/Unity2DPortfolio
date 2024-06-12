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
    protected override void InitStates()
    {
        base.InitStates();
        AssignAttackState<HSlicerAttack>();
    }

    protected override void AssignAttackState<HSlicerAttack>()
    {
        _states[(uint)EMonsterState.ATTACK] = new monster_states.HSlicerAttack(this);
    }

    public void OnHSlicerValidAttack1()
    {
        getAttack().OnHSlicerValidAttack1();
    }
    public void OnHSlicerValidAttack2()
    {
        getAttack().OnHSlicerValidAttack2();
    }
    HSlicerAttack getAttack()
    {
        HSlicerAttack state = (HSlicerAttack)_states[(uint)EMonsterState.ATTACK];
        return state;
    }

}
