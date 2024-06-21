using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    public TutorialScene() : base(define.ESceneType.TUTORIAL) { }

    private void Awake()
    {
        Init();   
    }

    protected override void Init()
    {
        base.Init();
        Managers.HitParticle.Init();
        Managers.MonsterPool.Init();
        Managers.SkillPool.Init();
        Managers.Skill.Init();
        Managers.Pause.Init();
        Managers.Dialog.Init();
        Managers.CamShake.Init();
        Managers.CamManager.Init();
    }
    public override void Clear()
    {
        Managers.Dialog.Clear();
    }
}
