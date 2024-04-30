using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SisterController : NPC, ITalkable
{
    [SerializeField] private DialogText _dialogText;
    [SerializeField] private UIDialogController _dialogController;
    public override void Interact()
    {
        Debug.Assert(_dialogController != null);
        Talk(_dialogText);
    }

    public void Talk(DialogText dText)
    {
        _dialogController.DisplayNextParagraph(dText);
    }
}
