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
        InteractBox.PlayerEnterEventHandler -= OnPlayerEnterColossalBossZone;
        InteractBox.PlayerEnterEventHandler += OnPlayerEnterColossalBossZone;
    }

    protected override void Init()
    {
        base.Init();
        Managers.PlayerRespawn.SpawnPlayer(false);
        Managers.Sound.Play(DataManager.SFX_BGM_CAVE_COLOSSAL, define.ESoundType.Bgm);
    }

    void OnPlayerEnterColossalBossZone(GameObject go)
    {
        if (go.name == "ColossalBossWakeZone")
        {
            Managers.Sound.Play(DataManager.SFX_BGM_COLOSSAL_BATTLE, define.ESoundType.Bgm);
        }
    }


    public override void Clear()
    {
        Managers.CamSwitch.Clear();
        Managers.Tween.Clear();
    }
}
