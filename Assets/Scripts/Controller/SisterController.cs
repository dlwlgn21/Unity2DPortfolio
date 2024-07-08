using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JetBrains.Annotations;
public enum ESister
{
    FIRST_MEET,
    ATTACK_TUTORUAL,
    ROLL_TUTORIAL,
    BACK_ATTACK_TUTORIAL,
    BLOCK_TUTORIAL,
    LAST_MEET,
}


public class SisterController : QuestNPC, ITalkable
{
    [SerializeField] private ESister _eSisterType;
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
        if ((GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).magnitude < 5f)
        {
            _isConversationEnd = true;
            _animator.Play("Teleport");
        }
    }
    public void OnTeleportAnimFullyPlayed()
    {
        switch (_eSisterType)
        {
            case ESister.FIRST_MEET:
                break;
            case ESister.ATTACK_TUTORUAL:
                LoadSequence("Prefabs/Tutorial/AttackTutorialSequence").OnDialogEnd();
                break;
            case ESister.ROLL_TUTORIAL:
                LoadSequence("Prefabs/Tutorial/RollTutorialSequence").OnDialogEnd();
                break;
            case ESister.BACK_ATTACK_TUTORIAL:
                LoadSequence("Prefabs/Tutorial/BackAttackTutorialSequence").OnDialogEnd();
                break;
            case ESister.BLOCK_TUTORIAL:
                LoadSequence("Prefabs/Tutorial/BlockTutorialSequence").OnDialogEnd();
                break;
            case ESister.LAST_MEET:
                Managers.Scene.LoadScene(define.ESceneType.ABANDON_ROAD_SCENE);
                break;
        }
        gameObject.SetActive(false);
    }

    private TutorialSequence LoadSequence(string path)
    {
        GameObject go = Managers.Resources.Load<GameObject>(path);
        Debug.Assert(go != null);
        switch (_eSisterType)
        {
            case ESister.ATTACK_TUTORUAL:
                return Instantiate(go).GetComponent<AttackTutorialSequence>();
            case ESister.ROLL_TUTORIAL:
                return Instantiate(go).GetComponent<RollTutorialSequence>();
            case ESister.BACK_ATTACK_TUTORIAL:
                return Instantiate(go).GetComponent<BackAttackTutorialSequence>();
            case ESister.BLOCK_TUTORIAL:
                return Instantiate(go).GetComponent<BlockTutorialSequence>();
            default:
                Debug.Assert(false);
                return null;
        }
    }
}
