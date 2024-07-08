using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterMelleAttack : MonoBehaviour
{
    static public UnityAction<int, BaseMonsterController> OnPlayerHittedByMonsterMelleAttackEventHandelr;
    private BaseMonsterController _controller;
    protected MonsterStat _stat;
    private void Awake()
    {
        _stat = transform.parent.GetComponent<MonsterStat>();
        _controller = transform.parent.GetComponent<BaseMonsterController>();
        Debug.Assert(_controller != null);
        Debug.Assert(_stat != null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            // TODO : �� �̰����� TimeManager NullPointException�� �ߴ��� Ȯ���ؾ� �Ѵ�.
            OnPlayerHittedByMonsterMelleAttackEventHandelr?.Invoke(_stat.Attack, _controller);
        }
    }
}
