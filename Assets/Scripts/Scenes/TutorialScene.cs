using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TutorialScene : PlayScene
{
    public TutorialScene() : base(define.ESceneType.Tutorial) { }

    private void Awake()
    {
        Init();   
    }

    protected override void Init()
    {
        base.Init();
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_BGM_TUTORIAL, define.ESoundType.Bgm);
    }
    public override void Clear()
    {
        Managers.Dialog.Clear();
        Managers.CamSwitch.Clear();
        Managers.Tween.Clear();
        Managers.Sound.Clear(define.ESceneType.Tutorial);
    }
}
