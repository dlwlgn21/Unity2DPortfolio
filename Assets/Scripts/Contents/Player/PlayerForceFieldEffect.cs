using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerForceFieldEffect : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerController _pc;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _pc = transform.parent.GetComponent<PlayerController>();
        PlayerController.HitEffectEventHandler += OnPlayerHittedByMonsterNormalAttack;
        SetComponentEnable(false);
    }

    private void OnDestroy()
    {
        PlayerController.HitEffectEventHandler -= OnPlayerHittedByMonsterNormalAttack;
    }
    public void OnPlayerHittedByMonsterNormalAttack(EPlayerState eState)
    {
        SetComponentEnable(true);
        _animator.Play("ForceFieldEffect", -1, 0f);
    }
    public void OnForceFieldAnimStart()
    {
        _pc.IsInvincible = true;
    }
    public void OnForceFieldAnimFullyPlayed()
    {
        _pc.IsInvincible = false;
    }

    private void SetComponentEnable(bool isEnable)
    {
        _animator.enabled = isEnable;
        _spriteRenderer.enabled = isEnable;
    }
}
