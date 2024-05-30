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
        if (_isConversationStart && gameObject.name == "StartSister")
        {
            Managers.Dialog.OnConversationEndHandler -= OnNPCDialogEnd;
            Debug.Log($"{gameObject.name} : OnNPCDialogEnd Called!!");
            _isConversationEnd = true;
            _animator.Play("Teleport");
            return;
        }
        if (_isConversationStart && gameObject.name == "EndSister")
        {
            Managers.Dialog.OnConversationEndHandler -= OnNPCDialogEnd;
            _isConversationEnd = true;
            _animator.Play("Teleport");
        }
    }
    public void OnTeleportAnimFullyPlayed()
    {
        if (gameObject.name == "StartSister")
        {
            gameObject.SetActive(false);
        }
        else if (gameObject.name == "EndSister")
        {
            Managers.Scene.LoadScene(define.ESceneType.MAIN_PLAY);
        }
    }
}
