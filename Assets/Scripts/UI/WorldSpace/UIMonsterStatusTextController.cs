using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterStatusTextController : UITextPopup
{
    private BaseMonsterController _mc;

    private readonly static string ATTACK_TEXT = "����!";
    private readonly static string HITTED_KNOCKBACK_TEXT = "�˹�!";
    private readonly static string HITTED_PARALYSIS = "����!";
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
        // TODO : �̰� ���� ����ų �� ����.
        switch (eState)
        {
            case ENormalMonsterState.MELLE_ATTACK:
            case ENormalMonsterState.LAUNCH_ATTACK:
                // ChangeState�� ������ ���� ���� ��찡 ��� �־ �װ� ������.
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
        // TODO : �̰� ���� ����ų �� ����.
        switch (eState)
        {
            case EColossalBossState.FIST_MELLE_ATTACK:
                ShowPopup("�ָ԰���!");
                break;
            case EColossalBossState.SPIN_MELLE_ATTACK:
                ShowPopup("����!");
                break;
            case EColossalBossState.BURST_MEELE_ATTACK:
            case EColossalBossState.BURFED_BURST_ATTACK:
                ShowPopup("����!");
                break;
            case EColossalBossState.BURF:
                ShowPopup("2������ ����!");
                break;
            case EColossalBossState.DIE:
                break;
        }
    }
}
