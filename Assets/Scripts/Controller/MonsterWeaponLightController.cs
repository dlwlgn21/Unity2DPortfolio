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
        if (_mc.ECurrentState == ENormalMonsterState.MelleAttack || 
            _mc.ECurrentState == ENormalMonsterState.LaunchAttack ||
            _mc.ECurrentState == ENormalMonsterState.HitByPlayerBlockSucces ||
            _mc.ECurrentState == ENormalMonsterState.HitByPlayerSkillParallysis ||
            _mc.ECurrentState == ENormalMonsterState.HitByPlayerSkillKnockbackBoom ||
            _mc.ECurrentState == ENormalMonsterState.Die)
        {
            TurnOffLightGradually();
        }
    }

    private void OnMonsterAttackEnd(ENormalMonsterState eType)
    {
        if (_mc.ECurrentState == ENormalMonsterState.MelleAttack || _mc.ECurrentState == ENormalMonsterState.LaunchAttack)
        {
            TurnOnLight();
        }
    }
}
