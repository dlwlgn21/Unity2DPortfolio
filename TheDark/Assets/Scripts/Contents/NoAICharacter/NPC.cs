using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    protected Transform _playerTransform;
    protected Animator _animator;
    protected const float INTERACT_DIST = 1.5f;
    protected bool _isConversationEnd = false;

    protected virtual void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        Managers.Dialog.OnConversationEndHandler += OnNPCDialogEnd;
    }

    public abstract void Interact();
    public abstract void OnNPCDialogEnd();

    protected bool IsWithinInteractDistance()
    {
        if (Vector2.Distance(_playerTransform.position, transform.position) < INTERACT_DIST)
            return true;
        return false;
    }
}
