using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossalBossCaveScene : PlayScene
{
    public ColossalBossCaveScene() : base(define.ESceneType.ColossalBossCaveScene) { }

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
    }
}
