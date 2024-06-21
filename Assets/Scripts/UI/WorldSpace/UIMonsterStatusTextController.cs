using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterStatusTextController : UITextPopup
{
    private BaseMonsterController _mc;

    private readonly static string ATTACK_TEXT = "공격!";
    private readonly static string HITTED_KNOCKBACK_TEXT = "넉백!";
    private readonly static string HITTED_PARALYSIS = "마비!";
    protected override void Init()
    {
        _ePopupType = EPopupType.STATUS;
        _mc = transform.parent.GetComponent<BaseMonsterController>();
        BaseMonsterController.MonsterChangeStateEventHandler += OnMonsterChangeState;
    }

    private void OnDestroy()
    {
        BaseMonsterController.MonsterChangeStateEventHandler -= OnMonsterChangeState;
    }
    public void OnMonsterChangeState(EMonsterState eState)
    {
        switch (_mc.ECurrentState)
        {
            case EMonsterState.ATTACK:
                if (eState == EMonsterState.ATTACK)
                {
                    ShowPopup(ATTACK_TEXT);
                }
                break;
            case EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
            case EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                if (eState == EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS || eState == EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB)
                {
                    ShowPopup(HITTED_KNOCKBACK_TEXT);
                }
                break;
            case EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                if (eState == EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS)
                {
                    ShowPopup(HITTED_PARALYSIS);
                }
                break;
        }
    }

}
