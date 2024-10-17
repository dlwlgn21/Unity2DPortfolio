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
                ShowPopup("�ָ԰���!");
                break;
            case EColossalBossState.SpinMelleAttack:
                ShowPopup("����!");
                break;
            case EColossalBossState.BurstMelleAttack:
            case EColossalBossState.BurfedBurstMelleAttack:
                ShowPopup("����!");
                break;
            case EColossalBossState.Burf:
                ShowPopup("2������ ����!");
                break;
            case EColossalBossState.Die:
                break;
        }
    }
}
