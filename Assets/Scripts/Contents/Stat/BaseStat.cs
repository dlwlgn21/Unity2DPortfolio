using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStat : MonoBehaviour
{
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
    public int HP { get { return _hp; } set { _hp = value; } }
    public int MaxHP { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defence { get { return _defence; } set { _defence = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    public abstract int DecreaseHpAndGetActualDamageAmount(int damage, out int beforeDamageHp, out int afterDamageHp);


}
