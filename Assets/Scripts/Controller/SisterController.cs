using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SisterController : NPC, ITalkable
{
    [SerializeField] private DialogText _dialogText;
    public override void Interact()
    {
        Talk(_dialogText);
    }

    public void Talk(DialogText dText)
    {
        Debug.Log("Talk!!!");
        Managers.Dialog.DisplayNextParagraph(dText);
    }

    public override void OnNPCDialogEnd()
    {
        if (IsWithinInteractDistance())
        {
            Managers.Dialog.OnConversationEndHandler -= OnNPCDialogEnd;
            _isConversationEnd = true;
            Debug.Log($"{gameObject.name} : OnNPCDialogEnd Called!!");
            _animator.Play("Teleport");
        }
    }

    public void OnTeleportAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }
}
