using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteRenderer _interactKeySprite;
    protected Transform _playerTransform;
    protected const float INTERACT_DIST = 1.5f;
    protected const float SCALE_TWEEN_TIME = 0.3f;
    void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsWithinInteractDistance())
            Interact();

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
