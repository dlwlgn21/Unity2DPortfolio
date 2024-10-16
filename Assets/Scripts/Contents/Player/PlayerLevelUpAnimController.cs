using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerLevelUpAnimController : PlayerEffectAnimController
{
    protected override void Init()
    {
        PlayerStat.OnLevelUpEventHandler -= OnPlayerLevelUp;
        PlayerStat.OnLevelUpEventHandler += OnPlayerLevelUp;
        _lightController.TurnOffGraduallyLightTimeInSec = 0.4f;
    }
    void OnPlayerLevelUp(int levelUpCount)
    {
        SetComponentEnable(true);
        _animator.Play("LevelUp", -1, 0f);
    }
    protected override void OnAnimStart()
    {
        _pc.IsInvincible = true;
        _lightController.TurnOnLight();
    }
    protected override void OnAnimFullyPlayed()
    {
        _pc.IsInvincible = false;
        SetComponentEnable(false);
    }
}
