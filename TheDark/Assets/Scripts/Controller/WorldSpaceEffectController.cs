using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class WorldSpaceEffectController : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    protected Vector2 _fixedWorldPos;
    protected Vector3 _originalLocalScale;

    protected void Init()
    {
        AssignComponents();
        SetComponentsEnabled(false);
        _originalLocalScale = transform.localScale;
    }
    protected void AssignComponents()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void FixSpriteDirection()
    {
        if (transform.parent.localRotation.eulerAngles.y > 0f)
        {
            transform.localScale = new Vector3(-1f, _originalLocalScale.y, _originalLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1f, _originalLocalScale.y, _originalLocalScale.z);
        }
    }

    protected void FixPosition()
    {
        transform.position = _fixedWorldPos;
    }

    protected void SetFixedFos(Vector2 pos)
    {
        _fixedWorldPos = pos;
    }
    protected void SetComponentsEnabled(bool isEnable)
    {
        _animator.enabled = isEnable;
        _spriteRenderer.enabled = isEnable;
    }
}
