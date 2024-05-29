using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : BaseScene
{
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        Managers.MainMenu.Init();
        ESceneType = define.ESceneType.MAIN_MENU;
    }



    public override void Clear()
    {
        Debug.Log("MainMenuScene Clear Called");
    }
}
