using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class UIHealthBar : MonoBehaviour
{
    [SerializeField]
    protected Gradient _Gradient;
    protected Image mHealthBar;
    protected Transform mParent;
    protected BaseStat mStat;

    float mFillSpeed;
    float mYMargin;
    protected abstract void Init();

    private void Start()
    {
        mParent = transform.parent;
        foreach (Image component in gameObject.GetComponentsInChildren<Image>())
        {
            if (component != null && component.name == "Fill")
            {
                mHealthBar = component;
                break;
            }
        }
        Init();
        Debug.Assert(mHealthBar != null);
        mFillSpeed = 0.5f;
        mYMargin = transform.parent.GetComponent<Collider2D>().bounds.size.y + 0.1f;
    }

    private void Update()
    {
        transform.position = mParent.position + Vector3.up * mYMargin;
        SetHpRatio((float)mStat.HP / mStat.MaxHP);

    }
    public void SetHpRatio(float ratio)
    {
        mHealthBar.DOFillAmount(ratio, mFillSpeed);
        mHealthBar.DOColor(_Gradient.Evaluate(ratio), mFillSpeed);
    }
}
