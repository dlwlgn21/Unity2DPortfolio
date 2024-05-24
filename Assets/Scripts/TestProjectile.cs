using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{
    public const float LIFE_TIME = 5f;
    [SerializeField] float _speed;
    [SerializeField] float _bombRange;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float _lifeTimer = LIFE_TIME;
    private bool _isValidCollided = false;
    private const int MONSTER_LAYER_MASK = 1 << ((int)define.EColliderLayer.MONSTERS);
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Launch(define.ECharacterLookDir eLookDir)
    {
        if (eLookDir == define.ECharacterLookDir.LEFT)
        {
            _rb.velocity = Vector2.left * _speed;
            _spriteRenderer.flipX = true;
        }
        else
        {
            _rb.velocity = Vector2.right * _speed;
        }
    }

    public void Init(Vector2 pos)
    {
        transform.position = pos;
        _lifeTimer = LIFE_TIME;
        _isValidCollided = false;
        _spriteRenderer.flipX = false;
        _rb.gravityScale = 0f;
    }
    private void Update()
    {
        if (_isValidCollided)
        {
            return;
        }

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer < 0f)
        {
            ReturnToPool();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.MONSTERS)
        {
            ProcessValidCollision();
        }
        else if (collision.gameObject.layer == (int)define.EColliderLayer.PLATFORM)
        {
            ProcessValidCollision();
        }
    }

    public void OnValidAnimBombTiming()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(transform.position, _bombRange, MONSTER_LAYER_MASK);
        if (monsters == null)
            return;

        foreach (Collider2D mon in monsters)
        {
            BaseMonsterController controller = mon.gameObject.GetComponent<BaseMonsterController>();
            Debug.Assert(controller != null);
            controller.HittedByPlayerKnockbackBomb();
        }
    }

    public void OnBombAnimEnded()
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        Managers.SkillPool.Return(gameObject);
    }
    private void ProcessValidCollision()
    {
        if (!_isValidCollided)
        {
            _isValidCollided = true;
            _animator.Play("Bomb");
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 1f;
        }
    }

    void OnDrawGizmosSelected() { Gizmos.DrawWireSphere(transform.position, _bombRange); }
}
