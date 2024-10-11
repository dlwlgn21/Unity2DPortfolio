using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public abstract class UI_PlayerStatBar : MonoBehaviour
{
    [SerializeField] protected Gradient _gradient;
    protected Image _barImg;
    protected const float FILL_SPEED = 0.2f;

    private void Awake()
    {
        _barImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "Bar");
        Debug.Assert(_barImg != null);
        Init();
    }

    protected abstract void Init();

    protected void SetBarRatio(float ratio, TweenCallback onTWComplete = null)
    {
        if (_barImg == null)
            Debug.DebugBreak();
        if (onTWComplete != null)
            _barImg.DOFillAmount(ratio, FILL_SPEED).OnComplete(onTWComplete).SetLink(gameObject);
        else
            _barImg.DOFillAmount(ratio, FILL_SPEED).SetLink(gameObject);
        _barImg.DOColor(_gradient.Evaluate(ratio), FILL_SPEED).SetLink(gameObject);
    }
}
