using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;

public class SisterController : QuestNPC, ITalkable
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
        if (_isConversationStart)
        {
            Managers.Dialog.OnConversationEndHandler -= OnNPCDialogEnd;
            Debug.Log($"{gameObject.name} : OnNPCDialogEnd Called!!");
            _isConversationEnd = true;
            _animator.Play("Teleport");
        }
    }
    public void OnTeleportAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }
}
