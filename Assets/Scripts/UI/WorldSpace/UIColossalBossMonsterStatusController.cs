public class UIColossalBossMonsterStatusController : UITextPopup
{
    protected override void Init()
    {
        _ePopupType = EPopupType.STATUS;
        ColossalBossMonsterController.ColossalChangeStateEventHandler += OnColossalBossChangeState;
    }

    private void OnDestroy()
    {
        ColossalBossMonsterController.ColossalChangeStateEventHandler -= OnColossalBossChangeState;
    }
    private void OnColossalBossChangeState(EColossalBossState eState)
    {
        switch (eState)
        {
            case EColossalBossState.FIST_MELLE_ATTACK:
                ShowPopup("주먹공격!");
                break;
            case EColossalBossState.SPIN_MELLE_ATTACK:
                ShowPopup("스핀!");
                break;
            case EColossalBossState.BURST_MEELE_ATTACK:
            case EColossalBossState.BURFED_BURST_ATTACK:
                ShowPopup("분출!");
                break;
            case EColossalBossState.BURF:
                ShowPopup("2페이즈 돌입!");
                break;
            case EColossalBossState.DIE:
                break;
        }
    }
}
