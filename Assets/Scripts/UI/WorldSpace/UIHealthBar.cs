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
    protected Transform _parent;
    protected BaseStat _stat;

    float _fillSpeed = 0.5f;
    float _yMargin;
    protected abstract void Init();

    private void Start()
    {
        _parent = transform.parent;
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
        
        _yMargin = transform.parent.GetComponent<Collider2D>().bounds.size.y + 0.15f;
        SetHpRatio(1);
    }

    private void Update()
    {
        transform.position = _parent.position + Vector3.up * _yMargin;
    }
    public void SetHpRatio(float ratio)
    {
        HealthBar.DOFillAmount(ratio, _fillSpeed);
        HealthBar.DOColor(_Gradient.Evaluate(ratio), _fillSpeed);
    }
}
