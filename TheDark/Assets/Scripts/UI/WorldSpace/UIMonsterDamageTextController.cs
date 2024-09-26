public class UIMonsterDamageTextController : UITextPopup
{
    private BaseMonsterController _mc;
    protected override void Init()
    {
        _ePopupType = EPopupType.DAMAGE;
        _mc = transform.parent.GetComponent<BaseMonsterController>();
        BaseMonsterController.HittedByNormalAttackWSUIEventHandler += OnMonsterHittedByPlayerNormalAttack;
    }

    private void OnDestroy()
    {
        BaseMonsterController.HittedByNormalAttackWSUIEventHandler -= OnMonsterHittedByPlayerNormalAttack;
    }

    public void OnMonsterHittedByPlayerNormalAttack(int actualDamage, int beforeDamageHP, int afterDamageHP)
    {
        if (_mc.IsHittedByPlayerNormalAttack)
        {
            ShowPopup(actualDamage);
        }
    }
}
