using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldSpaceEffectController : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    protected Vector2 _fixedWorldPos;

    protected void AssignComponents()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position = _fixedWorldPos;
    }
}
