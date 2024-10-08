using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : PlayScene
{
    public TutorialScene() : base(define.ESceneType.Tutorial) { }

    private void Awake()
    {
        Init();   
    }

    protected override void Init()
    {
        base.Init();
    }
    public override void Clear()
    {
        Managers.Dialog.Clear();
        Managers.CamSwitch.Clear();
        Managers.Tween.Clear();
    }
}
