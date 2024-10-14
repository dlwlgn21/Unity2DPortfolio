using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public sealed class PlayerForceFieldAnimController : PlayerEffectAnimController
{
    protected override void Init()
    {
        PlayerController.HitEffectEventHandler -= OnPlayerHittedByMonsterNormalAttack;
        PlayerController.HitEffectEventHandler += OnPlayerHittedByMonsterNormalAttack;
    }
    void OnPlayerHittedByMonsterNormalAttack(EPlayerState eState)
    {
        SetComponentEnable(true);
        _animator.Play("ForceFieldEffect", -1, 0f);
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
