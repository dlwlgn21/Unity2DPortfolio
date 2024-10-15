using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossalBossCaveScene : PlayScene
{
    public ColossalBossCaveScene() : base(define.ESceneType.ColossalBossCaveScene) { }

    private void Awake()
    {
        Debug.Log($"ColossalBossCaveScene Awake Called!");
        Init();

    }

    protected override void Init()
    {
        base.Init();
        Managers.PlayerRespawn.SpawnPlayer(false);
    }

    public override void Clear()
    {
        Managers.CamSwitch.Clear();
        Managers.Tween.Clear();
    }
}
