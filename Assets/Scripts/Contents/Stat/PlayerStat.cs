using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : BaseStat
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;
    public int Level { get { return _level; } set { _level = value; } }
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
