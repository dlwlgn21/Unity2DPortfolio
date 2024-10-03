using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerStat : BaseStat
{
    [SerializeField] int _level;
    [SerializeField] int _exp;
    [SerializeField] int _gold;

    // 24.10.3 Item 착용위해 추가.
    [SerializeField] int _swordPlusDamage;
    [SerializeField] int _helmetPlusDefence;
    [SerializeField] int _armorPlusDefence;

    public int Level { get { return _level; } set { _level = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    public int Gold { get { return _gold; } set { _gold = value; } }

    // 24.10.3 Item 착용위해 추가.
    public int SwordPlusDamage { get { return _swordPlusDamage; } set { _swordPlusDamage = value; } }
    public int HelmetPlusDefence { get { return _helmetPlusDefence; } set { _helmetPlusDefence = value; } }
    public int ArmorPlusDefence { get { return _armorPlusDefence; } set { _armorPlusDefence = value; } }

    public override int DecreaseHpAndGetActualDamageAmount(int damage, out int beforeDamageHp, out int afterDamageHp)
    {
        if (HP <= 0)
        {
            beforeDamageHp = 0;
            afterDamageHp = 0;
            return 0;
        }
        int actualDamage = Mathf.Max(1, damage - (Defence + HelmetPlusDefence + ArmorPlusDefence));
        beforeDamageHp = HP;
        HP -= actualDamage;
        afterDamageHp = HP;
        if (HP <= 0)
        {
            HP = 0;
        }
        return actualDamage;
    }

    void Awake()
    {
        var dict = Managers.Data.PlayerStatDict;
        Level = dict[1].level;
        HP = dict[1].maxHp;
        MaxHP = dict[1].maxHp;
        Attack = dict[1].attack;
        Defence = dict[1].defence;
        MoveSpeed = dict[1].moveSpeed;
        Exp = 0;
        Gold = 0;
        SwordPlusDamage = 0;
        HelmetPlusDefence = 0;
        ArmorPlusDefence = 0;
    }
}
