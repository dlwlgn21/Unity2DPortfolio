using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitAnimController : WorldSpaceAnimController
{
    public readonly static string HIT_ANIM_1_KEY = "Hit1";
    public readonly static string HIT_ANIM_2_KEY = "Hit2";
    public readonly static string HIT_ANIM_3_KEY = "Hit3";

    private PlayerController _pc;
    private void Awake()
    {
        Init();
        SetComponentsEnabled(false);
        _pc = transform.parent.gameObject.GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        PlayerController.HitEffectEventHandler -= OnHittedByMonsterNormalAttack;
        PlayerController.HitEffectEventHandler += OnHittedByMonsterNormalAttack;
    }

    private void OnDisable()
    {
        PlayerController.HitEffectEventHandler -= OnHittedByMonsterNormalAttack;
    }

    private void Update()
    {
        FixPosition();
    }

    public void OnHittedByMonsterNormalAttack(EPlayerState eState)
    {
        SetComponentsEnabled(true);
        SetFixedFos(_pc.transform.position);
        switch (eState)
        {
            case EPlayerState.HitByMelleAttack:
                _animator.Play(HIT_ANIM_2_KEY, -1, 0f);
                break;
            case EPlayerState.BlockSucces:
                _animator.Play(HIT_ANIM_3_KEY, -1, 0f);
                break;
            default:
                break;
        }
    }

    protected override void SetSpriteFlip(ECharacterLookDir eLookDir)
    {
    }
}
