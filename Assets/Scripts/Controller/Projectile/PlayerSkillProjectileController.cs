using define;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public sealed class PlayerSkillProjectileController : BaseProjectileController
{
    [SerializeField] float _speed;
    [SerializeField] float _bombRange;

    LightController _lightController;
    const int MONSTER_LAYER_MASK = 1 << ((int)define.EColliderLayer.MonsterBody);

    const string MUZZLE_ANIM_KEY = "Muzzle";
    const string PROJECTILE_ANIM_KEY = "Projectile";
    const string HIT_ANIM_KEY = "Hit";
    const string BOOM_ANIM_KEY = "Bomb";
    private void Awake()
    {
        AssginCommonComponents();
        _lightController = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "Light");
        _lightController.TurnOffGraduallyLightTimeInSec = 0.3f;
        //_lightController.gameObject.SetActive(false);
    }

    public void Launch(define.ECharacterLookDir eLookDir)
    {
        _eLaunchDir = eLookDir;
        _animator.Play(MUZZLE_ANIM_KEY, -1, 0f);
        StartCoroutine(this.StartCountLifeTimeCo());
    }

    public void Init(Vector2 pos)
    {
        transform.position = pos;
        _sr.flipX = false;
        _rb.gravityScale = 0f;
        _isHit = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.MonsterBody)
        {
            _animator.Play(HIT_ANIM_KEY, -1, 0f);
            _isHit = true;
        }
    }

    void OnDropedGroundSoundTiming()
    {
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_SKILL_KNOCKBACK_PROJECTILE_DROPED);
    }
    private void OnValidAnimBombTiming()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, _bombRange, MONSTER_LAYER_MASK);
        if (monsters == null)
        {
            return;
        }

        foreach (Collider2D mon in monsters)
        {
            mon.gameObject.GetComponent<BaseMonsterController>()?.OnHittedByPlayerSkill(EActiveSkillType.Spawn_Shooter);
        }
        _lightController.TurnOffLightGradually();
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_MONSTER_DIE_EXPOLOSION_2);
    }

    private void OnBombAnimFullyPlayed()
    {
        _lightController.ForceToStopCoroutineAndTurnOffLight();
        ReturnToPool();
    }

    #region OVERRIDE_ABSTRACT
    private void OnMuzzleAnimFullyPlayed()
    {
        _animator.Play(PROJECTILE_ANIM_KEY, -1, 0f);
        if (_eLaunchDir == define.ECharacterLookDir.Left)
        {
            _rb.velocity = Vector2.left * _speed;
            _sr.flipX = true;
        }
        else
        {
            _rb.velocity = Vector2.right * _speed;
        }
    }
    private void OnHitAnimFullyPlayed()
    {
        if (_isHit)
        {
            _lightController.TurnOnLight();
            _animator.Play(BOOM_ANIM_KEY, -1, 0f);
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 1f;
        }
        else
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        Managers.ProjectilePool.ReturnPlayerKnockbackBoom(gameObject);
    }

    #endregion

    private IEnumerator StartCountLifeTimeCo()
    {
        yield return new WaitForSeconds(LIFE_TIME_IN_SEC);
        if (!_isHit)
        {
            _animator.Play(HIT_ANIM_KEY, -1, 0f);
        }
    }

    void OnDrawGizmosSelected() { Gizmos.DrawWireSphere(transform.position, _bombRange); }
}
