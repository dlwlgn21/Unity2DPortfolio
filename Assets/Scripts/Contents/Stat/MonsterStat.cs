using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterStat : BaseStat
{
    [SerializeField]
    protected int _monsterType;

    [SerializeField]
    protected int _exp;

    public int MonsterType { get { return _monsterType; } set { _monsterType = value; } }

    public int Exp { get { return _exp; } set { _exp = value; } }

    public void OnHitted(int damage)
    {
        if (HP <= 0)
            return;

        int actualDamage = Mathf.Max(1, Mathf.Abs(damage - Defence));
        HP -= actualDamage;
        if (HP <= 0)
        {
            HP = 0;
        }
    }

    public void Init(define.EMonsterNames eMonster)
    {
        var dict = Managers.Data.MonsterStatDict;
        MonsterType = dict[(int)eMonster].monsterType;
        HP = dict[(int)eMonster].maxHp;
        MaxHP = dict[(int)eMonster].maxHp;
        Attack = dict[(int)eMonster].attack;
        Defence = dict[(int)eMonster].defence;
        MoveSpeed = dict[(int)eMonster].moveSpeed;
        Exp = dict[(int)eMonster].exp;
    }
}
