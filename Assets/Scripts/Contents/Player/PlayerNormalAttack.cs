using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerNormalAttack : MonoBehaviour
{
    static public UnityAction<PlayerController, EPlayerNoramlAttackType, BaseMonsterController> PlayerNormalAttackEventHandler;
    [SerializeField] private EPlayerNoramlAttackType _eAttackType;
    PlayerController _pc;
    public EPlayerNoramlAttackType EAttackType { get; private set; }

    ~PlayerNormalAttack()
    {
        PlayerNormalAttackEventHandler = null;
    }

    private void Awake()
    {
        EAttackType = _eAttackType;
        _pc = transform.parent.gameObject.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            // TODO : 이렇게 하는게 맞나 싶기도 하다. 나중에 누구한테든지 물어보장.
            Debug.Assert(PlayerNormalAttackEventHandler != null);
            PlayerNormalAttackEventHandler.Invoke(_pc, EAttackType, collision.gameObject.GetComponent<BaseMonsterController>());
        }
    }


}
