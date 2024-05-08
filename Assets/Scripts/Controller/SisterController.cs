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
        Managers.Dialog.DisplayNextParagraph(dText);
    }
}
