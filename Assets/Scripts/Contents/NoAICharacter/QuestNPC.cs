using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class QuestNPC : NPC, IInteractable
{
    protected bool _isConversationStart = false;
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        if (_isConversationEnd)
        {
            return;
        }
        if (_isConversationStart)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                Interact();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _isConversationStart = true;
            Interact();
            Managers.Sound.Play(DataManager.SFX_UI_DIALOG_BOX_POPUP);
        }
    }
}
