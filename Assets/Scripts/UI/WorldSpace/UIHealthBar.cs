using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    protected Gradient _Gradient;


    public Image HealthBar { get; private set; }
    protected Transform mParent;
    protected BaseStat mStat;

    float mFillSpeed = 0.5f;
    float mYMargin;
    protected abstract void Init();

    private void Start()
    {
        mParent = transform.parent;
        foreach (Image component in gameObject.GetComponentsInChildren<Image>())
        {
            if (component != null && component.name == "Fill")
            {
                HealthBar = component;
                break;
            }
        }
        Init();
        Debug.Assert(HealthBar != null);
        
        mYMargin = transform.parent.GetComponent<Collider2D>().bounds.size.y + 0.15f;
        SetHpRatio(1);
    }

    private void Update()
    {
        transform.position = mParent.position + Vector3.up * mYMargin;
    }
    public void SetHpRatio(float ratio)
    {
        HealthBar.DOFillAmount(ratio, mFillSpeed);
        HealthBar.DOColor(_Gradient.Evaluate(ratio), mFillSpeed);
    }
}
