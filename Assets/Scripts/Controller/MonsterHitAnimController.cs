using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MonsterHitAnimController : WorldSpaceAnimController
{
    const string HIT_EFFECT_1_KEY = "Hit1";
    const string HIT_EFFECT_2_KEY = "Hit2";
    const string HIT_EFFECT_3_KEY = "Hit3";
    Quaternion _lastParentQut;
    private void Awake()
    {
        Init();
        SetComponentsEnabled(false);
        _lastParentQut = transform.parent.localRotation;
    }


    private void Update()
    {
        FixPosition();
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation)
                                    * _lastParentQut
                                    * transform.localRotation;
        _lastParentQut = transform.parent.localRotation;
    }

    public void PlayHitEffect(ECharacterLookDir eLookDir, Vector2 pos, EPlayerNoramlAttackType eType)
    {
        SetComponentsEnabled(true);
        SetFixedFos(pos);
        SetSpriteFlip(eLookDir);
        switch (eType)
        {
            case EPlayerNoramlAttackType.Attack_1:
                _animator.Play(HIT_EFFECT_1_KEY, -1, 0f);
                break;
            case EPlayerNoramlAttackType.Attack_2:
                _animator.Play(HIT_EFFECT_2_KEY, -1, 0f);
                break;
            case EPlayerNoramlAttackType.Attack_3:
                _animator.Play(HIT_EFFECT_3_KEY, -1, 0f);
                break;
            case EPlayerNoramlAttackType.BackAttack:
                _animator.Play(HIT_EFFECT_3_KEY, -1, 0f);
                break;
            default:
                break;
        }
    }

    protected override void SetSpriteFlip(ECharacterLookDir eLookDir)
    {
        if (eLookDir == ECharacterLookDir.Right)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;
    }
}
