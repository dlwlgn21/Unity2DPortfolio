using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogEvent : DialogEventBoxCollider, ITalkable
{
    [SerializeField] private DialogText _dialogText;
    [SerializeField] private TutorialEvent _nextEvent;
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
        if (!_isDialogStart)
            RegisterOnDialogEnd();
        Talk(_dialogText);
    }
    protected override void OnDialogEnd()
    {
        _isDialogStart = false;
        UnregisterOnDialogEnd();
        Debug.Assert(_nextEvent != null);
        _nextEvent.OnDialogEnd();
        gameObject.SetActive(false);
    }
}