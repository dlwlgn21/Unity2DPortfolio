using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_PlayerHpBar : UI_HealthBar
{
    private void Start()
    {
        SetFullHpBarAndText();
    }
    public override void Init()
    {
        _stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        Debug.Assert(_stat != null);
        //SetFullHpBarRatio();
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
        PlayerController.HitUIEventHandler += OnPlayerHittedByMonsterNormalAttack;
        PlayerStat.PlayerHpIncreaseEventHandler -= OnPlayerHpIncreased;
        PlayerStat.PlayerHpIncreaseEventHandler += OnPlayerHpIncreased;
    }

    private void OnDestroy()
    {
        PlayerController.HitUIEventHandler -= OnPlayerHittedByMonsterNormalAttack;
        PlayerStat.PlayerHpIncreaseEventHandler -= OnPlayerHpIncreased;
    }
    void OnPlayerHittedByMonsterNormalAttack(int damage, int beforeDamgeHp, int afterDamageHp)
    {
        DecraseHP(beforeDamgeHp, afterDamageHp);
    }


    void OnPlayerHpIncreased(int currHp, int increasedHp)
    {
        IncraseHPRatio((float)increasedHp / _stat.MaxHP);
        _currHpText.DOCounter(currHp, increasedHp, _fillSpeed);
    }
}
