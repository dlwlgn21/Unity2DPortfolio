using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackLightController : LightController
{
    static readonly float DELAYED_TURN_ON_LIGHT_TIME_IN_SEC = 0.1f;
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
    private void OnPlayerNormalAttackStart(EPlayerState eState)
    {
        switch (eState)
        {
            case EPlayerState.NormalAttack_1:
                StartCoroutine(TurnOnLightDelayed());
                break;
        }
    }

    private void OnPlayerAttackEnd()
    {
        TurnOffLightGradually();
    }


    IEnumerator TurnOnLightDelayed()
    {
        yield return new WaitForSeconds(DELAYED_TURN_ON_LIGHT_TIME_IN_SEC);
        TurnOnLight();
    }
}
