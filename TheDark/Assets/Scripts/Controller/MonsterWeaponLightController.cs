using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponLightController : LightController
{
    NormalMonsterController _mc;
    public override void Init()
    {
        base.Init();
        _mc = transform.parent.GetComponent<NormalMonsterController>();
    }



    private void OnMonsterAttackStart(ENormalMonsterState eType)
    {
        if (_mc.ECurrentState == ENormalMonsterState.MELLE_ATTACK || 
            _mc.ECurrentState == ENormalMonsterState.LAUNCH_ATTACK ||
            _mc.ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS ||
            _mc.ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS ||
            _mc.ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB ||
            _mc.ECurrentState == ENormalMonsterState.DIE)
        {
            TurnOffLightGradually();
        }
    }

    private void OnMonsterAttackEnd(ENormalMonsterState eType)
    {
        if (_mc.ECurrentState == ENormalMonsterState.MELLE_ATTACK || _mc.ECurrentState == ENormalMonsterState.LAUNCH_ATTACK)
        {
            TurnOnLight();
        }
    }
}
