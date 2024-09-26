using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogNPC : NPC
{
    protected SpriteRenderer _interactKeySprite;

    protected override void Start()
    {
        base.Start();
        _interactKeySprite = Utill.GetComponentInChildrenOrNull<SpriteRenderer>(gameObject, "DownArrow");
    }

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
}
