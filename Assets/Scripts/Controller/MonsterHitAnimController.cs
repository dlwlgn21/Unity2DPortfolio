using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitAnimController : WorldSpaceAnimController
{
    const string HIT_EFFECT_1_KEY = "Hit1";
    const string HIT_EFFECT_2_KEY = "Hit2";
    const string HIT_EFFECT_3_KEY = "Hit3";

    private void Awake()
    {
        Init();
        SetComponentsEnabled(false);
    }


    private void Update()
    {
        FixPosition();
    }

    public void PlayHitEffect(Vector2 pos, EPlayerNoramlAttackType eType)
    {
        SetComponentsEnabled(true);
        SetFixedFos(pos);
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
}
