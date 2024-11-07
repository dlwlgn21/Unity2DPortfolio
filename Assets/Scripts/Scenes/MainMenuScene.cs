using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MainMenuScene : BaseScene
{

    public MainMenuScene() : base(define.ESceneType.MainMenu) { }
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
        Managers.Tween.Clear();
        Managers.MainMenu.Clear();
    }
}
