using System.Diagnostics;

public class UINormalMonsterStatusTextController : UITextPopup
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
            case ENormalMonsterState.MELLE_ATTACK:
            case ENormalMonsterState.LAUNCH_ATTACK:
                ShowPopup(ATTACK_TEXT);
                break;
            case ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                ShowPopup(HITTED_KNOCKBACK_TEXT);
                break;
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                ShowPopup(HITTED_PARALYSIS);
                break;
        }
    }


}
