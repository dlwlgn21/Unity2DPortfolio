using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialEvent : MonoBehaviour
{
    protected bool _isTutorialStart = false;
    protected BaseMonsterController _mc;
    protected PlayerController _pc;
    protected int _playerOriginalAttackDamage;

    protected void Init()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPos = player.GetComponent<Transform>().position;
        _pc = player.GetComponent<PlayerController>();
        _mc = Managers.MonsterPool.Get(define.EMonsterNames.Warden, new Vector2(playerPos.x + 5f, playerPos.y + 1f)).GetComponent<BaseMonsterController>();
        _mc.gameObject.GetComponent<MonsterStat>().SetHPForTutorialAndAttackToZero();
    }
    public abstract void OnDialogEnd();

    protected void SetPlayerAttackToZero()
    {
        _playerOriginalAttackDamage = _pc.Stat.Attack;
        _pc.Stat.Attack = 0;
    }
    protected void RollBackPlayerAttack()
    {
        _pc.Stat.Attack = _playerOriginalAttackDamage;
    }
}