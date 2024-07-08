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
        // Debuging ���ϰ� �ϱ� ���ؼ� �ٽ� �� �� Init����. �׷��� �ٷ� MainPlayScene���� �׽�Ʈ �غ� �� �����ϱ�
        Managers.HitParticle.Init();
        Managers.MonsterPool.Init();
        Managers.ProjectilePool.Init();
        Managers.Pause.Init();
        Managers.Dialog.Init();
        Managers.CamShake.Init();
        Managers.CamManager.Init();
        Managers.PlayerRespawn.Init();

        // For Event Test
        Managers.RegisterStaticEventManager.Init();

        if (GameObject.Find("PlayerMovementEffect") == null)
        {
            GameObject go = Managers.Resources.Load<GameObject>("Prefabs/Player/PlayerMovementEffect");
            GameObject movementEffect = Instantiate(go);
            movementEffect.name = "PlayerMovementEffect";
            DontDestroyOnLoad(movementEffect);
        }
    }
}
