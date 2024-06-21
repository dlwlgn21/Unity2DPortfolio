using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayScene : BaseScene
{
    public MainPlayScene() : base(define.ESceneType.MAIN_PLAY) { }

    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        // Debuging ���ϰ� �ϱ� ���ؼ� �ٽ� �� �� Init����. �׷��� �ٷ� MainPlayScene���� �׽�Ʈ �غ� �� �����ϱ�
        Managers.HitParticle.Init();
        Managers.MonsterPool.Init();
        Managers.SkillPool.Init();
        Managers.Skill.Init();
        Managers.Pause.Init();
        Managers.Dialog.Init();
        Managers.CamShake.Init();
        Managers.CamManager.Init();

        // For Event Test
        Managers.RegisterStaticEventManager.Init();
    }

    public override void Clear()
    {
    }
}
