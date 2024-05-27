using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    protected SpriteRenderer _interactKeySprite;
    protected Animator _animator;
    protected Transform _playerTransform;
    protected const float INTERACT_DIST = 1.5f;
    protected const float SCALE_TWEEN_TIME = 0.3f;
    protected bool _isConversationEnd = false;
    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Managers.Dialog.OnConversationEndHandler += OnNPCDialogEnd;
        _interactKeySprite = Utill.GetComponentInChildrenOrNull<SpriteRenderer>(gameObject, "DownArrow");
        _animator = GetComponent<Animator>();
    }
    public abstract void OnNPCDialogEnd();

    void Update()
    {
        if (_isConversationEnd)
        {
            if (_interactKeySprite.gameObject.activeSelf)
            {
                _interactKeySprite.gameObject.SetActive(false);
            }
            return;
        }

        if (Input.GetKeyUp(KeyCode.E) && IsWithinInteractDistance())
        {
            Debug.Log($"{gameObject.name} : Interact Called!!");
            Interact();
        }

        if (_interactKeySprite.gameObject.activeSelf && !IsWithinInteractDistance())
        {
            // Turn off sprite
            _interactKeySprite.gameObject.SetActive(false);
        }
        else if (!_interactKeySprite.gameObject.activeSelf && IsWithinInteractDistance())
        {
            // Turn on sprite
            _interactKeySprite.gameObject.SetActive(true);
        }
    }
    public abstract void Interact();
    protected bool IsWithinInteractDistance()
    {
        if (Vector2.Distance(_playerTransform.position, transform.position) < INTERACT_DIST)
            return true;
        return false;
    }
}
