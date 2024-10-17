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
    }

    void OnPlayerManaNotEnoughUseSkill()
    {
        _text.text = "마나가 부족해!";
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
