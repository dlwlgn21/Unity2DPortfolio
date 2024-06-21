using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackLightController : AttackLightController
{
    public override void Init()
    {
        base.Init();
        PlayerController.PlayerChangeStateEventHandler += OnPlayerNormalAttackStart;
        player_states.NormalAttackState.NormalAttackExitEventHandler += OnPlayerAttackEnd;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerChangeStateEventHandler -= OnPlayerNormalAttackStart;
        player_states.NormalAttackState.NormalAttackExitEventHandler -= OnPlayerAttackEnd;
    }
    public void OnPlayerNormalAttackStart(EPlayerState eState)
    {
        switch (eState)
        {
            case EPlayerState.NORMAL_ATTACK_1:
            case EPlayerState.NORMAL_ATTACK_2:
            case EPlayerState.NORMAL_ATTACK_3:
                TurnOnLight();
                break;
        }
    }

    public void OnPlayerAttackEnd()
    {
        TurnOffLightGradually();
    }
}
