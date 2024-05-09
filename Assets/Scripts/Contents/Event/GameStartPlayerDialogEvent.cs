using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartPlayerDialogEvent : DialogEventBoxCollider, ITalkable
{
    [SerializeField] private DialogText _dialogText;
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
        // TODO : Destroy 할 지, 그냥 SetActive(false) 할 지 정하자
        _isDialogStart = false;
        _boxCollider.enabled = false;
        Managers.UIKeyTutorial.ActiveMoveKeys();
        UnregisterOnDialogEnd();
        StartCoroutine(SetActiveFalseAfter10Seconds());
    }

    IEnumerator SetActiveFalseAfter10Seconds()
    {
        yield return new WaitForSeconds(10f);
        Managers.UIKeyTutorial.UnactiveMoveKeys();
        gameObject.SetActive(false);
    }
}
