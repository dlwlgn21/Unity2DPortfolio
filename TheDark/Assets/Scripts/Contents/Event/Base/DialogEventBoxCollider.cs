using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogEventBoxCollider : EventBoxCollider
{
    protected override void Init()
    {
        base.Init();
    }
    protected void RegisterOnDialogEnd() { Managers.Dialog.OnConversationEndHandler += OnDialogEnd; }
    protected void UnregisterOnDialogEnd() { Managers.Dialog.OnConversationEndHandler -= OnDialogEnd; }
    protected abstract void OnDialogEnd();
}
