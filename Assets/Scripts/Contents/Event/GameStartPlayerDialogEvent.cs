using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartPlayerDialogEvent : DialogEventBoxCollider, ITalkable
{
    [SerializeField] DialogText _dialogText;
    TutorialManager _tutorialManager;
    bool _isDialogStart = false;
    void Start()
    {
        Init();
        _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        Managers.Input.KeyboardHandler -= OnEKeyDown;
        Managers.Input.KeyboardHandler += OnEKeyDown;
    }
    
    void OnEKeyDown()
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
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_UI_DIALOG_BOX_POPUP);
        Talk(_dialogText);
    }

    protected override void OnDialogEnd()
    {
        // TODO : Destroy 할 지, 그냥 SetActive(false) 할 지 정하자
        _isDialogStart = false;
        _boxCollider.enabled = false;
        _tutorialManager.ActiveMoveKeys();
        UnregisterOnDialogEnd();
        StartCoroutine(SetActiveFalseAfter10Seconds());
    }

    IEnumerator SetActiveFalseAfter10Seconds()
    {
        yield return new WaitForSeconds(10f);
        _tutorialManager.UnactiveMoveKeys();
        gameObject.SetActive(false);
    }
}
