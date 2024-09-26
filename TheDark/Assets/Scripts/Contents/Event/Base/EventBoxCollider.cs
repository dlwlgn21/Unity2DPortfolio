using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBoxCollider : MonoBehaviour
{
    protected BoxCollider2D _boxCollider;
    protected bool _isFirstPlayerEnterFlag = false;

    protected virtual void Init()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            OnPlayerEnter();
        }
    }
    protected abstract void OnPlayerEnter();
}
