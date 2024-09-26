public class UINormalMonsterStatusTextController : UITextPopup
{
    private NormalMonsterController _mc;
    private readonly static string ATTACK_TEXT = "공격!";
    private readonly static string HITTED_KNOCKBACK_TEXT = "넉백!";
    private readonly static string HITTED_PARALYSIS = "마비!";
    protected override void Init()
    {
        _ePopupType = EPopupType.STATUS;
        _mc = transform.parent.GetComponent<NormalMonsterController>();
        NormalMonsterController.MonsterChangeStateEventHandler += OnMonsterChangeState;
    }

    private void OnDestroy()
    {
        NormalMonsterController.MonsterChangeStateEventHandler -= OnMonsterChangeState;
    }
    public void OnMonsterChangeState(ENormalMonsterState eState)
    {
        switch (eState)
        {
            case ENormalMonsterState.MELLE_ATTACK:
            case ENormalMonsterState.LAUNCH_ATTACK:
                if (_mc.ECurrentState == ENormalMonsterState.MELLE_ATTACK || _mc.ECurrentState == ENormalMonsterState.LAUNCH_ATTACK)
                {
                    ShowPopup(ATTACK_TEXT);
                }
                break;
            case ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                if (_mc.ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS || _mc.ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB)
                {
                    ShowPopup(HITTED_KNOCKBACK_TEXT);
                }
                break;
            case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                if (_mc.ECurrentState == ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS)
                {
                    ShowPopup(HITTED_PARALYSIS);
                }
                break;
        }
    }


}
