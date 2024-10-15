using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayScene : BaseScene
{
    public static UnityAction OnSceneInitEventHandelr;
    protected PlayScene(ESceneType eSceneType) : base(eSceneType)
    { }

    protected override void Init()
    {
        base.Init();


        // Debuging ���ϰ� �ϱ� ���ؼ� �ٽ� �� �� Init����. �׷��� �ٷ� MainPlayScene���� �׽�Ʈ �غ� �� �����ϱ�
        Managers.Pause.Init();
        Managers.Dialog.Init();
        Managers.UI.Init();
        Managers.Cam.Init();
        Managers.CamSwitch.Init();
        Managers.PlayerRespawn.Init();
        Managers.PlayerLevel.Init();
        Managers.PlayerSkill.Init();
        Managers.MonsterSpawn.Init();
        Managers.MonsterPool.Init();
        Managers.ProjectilePool.Init();

        if (GameObject.Find("PlayerMovementAnimator") == null)
        {
            GameObject go = Managers.Resources.Load<GameObject>("Prefabs/Player/PlayerMovementAnimator");
            GameObject movementEffect = Instantiate(go);
            movementEffect.name = "PlayerMovementAnimator";
            DontDestroyOnLoad(movementEffect);
        }
    }
}
