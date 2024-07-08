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
        NormalMonsterController.MonsterChangeStateEventHandler += OnMonsterChangeState;
        ColossalBossMonsterController.ColossalChangeStateEventHandler += OnColossalBossChangeState;
    }

    private void OnDestroy()
    {
        NormalMonsterController.MonsterChangeStateEventHandler -= OnMonsterChangeState;
        ColossalBossMonsterController.ColossalChangeStateEventHandler -= OnColossalBossChangeState;
    }
    public void OnMonsterChangeState(ENormalMonsterState eState)
    {
        // TODO : 이곳 말썽 일으킬 수 있음.
        switch (eState)
        {
            case ENormalMonsterState.MELLE_ATTACK:
            case ENormalMonsterState.LAUNCH_ATTACK:
                // ChangeState할 때마다 공격 띄우는 경우가 계속 있어서 그거 방지용.
                //if (eState == ENormalMonsterState.MELLE_ATTACK || eState == ENormalMonsterState.LAUNCH_ATTACK)
                //{
                //    ShowPopup(ATTACK_TEXT);
                //}
                ShowPopup(ATTACK_TEXT);
                break;
            case ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                //if (eState == ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS || eState == ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB)
                //{
                //    ShowPopup(HITTED_KNOCKBACK_TEXT);
                //}
                ShowPopup(HITTED_KNOCKBACK_TEXT);
                break;
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                //if (eState == ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS)
                //{
                //    ShowPopup(HITTED_PARALYSIS);
                //}
                ShowPopup(HITTED_PARALYSIS);
                break;
        }
    }

    public void OnColossalBossChangeState(EColossalBossState eState)
    {
        // TODO : 이곳 말썽 일으킬 수 있음.
        switch (eState)
        {
            case EColossalBossState.FIST_MELLE_ATTACK:
                ShowPopup("주먹공격!");
                break;
            case EColossalBossState.SPIN_MELLE_ATTACK:
                ShowPopup("스핀!");
                break;
            case EColossalBossState.BURST_MEELE_ATTACK:
            case EColossalBossState.BURFED_BURST_ATTACK:
                ShowPopup("분출!");
                break;
            case EColossalBossState.BURF:
                ShowPopup("2페이즈 돌입!");
                break;
            case EColossalBossState.DIE:
                break;
        }
    }
}
