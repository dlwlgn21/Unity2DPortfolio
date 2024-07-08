using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterDamageProjectile : BaseMonsterProjectile
{
    static public UnityAction<ECharacterLookDir, int> DamageProjectileHitEventHandler;
    public int Damage { get; set; }
    private void Awake()
    {
        AssignComponents();
        SetProjectileType(EMonsterProjectileType.DAMAGE);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Assert(Damage != 0);
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            PlayAnimation(EMonsterProjectileState.HIT);
            DamageProjectileHitEventHandler?.Invoke(_eLaunchDir, Damage);
            _rb.velocity = Vector2.zero;
            _isHit = true;
        }
    }

}
