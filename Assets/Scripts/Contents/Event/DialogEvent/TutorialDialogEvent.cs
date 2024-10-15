using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogEvent : DialogEventBoxCollider, ITalkable
{
    [SerializeField] private DialogText _dialogText;
    [SerializeField] private TutorialSequence _currTutorialEvent;

    private bool _isDialogStart = false;
    void Start()
    {
        Init();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isDialogStart && _boxCollider.enabled)
            Talk(_dialogText);
    }
    public void Talk(DialogText dText)
    {
        _isDialogStart = true;
        Managers.Dialog.DisplayNextParagraph(dText);
    }

    protected override void OnPlayerEnter()
    {
        if (_isDialogStart)
            return;
        if (!_isDialogStart)
            RegisterOnDialogEnd();
        Talk(_dialogText);
    }
    protected override void OnDialogEnd()
    {
        UnregisterOnDialogEnd();
        Debug.Assert(_currTutorialEvent != null);
        _currTutorialEvent.OnDialogEnd();
        _boxCollider.enabled = false;
    }
}