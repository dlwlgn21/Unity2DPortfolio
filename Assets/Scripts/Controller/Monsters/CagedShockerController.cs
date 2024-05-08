using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagedShockerController : BaseMonsterController
{
    public override void Init()
    {
        base.Init();
        InitStat();
        MonsterType = EMonsterNames.CagedShoker;
    }
    protected override void InitStat()
    {
        Stat.Init(EMonsterNames.CagedShoker);
    }
    protected override void InitStates()
    {
        base.InitStates();
        AssignAttackState<CagedShockerAttack>();
    }

    protected override void AssignAttackState<CagedShockerAttack>()
    {
        _states[(uint)EMonsterState.ATTACK] = new monster_states.CagedShockerAttack(this);
    }

    public void OnNoramlAttack1ValidSlashed()
    {
        getAttack().OnNoramlAttack1ValidSlashed();
    }
    public void OnNoramlAttack2ValidSlashed()
    {
        getAttack().OnNoramlAttack2ValidSlashed();
    }

    CagedShockerAttack getAttack()
    {
        CagedShockerAttack state = (CagedShockerAttack)_states[(uint)EMonsterState.ATTACK];
        return state;
    }


}
