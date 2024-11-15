using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UI_WSPlayerPopupStatusController : UI_WSPlayerPopupTextController
{
    protected override void Init()
    {
        Skill_BaseController.DeniedUseSkillEventHandler -= OnPlayerDeniedUseSkill;
        Skill_BaseController.DeniedUseSkillEventHandler += OnPlayerDeniedUseSkill;
        //PlayerController.PlayerStatusEffectEventHandler -= OnPlayerStatusChanged;
        //PlayerController.PlayerStatusEffectEventHandler += OnPlayerStatusChanged;
        StatusEffectController.PlayerStatusEffectEventHandler -= OnPlayerStatusChanged;
        StatusEffectController.PlayerStatusEffectEventHandler += OnPlayerStatusChanged;
        UI_PlayerConsumableSlot.DeniedConsumableEventHandler -= OnPlayerDeniedConsumeItem;
        UI_PlayerConsumableSlot.DeniedConsumableEventHandler += OnPlayerDeniedConsumeItem;
        CaveEnteranceController.DeniedEnterCaveEventHandler -= OnPlayerDeniedEnterCave;
        CaveEnteranceController.DeniedEnterCaveEventHandler += OnPlayerDeniedEnterCave;
        DoorController.DeniedDoorOpenEventHandler -= OnPlayerDeniedEnterCave;
        DoorController.DeniedDoorOpenEventHandler += OnPlayerDeniedEnterCave;
        PlayScene.PlaySceneLoadedEventHandelr -= OnSceneLoaded;
        PlayScene.PlaySceneLoadedEventHandelr += OnSceneLoaded;
    }

    void OnSceneLoaded()
    {
        _text.text = "";
    }

    void OnPlayerDeniedEnterCave()
    {
        _text.text = "들어갈 수 없어!\n몬스터를 전부 퇴치해야해!";
        StartTW();
    }

    void OnPlayerDeniedUseSkill(EDeniedUseSkillCause eDeniedCause)
    {
        switch (eDeniedCause)
        {
            case EDeniedUseSkillCause.NotEnoughMana:
                _text.text = "마나가 부족해!";
                break;
            case EDeniedUseSkillCause.CoolTime:
                _text.text = "스킬이 쿨타임이야!";
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
                _text.text = "포션이 슬롯에 없어!";
                break;
            case EDeniedUseConsumableItemCause.CoolTime:
                _text.text = "포션이 쿨타임이야!";
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
                _text.text = "화상!";
                StartTW();
                break;
            case EAttackStatusEffect.Slow:
                _text.text = "슬로우...";
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
