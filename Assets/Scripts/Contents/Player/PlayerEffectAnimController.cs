using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerEffectAnimController : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected PlayerController _pc;
    protected LightController _lightController;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _pc = transform.parent.GetComponent<PlayerController>();
        _lightController = Utill.GetFirstComponentInChildrenOrNull<LightController>(gameObject);
        SetComponentEnable(false);
        Init();
    }
    protected abstract void Init();
    protected abstract void OnAnimStart();
    protected abstract void OnAnimFullyPlayed();
    protected void OnLightTurnOffTiming()
    {
        _lightController.TurnOffLightGradually();
    }
    protected void SetComponentEnable(bool isEnable)
    {
        _animator.enabled = isEnable;
        _spriteRenderer.enabled = isEnable;
    }
}
