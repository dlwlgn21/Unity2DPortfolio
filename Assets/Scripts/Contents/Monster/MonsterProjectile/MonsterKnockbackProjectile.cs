using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterKnockbackProjectile : BaseMonsterProjectile
{
    static public UnityAction<ECharacterLookDir, Vector2> KnockbackProjectileHitEventHandler;
    public Vector2 KnockbackForce { get; set; }

    private void Awake()
    {
        AssignComponents();
        SetProjectileType(EMonsterProjectileType.KNOCKBACK);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            PlayAnimation(EMonsterProjectileState.HIT);
            KnockbackProjectileHitEventHandler?.Invoke(_eLaunchDir, KnockbackForce);
            _rb.velocity = Vector2.zero;
            _isHit = true;
        }
    }
}
