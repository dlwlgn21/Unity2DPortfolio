using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : BaseScene
{

    public MainMenuScene() : base(define.ESceneType.MainMenu) { }
    private void Awake()
    {
        Debug.Log($"MainMenuScene Awake Called");
        Init();
    }

    protected override void Init()
    {
        base.Init();
        Managers.MainMenu.Init();
    }

    public override void Clear()
    {
        Managers.Tween.Clear();
        Managers.MainMenu.Clear();
    }
}
