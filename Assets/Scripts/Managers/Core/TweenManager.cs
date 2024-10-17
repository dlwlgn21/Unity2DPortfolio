using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenManager
{
    const float T_VAL = 1.1f;
    const float BASIC_UI_TW_TIME_IN_SEC = 0.1f;
    static public Vector3 TWEEN_SCALE_END_VALUE = new(T_VAL, T_VAL, T_VAL);
    static public float TWEEN_SCALE_END_TIME_IN_SEC = BASIC_UI_TW_TIME_IN_SEC;
    static public float TWEEN_SCALE_END_TIME_QUARTER_IN_SEC = 0.25f;
    public void Init()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        DOTween.SetTweensCapacity(250, 50);
    }
    
    public void Clear()
    {
        DOTween.Clear();
    }

    public void StartUIScaleTW(Transform transform, float timeInSec = BASIC_UI_TW_TIME_IN_SEC)
    {
        if (IsTweening(transform))
            return;
        transform.DOScale(TWEEN_SCALE_END_VALUE, timeInSec).SetEase(Ease.InOutElastic);
    }
    public void StartUIScaleTW(Transform transform, float endValue, float timeInSec = BASIC_UI_TW_TIME_IN_SEC)
    {
        if (IsTweening(transform))
            return;
        transform.DOScale(endValue, timeInSec).SetEase(Ease.InOutElastic);
    }
    public void StartUIScaleTW(Transform transform, TweenCallback callback, float timeInSec = BASIC_UI_TW_TIME_IN_SEC)
    {
        if (IsTweening(transform))
            return;
        transform.DOScale(TWEEN_SCALE_END_VALUE, timeInSec).SetEase(Ease.InOutElastic).OnComplete(callback);
    }

    public void StartUIScaleTW(Transform transform, float endValue, TweenCallback callback, float timeInSec = BASIC_UI_TW_TIME_IN_SEC)
    {
        if (IsTweening(transform))
            return;
        transform.DOScale(endValue, timeInSec).SetEase(Ease.InOutElastic).OnComplete(callback);
    }
    public void StartUIDoPunchPos(Transform transform, float coefficient = 5f)
    {
        if (IsTweening(transform))
            return;
        transform.DOPunchPosition(Vector3.up * coefficient, 1f);
    }

    public void StartDoPunchPos(Transform transform, float coefficient = 0.1f)
    {
        if (IsTweening(transform))
            return;
        transform.DOPunchPosition(Vector3.up * coefficient, 1f);
    }

    public void StartDoPunchPos(Transform transform, Vector3 force, float duration, TweenCallback callback)
    {
        if (IsTweening(transform))
            return;
        transform.DOPunchPosition(force, duration).SetEase(Ease.InOutElastic).OnComplete(callback);
    }

    public void EndToOneUIScaleTW(Transform transform, float timeInSec = BASIC_UI_TW_TIME_IN_SEC)
    {
        if (IsTweening(transform))
            return;
        transform.DOScale(Vector3.one, timeInSec).SetEase(Ease.InOutElastic);
    }
    public void EndToZeroScaleTW(Transform transform, float timeInSec = BASIC_UI_TW_TIME_IN_SEC)
    {
        if (IsTweening(transform))
            return;
        transform.DOScale(new Vector3(0f, 0f, 1f), timeInSec).SetEase(Ease.InOutElastic);
    }

    public void EndToZeroScaleTWNoCareCurrTweening(Transform transform, TweenCallback callback, float timeInSec = BASIC_UI_TW_TIME_IN_SEC)
    {
        transform.DOScale(new Vector3(0f, 0f, 1f), timeInSec).SetEase(Ease.InOutElastic).OnComplete(callback);
    }


    bool IsTweening(Transform transform)
    {
        if (DOTween.IsTweening(transform))
            return true;
        return false;
    }
}
