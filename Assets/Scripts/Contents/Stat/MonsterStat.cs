using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using define;
using UnityEngine.Events;

public class MonsterStat : BaseStat
{
    public const int TUTIRIAL_HP = 50;
    [SerializeField]
    protected int _monsterType;

    [SerializeField]
    protected int _exp;
    public int MonsterType { get { return _monsterType; } set { _monsterType = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    
    public EAttackStatusEffect  EStatusEffectType { get; private set; }
    public Vector2 KnockbackForce { get; set; } = new Vector2(1.5f, 1.5f);
    public float SlowTimeInSec { get; set; }

    public override int DecreaseHpAndGetActualDamageAmount(int damage, out int beforeDamageHp, out int afterDamageHp)
    {
        //if (HP <= 0)
        //{
        //    beforeDamageHp = 0;
        //    afterDamageHp = 0;
        //    return 0;
        //}
        // TutorialScene에서의 0 데미지 처리를 위해서 추가함.
        if (damage == 0)
        {
            beforeDamageHp = HP;
            afterDamageHp = HP;
            return 0;
        }
        int actualDamage = Mathf.Max(1, damage - Defence);
        beforeDamageHp = HP;
        HP -= actualDamage;
        afterDamageHp = HP;
        if (HP <= 0)
        {
            HP = 0;
        }
        return actualDamage;
    }

    public void InitBasicStat(define.EMonsterNames eMonster)
    {
        var dict = Managers.Data.MonsterStatDict;
        MonsterType = dict[(int)eMonster].monsterType;
        EStatusEffectType = (EAttackStatusEffect)dict[(int)eMonster].monsterStatusEffect;
        HP = dict[(int)eMonster].maxHp;
        MaxHP = dict[(int)eMonster].maxHp;
        Attack = dict[(int)eMonster].attack;
        Defence = dict[(int)eMonster].defence;
        MoveSpeed = dict[(int)eMonster].moveSpeed;
        Exp = dict[(int)eMonster].exp;
    }
    public void InitStatForTutorial()
    {
        HP = TUTIRIAL_HP;
        MaxHP = TUTIRIAL_HP;
        Attack = 0;
        Exp = 0;
    }
}
