using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UI_WSPlayerPopupStatusController : UI_WSPlayerPopupTextController
{
    protected override void Init()
    {
        Skill_BaseController.ManaNotEnoughEventHandler -= OnPlayerManaNotEnoughUseSkill;
        Skill_BaseController.ManaNotEnoughEventHandler += OnPlayerManaNotEnoughUseSkill;
        PlayerController.PlayerStatusEffectEventHandler -= OnPlayerStatusChanged;
        PlayerController.PlayerStatusEffectEventHandler += OnPlayerStatusChanged;
        UI_PlayerConsumableSlot.DeniedConsumableEventHandler -= OnPlayerConsumeItem;
        UI_PlayerConsumableSlot.DeniedConsumableEventHandler += OnPlayerConsumeItem;
    }

    void OnPlayerManaNotEnoughUseSkill()
    {
        _text.text = "������ ������!";
        StartTW();
    }

    void OnPlayerConsumeItem()
    {
        _text.text = "������ ����� �� ����!";
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
}
