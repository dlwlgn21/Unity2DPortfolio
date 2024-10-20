using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UI_WSPlayerPopupStatusController : UI_WSPlayerPopupTextController
{
    protected override void Init()
    {
        Skill_BaseController.DeniedUseSkillEventHandler -= OnPlayerDeniedUseSkill;
        Skill_BaseController.DeniedUseSkillEventHandler += OnPlayerDeniedUseSkill;
        PlayerController.PlayerStatusEffectEventHandler -= OnPlayerStatusChanged;
        PlayerController.PlayerStatusEffectEventHandler += OnPlayerStatusChanged;
        UI_PlayerConsumableSlot.DeniedConsumableEventHandler -= OnPlayerDeniedConsumeItem;
        UI_PlayerConsumableSlot.DeniedConsumableEventHandler += OnPlayerDeniedConsumeItem;
        CaveEnteranceController.DeniedEnterCaveEventHandler -= OnPlayerDeniedEnterCave;
        CaveEnteranceController.DeniedEnterCaveEventHandler += OnPlayerDeniedEnterCave;
        DoorController.DeniedDoorOpenEventHandler -= OnPlayerDeniedEnterCave;
        DoorController.DeniedDoorOpenEventHandler += OnPlayerDeniedEnterCave;
        Debug.Log("UI_WSPlayerPopupStatusController Init!");
    }

    void OnPlayerDeniedEnterCave()
    {
        _text.text = "�� �� ����!\n���͸� ���� ��ġ�ؾ���!";
        StartTW();
    }

    void OnPlayerDeniedUseSkill(EDeniedUseSkillCause eDeniedCause)
    {
        switch (eDeniedCause)
        {
            case EDeniedUseSkillCause.NotEnoughMana:
                _text.text = "������ ������!";
                break;
            case EDeniedUseSkillCause.CoolTime:
                _text.text = "��ų�� ��Ÿ���̾�!";
                break;
            default:
                Debug.DebugBreak();
                break;
        }
        StartTW();
    }
    void OnPlayerDeniedConsumeItem(EDeniedUseConsumableItemCause eDeniedCause)
    {
        switch (eDeniedCause)
        {
            case EDeniedUseConsumableItemCause.NoSlot:
                _text.text = "������ ���Կ� ����!";
                break;
            case EDeniedUseConsumableItemCause.CoolTime:
                _text.text = "������ ��Ÿ���̾�!";
                break;
            default:
                Debug.DebugBreak();
                break;
        }
        StartTW();
    }

    void OnPlayerStatusChanged(EAttackStatusEffect eType, float statusEffectTimeInSec)
    {
        switch (eType)
        {
            case EAttackStatusEffect.None:
                break;
            case EAttackStatusEffect.Knockback:
                break;
            case EAttackStatusEffect.Blind:
                break;
            case EAttackStatusEffect.Burn:
                _text.text = "ȭ��!";
                StartTW();
                break;
            case EAttackStatusEffect.Slow:
                _text.text = "���ο�...";
                StartTW();
                break;
            case EAttackStatusEffect.Parallysis:
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed UI_WSPopupStatus");
    }
}