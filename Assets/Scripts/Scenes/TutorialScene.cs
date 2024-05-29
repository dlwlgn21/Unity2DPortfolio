using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    private void Awake()
    {
        Init();   
    }

    protected override void Init()
    {
        base.Init();
        ESceneType = define.ESceneType.TUTORIAL;
        Managers.HitParticle.Init();
        Managers.MonsterPool.Init();
        Managers.SkillPool.Init();
        Managers.Pause.Init();
        Managers.Dialog.Init();
        Managers.CamShake.Init();
    }
    public override void Clear()
    {
        Debug.Log("TutorialScene Clear Called");
    }
}
