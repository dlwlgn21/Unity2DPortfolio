using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ColossalBossCaveScene : PlayScene
{
    public ColossalBossCaveScene() : base(define.ESceneType.ColossalBossCaveScene) { }

    private void Awake()
    {
        Init();
        //InteractBox.PlayerEnterEventHandler -= OnPlayerEnterColossalBossZone;
        //InteractBox.PlayerEnterEventHandler += OnPlayerEnterColossalBossZone;
    }

    protected override void Init()
    {
        base.Init();
        Managers.PlayerRespawn.SpawnPlayer(false);
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_BGM_CAVE_COLOSSAL, define.ESoundType.Bgm);
        DoorController bossRoomDoor = GameObject.FindGameObjectWithTag("Door").GetComponent<DoorController>();
        Debug.Assert(bossRoomDoor != null);
        bossRoomDoor.SetConditionFunc(IsCanEnterBossRoom);
    }

    //void OnPlayerEnterColossalBossZone(GameObject go)
    //{
        //if (go.name == "ColossalBossWakeZone")
        //{
        //    Managers.Sound.Play(DataManager.SFX_BGM_COLOSSAL_BATTLE, define.ESoundType.Bgm);
        //}
    //}


    public override void Clear()
    {
        Managers.CamSwitch.Clear();
        Managers.Tween.Clear();
        Managers.Sound.Clear(define.ESceneType.ColossalBossCaveScene);
    }

    bool IsCanEnterBossRoom()
    {
        if (Managers.MonsterPool.MonsterCountInCurrScene == 0)
            return true;
        return false;
    }
}
