using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEditor.Rendering;
using System;
using UnityEngine.Events;

public sealed class UIDialogManager
{

    public bool IsTalking { get; private set; }
    public UnityAction OnConversationEndHandler = null;

    GameObject _UIDialog;
    GameObject _dialogBoxImg;
    TextMeshProUGUI _NPCName;
    TextMeshProUGUI  _dialogText;
    string _paragraph;
    bool _isConversationEnded = false;
    bool _isTyping = false;
    Queue<string> _paragraphQueue = new Queue<string>();

    public void Init()
    {
        if (GameObject.Find("UIDialog") == null)
        {
            GameObject ori = Managers.Resources.Load<GameObject>("Prefabs/UI/UIDialog");
            _UIDialog = UnityEngine.Object.Instantiate(ori);
            _UIDialog.name = "UIDialog";
            _dialogBoxImg = _UIDialog.transform.GetChild(0).gameObject;
            _NPCName = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(_dialogBoxImg, "NpcName");
            _dialogText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(_dialogBoxImg, "DialogText");
            UnityEngine.Object.DontDestroyOnLoad(_UIDialog);
        }
    }
    public void DisplayNextParagraph(DialogText dText)
    {
        if (_isTyping)
            return;

        if (_paragraphQueue.Count <= 0)
        {
            if (!_isConversationEnded)
            {
                StartConversation(dText);
            }
            else
            {
                EndConversation();
                return;
            }
        }
        _dialogText.text = "";
        _paragraph = _paragraphQueue.Dequeue();
        _dialogText.DOText(_paragraph, 1f).OnStart(OnTypingStarted).OnComplete(OnTypingEnded);
        if (_paragraphQueue.Count <= 0)
            _isConversationEnded = true;
    }

    private void StartConversation(DialogText dText)
    {
        IsTalking = true;
        if (!_dialogBoxImg.activeSelf)
            _dialogBoxImg.SetActive(true);
        _NPCName.text = dText.SpeckerName;
        for (int i = 0; i < dText.Paragraphs.Length; i++)
        {
            _paragraphQueue.Enqueue(dText.Paragraphs[i]);
        }
    }
    private void EndConversation()
    {
        // EndConversation() 두 번 호출되는거 방지용.
        if (IsTalking == false)
        {
            return;
        }
        if (OnConversationEndHandler != null)
        {
            OnConversationEndHandler.Invoke();
        }
        IsTalking = false;
        _paragraphQueue.Clear();
        _isConversationEnded = false;
        if (_dialogBoxImg.activeSelf)
        {
            _dialogBoxImg.SetActive(false);
        }
    }
    public void OnTypingStarted()
    {
        _isTyping = true;
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_UI_DIALOG_START);
    }
    public void OnTypingEnded()
    {
        _isTyping = false;
    }

    public void Clear()
    {
        _paragraphQueue.Clear();
        OnConversationEndHandler = null;
    }
}
