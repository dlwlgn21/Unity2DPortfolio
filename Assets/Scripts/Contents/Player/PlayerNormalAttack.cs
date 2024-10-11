using define;
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
        int totalDamage = _pc.Stat.Attack + _pc.Stat.SwordPlusDamage;
        if (collision.CompareTag("Monster"))
        {
            BaseMonsterController mc = collision.gameObject.GetComponent<BaseMonsterController>();

            if (mc == null)
            {
                Debug.Assert(false);
                return;
            }
            // TODO : 이거 하드코딩된 매직넘버가 겁나 맘에 안들지만 일단 빠른 개발을 위해 내비둔다...
            _pc.Stat.Mana += + 2;
            switch (EAttackType)
            {
                case EPlayerNoramlAttackType.ATTACK_1:
                    if (mc.ELookDir == _pc.ELookDir)
                    {
                        mc.OnHittedByPlayerNormalAttack(_pc.ELookDir, totalDamage * PlayerController.BACK_ATTACK_DAMAGE_COEFF, EPlayerNoramlAttackType.BACK_ATTACK);
                    }
                    else
                    {
                        mc.OnHittedByPlayerNormalAttack(_pc.ELookDir, totalDamage, EPlayerNoramlAttackType.ATTACK_1);
                    }
                    break;
                case EPlayerNoramlAttackType.ATTACK_2:
                    mc.OnHittedByPlayerNormalAttack(_pc.ELookDir, (int)(totalDamage * PlayerController.NORMAL_ATTACK_2_DAMAGE_COEFF), EPlayerNoramlAttackType.ATTACK_2);
                    break;
                case EPlayerNoramlAttackType.ATTACK_3:
                    mc.OnHittedByPlayerNormalAttack(_pc.ELookDir, totalDamage * PlayerController.NORMAL_ATTACK_3_DAMAGE_COEFF, EPlayerNoramlAttackType.ATTACK_3);
                    break;
            }
        }
    }



}
