using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayScene : BaseScene
{
    protected PlayScene(ESceneType eSceneType) : base(eSceneType)
    { }

    protected override void Init()
    {
        base.Init();
        // Debuging 편하게 하기 위해서 다시 한 번 Init해줌. 그래야 바로 MainPlayScene에서 테스트 해볼 수 있으니까
        Managers.MonsterPool.Init();
        Managers.ProjectilePool.Init();
        Managers.Pause.Init();
        Managers.Dialog.Init();
        Managers.CamShake.Init();
        Managers.CamSwitch.Init();
        Managers.PlayerRespawn.Init();
        Managers.MonsterSpawn.Init();

        if (GameObject.Find("PlayerMovementEffect") == null)
        {
            GameObject go = Managers.Resources.Load<GameObject>("Prefabs/Player/PlayerMovementEffect");
            GameObject movementEffect = Instantiate(go);
            movementEffect.name = "PlayerMovementEffect";
            DontDestroyOnLoad(movementEffect);
        }
    }
}
