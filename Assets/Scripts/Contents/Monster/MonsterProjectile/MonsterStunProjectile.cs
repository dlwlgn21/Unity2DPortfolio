using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStunProjectile : BaseMonsterProjectile
{
    private void Awake()
    {
        AssignComponents();
        SetProjectileType(EMonsterProjectileType.STUN);
    }
}
