using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum EMonsterProjectileType
{
    DAMAGE,
    KNOCKBACK,
    STUN,
    COUNT
}


public abstract class BaseMonsterProjectile : MonoBehaviour
{
    protected enum EMonsterProjectileState
    {
        MUZZLE,
        PROJECTILE,
        HIT
    }

    protected float _proectileSpeed = 3f;
    public EMonsterProjectileType EProjectileType { get; private set; } = EMonsterProjectileType.COUNT;
    protected SpriteRenderer _sr;
    protected Animator _animator;
    protected Rigidbody2D _rb;
    protected ECharacterLookDir _eLaunchDir;
    protected const float LIFE_TIME_IN_SEC = 3f;
    
    protected string _muzzleAnimKey = null;
    protected string _ProjectileAnimKey = null;
    protected string _HitAnimKey = null;
    protected bool _isHit = false;

    public void Init()
    {
        gameObject.SetActive(true);
        _rb.velocity = Vector2.zero;
    }

    public void OnValidShootAnimTiming(ECharacterLookDir eLookDir, Vector2 shootPos)
    {
        Debug.Assert(EProjectileType != EMonsterProjectileType.COUNT && _muzzleAnimKey != null);
        transform.position = shootPos;
        _eLaunchDir = eLookDir;
        if (_eLaunchDir == ECharacterLookDir.LEFT)
        {
            _sr.flipX = true;
        }
        else
        {
            _sr.flipX = false;
        }
        PlayAnimation(EMonsterProjectileState.MUZZLE);
        StartCoroutine(StartCountLifeTimeCo());
    }



    public void OnMuzzleAnimFullyPlayed()
    {
        PlayAnimation(EMonsterProjectileState.PROJECTILE);
        if (_eLaunchDir == ECharacterLookDir.LEFT)
        {
            _rb.velocity = new Vector2(-_proectileSpeed, 0f);
        }
        else
        {
            _rb.velocity = new Vector2(_proectileSpeed, 0f);
        }
    }

    public void OnHitAnimFullyPlayed()
    {
        Managers.ProjectilePool.ReturnMonsterProjectile(gameObject, EProjectileType);
    }

    private IEnumerator StartCountLifeTimeCo()
    {
        yield return new WaitForSeconds(LIFE_TIME_IN_SEC);
        if (!_isHit)
        {
            PlayAnimation(EMonsterProjectileState.HIT);
        }
    }

    protected void PlayAnimation(EMonsterProjectileState eState)
    {
        switch (eState)
        {
            case EMonsterProjectileState.MUZZLE:
                _animator.Play(_muzzleAnimKey, -1, 0f);
                break;
            case EMonsterProjectileState.PROJECTILE:
                _animator.Play(_ProjectileAnimKey, -1, 0f);
                break;
            case EMonsterProjectileState.HIT:
                _animator.Play(_HitAnimKey, -1, 0f);
                break;
        }
    }

    protected void AssignComponents()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
    protected void SetProjectileType(EMonsterProjectileType eType)
    {
        EProjectileType = eType;
        switch (EProjectileType)
        {
            case EMonsterProjectileType.DAMAGE:
                _muzzleAnimKey = "DamageMuzzle";
                _ProjectileAnimKey = "DamageProjectile";
                _HitAnimKey = "DamageHit";
                _proectileSpeed = 3f;
                break;
            case EMonsterProjectileType.KNOCKBACK:
                _muzzleAnimKey = "KnockbackMuzzle";
                _ProjectileAnimKey = "KnockbackProjectile";
                _HitAnimKey = "KnockbackHit";
                break;
            case EMonsterProjectileType.STUN:
                _muzzleAnimKey = "StunMuzzle";
                _ProjectileAnimKey = "StunProjectile";
                _HitAnimKey = "StunHit";
                break;
        }
    }
}
