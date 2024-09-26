using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum EMonsterStatusEffect
{
    NONE,
    KNOCKBACK,
    BLIND,
    BURN,
    SLOW,
    PARALLYSIS,
}

public class MonsterStat : BaseStat
{
    public const int TUTIRIAL_HP = 50;

    [SerializeField]
    protected int _monsterType;

    [SerializeField]
    protected int _exp;
    public int MonsterType { get { return _monsterType; } set { _monsterType = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    
    public EMonsterStatusEffect  EStatusEffectType { get; private set; }
    public Vector2 KnockbackForce { get; set; } = new Vector2(1.5f, 1.5f);
    public float SlowTimeInSec { get; set; }
    public void InitBasicStat(define.EMonsterNames eMonster)
    {
        if (HP == 0)
        {
            var dict = Managers.Data.MonsterStatDict;
            MonsterType = dict[(int)eMonster].monsterType;
            EStatusEffectType = (EMonsterStatusEffect)dict[(int)eMonster].monsterStatusEffect;
            HP = dict[(int)eMonster].maxHp;
            MaxHP = dict[(int)eMonster].maxHp;
            Attack = dict[(int)eMonster].attack;
            Defence = dict[(int)eMonster].defence;
            MoveSpeed = dict[(int)eMonster].moveSpeed;
            Exp = dict[(int)eMonster].exp;
        }
    }
    public void SetHPForTutorialAndAttackToZero()
    {
        HP = TUTIRIAL_HP;
        MaxHP = TUTIRIAL_HP;
        Attack = 0;
    }
}
