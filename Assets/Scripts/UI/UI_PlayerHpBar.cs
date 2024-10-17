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
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
        PlayerController.HitUIEventHandler += OnPlayerHittedByMonsterNormalAttack;
        PlayerStat.OnPlayerHpIncreaseEventHandler -= OnPlayerHpIncreased;
        PlayerStat.OnPlayerHpIncreaseEventHandler += OnPlayerHpIncreased;
    }

    private void OnDestroy()
    {
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
        PlayerStat.OnPlayerHpIncreaseEventHandler -= OnPlayerHpIncreased;
    }
    void OnPlayerHittedByMonsterNormalAttack(int damage, int beforeDamgeHp, int afterDamageHp)
    {
        DecraseHP(beforeDamgeHp, afterDamageHp);
    }


    void OnPlayerHpIncreased(int currHp, int increasedHp)
    {
        IncraseHP((float)currHp / _stat.MaxHP);
        _currHpText.DOCounter(currHp, increasedHp, _fillSpeed);
    }
}
