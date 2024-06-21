using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitEffectController : WorldSpaceEffectController
{
    public readonly static string HIT_EFFECT_1_KEY = "Hit1";
    public readonly static string HIT_EFFECT_2_KEY = "Hit2";
    public readonly static string HIT_EFFECT_3_KEY = "Hit3";

    private BaseMonsterController _mc;
    private void Awake()
    {
        Init();
        SetComponentsEnabled(false);
        _mc = transform.parent.gameObject.GetComponent<BaseMonsterController>();
    }
    private void OnEnable()
    {
        BaseMonsterController.HittedByNormalAttackEffectEventHandler += OnHittedByPlayerNormalAttack;
    }

    private void OnDisable()
    {
        BaseMonsterController.HittedByNormalAttackEffectEventHandler -= OnHittedByPlayerNormalAttack;
    }

    private void Update()
    {
        FixPosition();
    }

    public void OnHittedByPlayerNormalAttack(EPlayerNoramlAttackType eType)
    {
        SetComponentsEnabled(true);
        if (!_mc.IsHittedByPlayerNormalAttack)
        {
            return;
        }
        SetFixedFos(_mc.transform.position);
        switch (eType)
        {
            case EPlayerNoramlAttackType.ATTACK_1:
                _animator.Play(HIT_EFFECT_1_KEY, -1, 0f);
                break;
            case EPlayerNoramlAttackType.ATTACK_2:
                _animator.Play(HIT_EFFECT_2_KEY, -1, 0f);
                break;
            case EPlayerNoramlAttackType.ATTACK_3:
                _animator.Play(HIT_EFFECT_3_KEY, -1, 0f);
                break;
            case EPlayerNoramlAttackType.BACK_ATTACK:
                _animator.Play(HIT_EFFECT_3_KEY, -1, 0f);
                break;
            default:
                break;
        }
    }
}
