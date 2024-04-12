using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterController : BaseMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.Blaster);
    }
    protected override void InitStates()
    {
        base.InitStates();
        AssignAttackState<BlasterAttack>();
    }

    protected override void AssignAttackState<BlasterAttack>()
    {
        mStates[(uint)EMonsterState.ATTACK] = new monster_states.BlasterAttack(this);
    }

    public void OnBlasterValidAttack()
    {
        getAttack().OnBlaterValidAttack();
    }

    BlasterAttack getAttack()
    {
        BlasterAttack state = (BlasterAttack)mStates[(uint)EMonsterState.ATTACK];
        return state;
    }
}
