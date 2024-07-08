using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractKey : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sr;
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        UnactiveInteractKey();
    }


    public void ActiveInteractKey()
    {
        SetEnableComponents(true);
    }

    public void UnactiveInteractKey()
    {
        SetEnableComponents(false);
    }

    private void SetEnableComponents(bool isEnable)
    {
        _sr.enabled = isEnable;
        _animator.enabled = isEnable;
    }
}
