using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStaticRegisterEventManager
{
    private bool _isAlreadyInited = false;
    public void Init()
    {
        if (!_isAlreadyInited)
        {
            #region PLAYER
            PlayerController.HitEffectEventHandler += Managers.CamShake.OnPlayerHittedByMonsterNormalAttack;
            PlayerController.PlayerChangeStateEventHandler += Managers.Sound.OnPlayerChangeState;
            #endregion

            #region MONSTER
            BaseMonsterController.BigAttackEventHandler += Managers.CamManager.OnMonsterHittedByPlayerNormalAttack;
            BaseMonsterController.HittedByNormalAttackNoArgsEventHandler += Managers.CamShake.OnMonsterHittedByPlayerNormalAttack;
            NormalMonsterController.MonsterChangeStateEventHandler += Managers.CamShake.OnMonsterHittedByPlayerSkill;
            #endregion

            _isAlreadyInited = true;
        }

    }

    public void Clear()
    {
        #region PLAYER
        PlayerController.HitEffectEventHandler -= Managers.CamShake.OnPlayerHittedByMonsterNormalAttack;
        PlayerController.PlayerChangeStateEventHandler -= Managers.Sound.OnPlayerChangeState;
        #endregion

        #region MONSTER
        BaseMonsterController.BigAttackEventHandler -= Managers.CamManager.OnMonsterHittedByPlayerNormalAttack;
        BaseMonsterController.HittedByNormalAttackNoArgsEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerNormalAttack;
        NormalMonsterController.MonsterChangeStateEventHandler -= Managers.CamShake.OnMonsterHittedByPlayerSkill;
        #endregion


    }

}
