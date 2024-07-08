using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonRoadScene : PlayScene
{
    public AbandonRoadScene() : base(define.ESceneType.ABANDON_ROAD_SCENE) { }

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
    }
}
