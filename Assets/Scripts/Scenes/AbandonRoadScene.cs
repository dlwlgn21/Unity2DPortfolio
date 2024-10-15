using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbandonRoadScene : PlayScene
{
    public AbandonRoadScene() : base(define.ESceneType.AbandonLoadScene) { }

    private void Awake()
    {
        Debug.Log($"AbandonRoadScene Awake Called");
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
