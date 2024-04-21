using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TweenManager
{
    public void Init()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        DOTween.SetTweensCapacity(250, 50);
    }

}
