using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_PlayerHpBar : UI_HealthBar
{
    private void Start()
    {
        SetFullHpBarRatio();
    }
    public override void Init()
    {
        _stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        Debug.Assert(_stat != null);
        //SetFullHpBarRatio();
        PlayerController.HitUIEventHandler += OnPlayerHittedByMonsterNormalAttack;
        PlayerController.PlayerIncreaseHpEventHandler += OnPlayerHpItemUsed;
    }

    private void OnDestroy()
    {
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
        PlayerController.PlayerIncreaseHpEventHandler -= OnPlayerHpItemUsed;
    }
    void OnPlayerHittedByMonsterNormalAttack(int damage, int beforeDamgeHp, int afterDamageHp)
    {
        DecraseHP(beforeDamgeHp, afterDamageHp);
    }

    void OnPlayerHpItemUsed(int beforeHp, int afterHealdHp)
    {
        IncraseHP((float)afterHealdHp / _stat.MaxHP);
        DoCounterHp(beforeHp, afterHealdHp);
    }
}
