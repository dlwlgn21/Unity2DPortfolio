using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : BaseScene
{

    public MainMenuScene() : base(define.ESceneType.MAIN_MENU) { }
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        Managers.MainMenu.Init();
    }

    public override void Clear()
    {
        Debug.Log("MainMenuScene Clear Called");
    }
}
