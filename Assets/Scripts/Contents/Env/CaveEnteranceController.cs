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
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E))
        {
            Debug.Log("Key Down!!");
            Managers.Scene.LoadScene(define.ESceneType.COLOSSAL_BOSS_CAVE_SCENE);
        }
    }
    public override void OnPlayerExit(Collider2D collision)
    {
        _interactKey.UnactiveInteractKey();
    }
}
