using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNormalAttack : MonoBehaviour
{
    [SerializeField] private EPlayerNoramlAttackType _eAttackType;
    PlayerController _pc;
    public EPlayerNoramlAttackType EAttackType { get; private set; }

    private void Awake()
    {
        EAttackType = _eAttackType;
        _pc = transform.parent.gameObject.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<BaseMonsterController>()?.OnHittedByPlayerNormalAttack(_pc, EAttackType);
        }
    }


}
