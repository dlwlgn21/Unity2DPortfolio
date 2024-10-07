using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterMelleAttack : MonoBehaviour
{
    static public UnityAction<BaseMonsterController> OnPlayerHittedByMonsterMelleAttackEventHandelr;
    private BaseMonsterController _mc;
    protected MonsterStat _stat;
    private void Awake()
    {
        _mc = transform.parent.GetComponent<BaseMonsterController>();
        Debug.Assert(_mc != null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            // TODO : �� �̰����� TimeManager NullPointException�� �ߴ��� Ȯ���ؾ� �Ѵ�.
            OnPlayerHittedByMonsterMelleAttackEventHandelr?.Invoke(_mc);
        }
    }
}
