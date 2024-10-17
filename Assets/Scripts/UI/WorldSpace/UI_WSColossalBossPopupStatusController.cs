public class UI_WSColossalBossPopupStatusController : UI_WSMonsterPopupTextController
{
    protected override void Init()
    {
        _ePopupType = EPopupType.STATUS;
        ColossalBossMonsterController.ColossalChangeStateEventHandler += OnColossalBossChangeState;
    }

    void OnDestroy()
    {
        ColossalBossMonsterController.ColossalChangeStateEventHandler -= OnColossalBossChangeState;
    }
    void OnColossalBossChangeState(EColossalBossState eState)
    {
        switch (eState)
        {
            case EColossalBossState.FistMelleAttack:
                ShowPopup("주먹공격!");
                break;
            case EColossalBossState.SpinMelleAttack:
                ShowPopup("스핀!");
                break;
            case EColossalBossState.BurstMelleAttack:
            case EColossalBossState.BurfedBurstMelleAttack:
                ShowPopup("분출!");
                break;
            case EColossalBossState.Burf:
                ShowPopup("2페이즈 돌입!");
                break;
            case EColossalBossState.Die:
                break;
        }
    }
}
