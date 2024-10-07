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
            // TODO : 왜 이곳에서 TimeManager NullPointException이 뜨는지 확인해야 한다.
            OnPlayerHittedByMonsterMelleAttackEventHandelr?.Invoke(_mc);
        }
    }
}
