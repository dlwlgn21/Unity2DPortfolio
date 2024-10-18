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
        Init();
    }

    protected override void Init()
    {
        base.Init();
        Managers.PlayerRespawn.SpawnPlayer(false);
        Managers.Sound.Play(DataManager.SFX_BGM_ABANDON_ROAD, define.ESoundType.Bgm);
    }

    public override void Clear()
    {
        Managers.CamSwitch.Clear();
        Managers.Tween.Clear();
    }
}
