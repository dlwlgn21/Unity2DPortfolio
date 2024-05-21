using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestProjectile : MonoBehaviour
{
    [SerializeField] float _speed;
    private float _lifeTimer = 5f;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isCollideWithMonster = false;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = Vector2.right * _speed;
    }

    public void Init(Vector2 pos)
    {
        transform.position = pos;
        _rb.velocity = Vector2.right * _speed;
        _lifeTimer = 3f;
        _rb.gravityScale = 0f;
        _isCollideWithMonster = false;
    }
    private void Update()
    {
        if (_isCollideWithMonster)
            return;

        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer < 0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.MONSTERS)
        {
            Debug.Log("Hit with Monster");
            _isCollideWithMonster = true;
            _animator.Play("Bomb");
            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 1f;
        }
        else if (collision.gameObject.layer == (int)define.EColliderLayer.PLATFORM)
        {
            Debug.Log("Hit with Platform");
            _isCollideWithMonster = true;
            _rb.velocity = Vector2.zero;
            _animator.Play("Bomb");
            _rb.gravityScale = 1f;
        }
    }

    public void OnBombAnimEnded()
    {
        Debug.Log("CalledOnBombAnimEnded");
        Destroy(gameObject);
    }
}
