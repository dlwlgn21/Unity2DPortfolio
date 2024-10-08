using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonRoadScene : PlayScene
{
    public AbandonRoadScene() : base(define.ESceneType.AbandonLoadScene) { }

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
        Managers.CamSwitch.Clear();
        Managers.Tween.Clear();
    }
}
