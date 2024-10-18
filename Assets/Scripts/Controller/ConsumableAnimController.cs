using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableAnimController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private const string POTION_ANIM_KEY = "ConsumablePotion";
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        SetComponentEnable(false);
        UI_PlayerConsumableSlot.UseConsumableEventHandler -= OnUseConsumable;
        UI_PlayerConsumableSlot.UseConsumableEventHandler += OnUseConsumable;
    }
 
    void OnUseConsumable()
    {
        SetComponentEnable(true);
        _animator.Play(POTION_ANIM_KEY, -1, 0f);
    }

    void OnAnimFullyPlayed()
    {
        SetComponentEnable(false);
    }

    private void SetComponentEnable(bool isEnable)
    {
        _animator.enabled = isEnable;
        _spriteRenderer.enabled = isEnable;
    }
    private void OnDestroy()
    {
        if (UI_PlayerConsumableSlot.UseConsumableEventHandler != null)
            UI_PlayerConsumableSlot.UseConsumableEventHandler -= OnUseConsumable;
    }
}
