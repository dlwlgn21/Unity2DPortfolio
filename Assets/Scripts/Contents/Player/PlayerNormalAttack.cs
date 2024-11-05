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
                Debug.DebugBreak();
                return;
            }
            // TODO : 이거 하드코딩된 매직넘버가 겁나 맘에 안들지만 일단 빠른 개발을 위해 내비둔다...
            _pc.Stat.Mana += + 2;
            switch (EAttackType)
            {
                case EPlayerNoramlAttackType.Attack_1:
                    if (mc.ELookDir == _pc.ELookDir)
                    {
                        mc.DamagedFromPlayer(_pc.ELookDir, totalDamage * PlayerController.sBackAttackDamageCoeff, EPlayerNoramlAttackType.BackAttack);
                        //Managers.Sound.Play(DataManager.SFX_PLAYER_BACKATTACK_PATH);
                    }
                    else
                        mc.DamagedFromPlayer(_pc.ELookDir, totalDamage, EPlayerNoramlAttackType.Attack_1);
                    break;
                case EPlayerNoramlAttackType.Attack_2:
                    mc.DamagedFromPlayer(_pc.ELookDir, (int)(totalDamage * PlayerController.sNormalAttack2DamageCoeff), EPlayerNoramlAttackType.Attack_2);
                    break;
                case EPlayerNoramlAttackType.Attack_3:
                    mc.DamagedFromPlayer(_pc.ELookDir, totalDamage * PlayerController.sNormalAttack3DamageCoeff, EPlayerNoramlAttackType.Attack_3);
                    break;
            }
        }
    }
}