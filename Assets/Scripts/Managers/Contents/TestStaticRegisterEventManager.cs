using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStaticRegisterEventManager
{
    public void Init()
    {
        #region PLAYER
        PlayerController.HitEffectEventHandler += Managers.CamShake.OnPlayerHittedByMonsterNormalAttack;
        PlayerController.HitEventHandler += Managers.TimeManager.OnPlayerHittedByMonster;
        PlayerController.PlayerChangeStateEventHandler += Managers.Sound.OnPlayerChangeState;
        #endregion

        #region MONSTER
        BaseMonsterController.BigAttackEventHandler += Managers.CamManager.OnMonsterHittedByPlayerNormalAttack;
        BaseMonsterController.BigAttackEventHandler += Managers.TimeManager.OnMonsterHittedByPlayerNormalAttack;
        BaseMonsterController.HittedByNormalAttackNoArgsEventHandler += Managers.CamShake.OnMonsterHittedByPlayerNormalAttack;
        BaseMonsterController.MonsterChangeStateEventHandler += Managers.CamShake.OnMonsterHittedByPlayerSkill;
        #endregion

    }

    public void Clear()
    {
        #region PLAYER
        PlayerController.HitEffectEventHandler -= Managers.CamShake.OnPlayerHittedByMonsterNormalAttack;
        PlayerController.HitEventHandler -= Managers.TimeManager.OnPlayerHittedByMonster;
        PlayerController.PlayerChangeStateEventHandler -= Managers.Sound.OnPlayerChangeState;
        #endregion

        #region MONSTER
        BaseMonsterController.BigAttackEventHandler -= Managers.CamManager.OnMonsterHittedByPlayerNormalAttack;
        BaseMonsterController.BigAttackEventHandler -= Managers.TimeManager.OnMonsterHittedByPlayerNormalAttack;
        BaseMonsterController.HittedByNormalAttackNoArgsEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerNormalAttack;
        BaseMonsterController.MonsterChangeStateEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerSkill;
        #endregion


    }

}
