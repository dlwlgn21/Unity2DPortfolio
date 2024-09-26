using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPlayerHpBar : UIHealthBar
{
    private void Start()
    {
        SetFullHpBarRatio();
    }
    public override void Init()
    {
        //SetFullHpBarRatio();
        PlayerController.HitUIEventHandler += OnPlayerHittedByMonsterNormalAttack;
    }

    private void OnDestroy()
    {
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
    }
    public void OnPlayerHittedByMonsterNormalAttack(int damage, int beforeDamgeHp, int afterDamageHp)
    {
        DecraseHP(beforeDamgeHp, afterDamageHp);
    }
}
