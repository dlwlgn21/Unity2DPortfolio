using System.Diagnostics;

public class UI_WSNormalMonsterPopupStatusController : UI_WSMonsterPopupTextController
{
    private const string ATTACK_TEXT = "����!";
    private const string HITTED_KNOCKBACK_TEXT = "�˹�!";
    private const string HITTED_PARALYSIS = "����!";
    protected override void Init()
    {
        _ePopupType = EPopupType.STATUS;
    }
    public void ShowMonsterStatus(ENormalMonsterState eState)
    {
        switch (eState)
        {
            case ENormalMonsterState.MelleAttack:
            case ENormalMonsterState.LaunchAttack:
                ShowPopup(ATTACK_TEXT);
                break;
            case ENormalMonsterState.HitByPlayerBlockSucces:
            case ENormalMonsterState.HitByPlayerSkillKnockbackBoom:
                ShowPopup(HITTED_KNOCKBACK_TEXT);
                break;
            case ENormalMonsterState.HitByPlayerSkillParallysis:
                ShowPopup(HITTED_PARALYSIS);
                break;
        }
    }
}
