using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
public enum ESisterType
{
    FirstMeet,
    AttackTutorial,
    RollTutorial,
    BackAttackTutorial,
    BlockTutorial,
    SkillTutorial,
}

public class SisterController : QuestNPC, ITalkable
{
    [SerializeField] private ESisterType _eSisterType;
    [SerializeField] private DialogText _dialogText;
    public override void Interact()
    {
        Talk(_dialogText);
    }
    public void Talk(DialogText dText)
    {
        Managers.Dialog.DisplayNextParagraph(dText);
    }

    public override void OnNPCDialogEnd()
    {
        if ((GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).magnitude < 5f)
        {
            _isConversationEnd = true;
            _animator.Play("Teleport");
        }
    }
    void OnTeleportAnimFullyPlayed()
    {
        switch (_eSisterType)
        {
            case ESisterType.FirstMeet:
                break;
            case ESisterType.AttackTutorial:
                LoadSequence("Prefabs/Tutorial/AttackTutorialSequence").OnDialogEnd();
                break;
            case ESisterType.RollTutorial:
                LoadSequence("Prefabs/Tutorial/RollTutorialSequence").OnDialogEnd();
                break;
            case ESisterType.BackAttackTutorial:
                LoadSequence("Prefabs/Tutorial/BackAttackTutorialSequence").OnDialogEnd();
                break;
            case ESisterType.BlockTutorial:
                LoadSequence("Prefabs/Tutorial/BlockTutorialSequence").OnDialogEnd();
                break;
            case ESisterType.SkillTutorial:
                LoadSequence("Prefabs/Tutorial/SkillTutorialSequence").OnDialogEnd();
                break;
        }
        gameObject.SetActive(false);
    }

    TutorialSequence LoadSequence(string path)
    {
        GameObject go = Managers.Resources.Load<GameObject>(path);
        Debug.Assert(go != null);
        switch (_eSisterType)
        {
            case ESisterType.AttackTutorial:
                return Instantiate(go).GetComponent<AttackTutorialSequence>();
            case ESisterType.RollTutorial:
                return Instantiate(go).GetComponent<RollTutorialSequence>();
            case ESisterType.BackAttackTutorial:
                return Instantiate(go).GetComponent<BackAttackTutorialSequence>();
            case ESisterType.BlockTutorial:
                return Instantiate(go).GetComponent<BlockTutorialSequence>();
            case ESisterType.SkillTutorial:
                return Instantiate(go).GetComponent<SkillTutorialSequence>();
            default:
                Debug.DebugBreak();
                return null;
        }
    }
}
