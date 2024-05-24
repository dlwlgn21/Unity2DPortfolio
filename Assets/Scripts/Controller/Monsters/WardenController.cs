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

    //public void OnValidHittedAnimEnd()
    //{
    //    if (ECurrentState == EMonsterState.HITTED)
    //    {
    //        ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED]).OnHittedAnimFullyPlayed();
    //        return;
    //    }
    //    if (ECurrentState == EMonsterState.HITTED_KNOCKBACK)
    //    {
    //        ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED_KNOCKBACK]).OnHittedAnimFullyPlayed();
    //        return;
    //    }
    //    Debug.Assert(false, "ECurrentState is strange...");
    //}

    WardenAttack getAttack()
    {
        WardenAttack state = (WardenAttack)_states[(uint)EMonsterState.ATTACK];
        return state;
    }
}
