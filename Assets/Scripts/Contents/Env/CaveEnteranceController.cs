using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CaveEnteranceController : BaseInteractableController
{
    static public UnityAction DeniedEnterCaveEventHandler;
    private void Start()
    {
        Init();
    }

    public override void OnPlayerEnter(Collider2D collision)
    {
        _interactKey.ActiveInteractKey();
    }


    public override void OnPlayerStay(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Managers.MonsterPool.MonsterCountInCurrScene != 0)
            {
                if (DeniedEnterCaveEventHandler != null)
                    DeniedEnterCaveEventHandler.Invoke();
            }
            else
            {
                Managers.Scene.LoadScene(define.ESceneType.ColossalBossCaveScene);
            }
        }
    }
    public override void OnPlayerExit(Collider2D collision)
    {
        _interactKey.UnactiveInteractKey();
    }

    private void OnDestroy()
    {
        DeniedEnterCaveEventHandler = null;
    }
}
