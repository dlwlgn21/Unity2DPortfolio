using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEditor.Rendering;

public class UIDialogController : MonoBehaviour
{
    TextMeshProUGUI mNPCName;
    TextMeshProUGUI mDialogText;
    string mParagraph;

    bool mIsConversationEnded = false;
    bool mIsTyping = false;
    void Awake()
    {
        mNPCName = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "NpcName");
        mDialogText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "DialogText");
        Debug.Assert(mNPCName != null && mDialogText != null);
    }
    Queue<string> mPragraphQueue = new Queue<string>();

    public void DisplayNextParagraph(DialogText dText)
    {
        if (mIsTyping)
            return;
        if (mPragraphQueue.Count <= 0)
        {
            if (!mIsConversationEnded)
            {
                StartConversation(dText);
            }
            else
            {
                EndConversation();
                return;
            }
        }

        mDialogText.text = "";
        mParagraph = mPragraphQueue.Dequeue();
        mDialogText.DOText(mParagraph, 1f).OnStart(OnTypingStarted).OnComplete(OnTypingEnded);
        if (mPragraphQueue.Count <= 0)
            mIsConversationEnded = true;
    }

    private void StartConversation(DialogText dText)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        mNPCName.text = dText.SpeckerName;
        for (int i = 0; i < dText.Paragraphs.Length; i++)
        {
            mPragraphQueue.Enqueue(dText.Paragraphs[i]);
        }
    }
    private void EndConversation()
    {
        mPragraphQueue.Clear();
        mIsConversationEnded = false;
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    public void OnTypingStarted()
    {
        mIsTyping = true;
    }
    public void OnTypingEnded()
    {
        mIsTyping = false;
    }
}
