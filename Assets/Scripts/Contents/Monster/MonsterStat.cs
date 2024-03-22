using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        var dict = Managers.Data.MonsterStatDict;
        MonsterType = dict[(int)define.EMonsterNames.CagedShoker].monsterType;
        HP = dict[(int)define.EMonsterNames.CagedShoker].maxHp;
        MaxHP = dict[(int)define.EMonsterNames.CagedShoker].maxHp;
        Attack = dict[(int)define.EMonsterNames.CagedShoker].attack;
        Defence = dict[(int)define.EMonsterNames.CagedShoker].defence;
        MoveSpeed = dict[(int)define.EMonsterNames.CagedShoker].moveSpeed;
        Exp = dict[(int)define.EMonsterNames.CagedShoker].exp;
    }
}
