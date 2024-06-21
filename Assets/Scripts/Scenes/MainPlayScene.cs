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

        // Debuging 편하게 하기 위해서 다시 한 번 Init해줌. 그래야 바로 MainPlayScene에서 테스트 해볼 수 있으니까
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
