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
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.HeabySlicer);
    }
    protected override void initStates()
    {
        base.initStates();
        AssignAttackState<HSlicerAttack>();
    }

    protected override void AssignAttackState<HSlicerAttack>()
    {
        mStates[(uint)EMonsterState.ATTACK] = new monster_states.HSlicerAttack();
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
        HSlicerAttack state = (HSlicerAttack)mStates[(uint)EMonsterState.ATTACK];
        return state;
    }

}
