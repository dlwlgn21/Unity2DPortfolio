using define;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public enum EMonsterProjectileType
{
    KNOCKBACK,
    SLOW,
    COUNT
}

enum EProjectileState
{
    Muzzle,
    Projectile,
    Hit
}
public class MonsterProjectileController : BaseProjectileController
{
    static public UnityAction<BaseMonsterController> MonsterProjectileHitPlayerEventHandelr;
    public EMonsterProjectileType EProjectileType { get; private set; } = EMonsterProjectileType.COUNT;

    private float _proectileSpeed = 3f;
    private string _muzzleAnimKey = null;
    private string _ProjectileAnimKey = null;
    private string _HitAnimKey = null;
    private BaseMonsterController _currOwnerController;

    public void Awake()
    {
        AssginCommonComponents();
    }

    public void Init()
    {
        gameObject.SetActive(true);
        _rb.velocity = Vector2.zero;
        _isHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null && !pc.IsValidStateToChangeHitState())
            {
                return;
            }
            Debug.Assert(_currOwnerController != null);
            PlayAnimation(EProjectileState.Hit);
            if (MonsterProjectileHitPlayerEventHandelr != null)
                MonsterProjectileHitPlayerEventHandelr.Invoke(_currOwnerController);
            _rb.velocity = Vector2.zero;
            _isHit = true;
        }
    }

    public void OnValidShootAnimTiming(ECharacterLookDir eLookDir, Vector2 shootPos, BaseMonsterController mc)
    {
        // TODO : 나중에 몬스터 더 추가되면 Blind, Burn, Parallysis Projectile을 만들 수도 있을 것 같아서 switch문 이렇게 남겨둠.
        switch (mc.Stat.EStatusEffectType)
        {
            case EAttackStatusEffect.None:
                break;
            case EAttackStatusEffect.Knockback:
                SetProjectileTypeAndAnimKeys(EMonsterProjectileType.KNOCKBACK);
                break;
            case EAttackStatusEffect.Blind:
                break;
            case EAttackStatusEffect.Burn:
                break;
            case EAttackStatusEffect.Slow:
                SetProjectileTypeAndAnimKeys(EMonsterProjectileType.SLOW);
                break;
            case EAttackStatusEffect.Parallysis:
                break;
            default:
                break;
        }
        Debug.Assert(EProjectileType != EMonsterProjectileType.COUNT && _muzzleAnimKey != null);
        transform.position = shootPos;
        _eLaunchDir = eLookDir;
        if (_eLaunchDir == ECharacterLookDir.Left)
        {
            _sr.flipX = true;
        }
        else
        {
            _sr.flipX = false;
        }
        PlayAnimation(EProjectileState.Muzzle);
        StartCoroutine(StartCountdownLifeTimeCo());
        _currOwnerController = mc;
    }

    #region OVERRIDE_ABSTRACT
    private void OnMuzzleAnimFullyPlayed()
    {
        PlayAnimation(EProjectileState.Projectile);
        if (_eLaunchDir == ECharacterLookDir.Left)
        {
            _rb.velocity = new Vector2(-_proectileSpeed, 0f);
        }
        else
        {
            _rb.velocity = new Vector2(_proectileSpeed, 0f);
        }
    }

    private void OnHitAnimFullyPlayed()
    {
        ReturnToPool();
    }
    private void ReturnToPool()
    {
        Managers.ProjectilePool.ReturnMonsterProjectile(gameObject, EProjectileType);
    }
    #endregion
    private IEnumerator StartCountdownLifeTimeCo()
    {
        yield return new WaitForSeconds(LIFE_TIME_IN_SEC);
        if (!_isHit)
        {
            PlayAnimation(EProjectileState.Hit);
        }
    }
    private void PlayAnimation(EProjectileState eState)
    {
        switch (eState)
        {
            case EProjectileState.Muzzle:
                _animator.Play(_muzzleAnimKey, -1, 0f);
                break;
            case EProjectileState.Projectile:
                _animator.Play(_ProjectileAnimKey, -1, 0f);
                break;
            case EProjectileState.Hit:
                _animator.Play(_HitAnimKey, -1, 0f);
                break;
        }
    }

    private void SetProjectileTypeAndAnimKeys(EMonsterProjectileType eType)
    {
        EProjectileType = eType;
        switch (EProjectileType)
        {
            case EMonsterProjectileType.KNOCKBACK:
                _muzzleAnimKey = "KnockbackMuzzle";
                _ProjectileAnimKey = "KnockbackProjectile";
                _HitAnimKey = "KnockbackHit";
                break;
            case EMonsterProjectileType.SLOW:
                _muzzleAnimKey = "StunMuzzle";
                _ProjectileAnimKey = "StunProjectile";
                _HitAnimKey = "StunHit";
                break;
        }
    }
}
