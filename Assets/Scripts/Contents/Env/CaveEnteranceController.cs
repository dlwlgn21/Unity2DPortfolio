using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEnteranceController : BaseInteractableController
{
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
            Managers.Scene.LoadScene(define.ESceneType.ColossalBossCaveScene);
        }
    }
    public override void OnPlayerExit(Collider2D collision)
    {
        _interactKey.UnactiveInteractKey();
    }
}
