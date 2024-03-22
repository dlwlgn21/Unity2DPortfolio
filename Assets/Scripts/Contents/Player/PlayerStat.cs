using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
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
    [SerializeField]
    protected int _gold;
    public int Level { get { return _level; } set { _level = value; } }
    public int HP { get { return _hp; } set { _hp = value; } }
    public int MaxHP { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defence { get { return _defence; } set { _defence = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public int Exp { get { return _exp; } set { _exp = value; } }
    public int Gold { get { return _gold; } set { _gold = value; } }

    void Start()
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
    }
}
