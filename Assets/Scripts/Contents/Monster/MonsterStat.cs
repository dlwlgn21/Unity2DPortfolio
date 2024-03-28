using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    [SerializeField]
    protected int _monsterType;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defence;
    [SerializeField]
    protected float _moveSpeed;
    [SerializeField]
    protected int _exp;

    public int MonsterType { get { return _monsterType; } set { _monsterType = value; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int MaxHP { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defence { get { return _defence; } set { _defence = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
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
