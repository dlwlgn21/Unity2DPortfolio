using define;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BloodEffectController : WorldSpaceEffectController
{
    private readonly static string BLOOD_BY_NORMAL_ATTACK_1_1 = "b3";
    //private static string BLOOD_BY_NORMAL_ATTACK_1_2 = "b8";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_2_1 = "b2";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_2_2 = "b5";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_3_1 = "b4";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_3_2 = "b6";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_3_3 = "b9";
    //private static string BLOOD_BY_NORMAL_ATTACK_DIE = "b7";

    private Vector3 _originalLocalScale;

    private void Awake()
    {
        AssignComponents();
        SetComponentsEnabled(false);
        _originalLocalScale = transform.localScale;
    }

    private void Update()
    {
        if (transform.parent.localRotation.eulerAngles.y > 0f)
        {
            transform.localScale = new Vector3(-1f, _originalLocalScale.y, _originalLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1f, _originalLocalScale.y, _originalLocalScale.z);
        }
        transform.position = _fixedWorldPos;
    }

    public void PlayBloodAnimation(EPlayerNoramlAttackType eType, Vector2 worldPos, ECharacterLookDir eLookDir)
    {
        if (_spriteRenderer == null)
        {
            AssignComponents();
        }
        SetComponentsEnabled(true);
        int randTwoIdx = UnityEngine.Random.Range(0, 2);
        _fixedWorldPos = worldPos;

        if (eLookDir == ECharacterLookDir.RIGHT)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        switch (eType)
        {
            case EPlayerNoramlAttackType.ATTACK_1:
                {
                    _animator.Play(BLOOD_BY_NORMAL_ATTACK_1_1, -1, 0f);
                    break;
                }
            case EPlayerNoramlAttackType.ATTACK_2:
                {
                    if (randTwoIdx == 0)
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_2_1, -1, 0f);
                    }
                    else
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_2_2, -1, 0f);
                    }
                }
                break;
            case EPlayerNoramlAttackType.ATTACK_3:
                {
                    int rand = UnityEngine.Random.Range(0, 3);
                    if (rand == 0)
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_3_1, -1, 0f);
                    }
                    else if (rand == 1)
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_3_2, -1, 0f);
                    }
                    else
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_3_3, -1, 0f);
                    }
                    break;
                }
            case EPlayerNoramlAttackType.BACK_ATTACK:
                {
                    int rand = UnityEngine.Random.Range(0, 3);
                    //BloodEffectSpriteRenderer.flipX = !BloodEffectSpriteRenderer.flipX;
                    if (rand == 0)
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_3_1, -1, 0f);
                    }
                    else if (rand == 1)
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_3_2, -1, 0f);
                    }
                    else
                    {
                        _animator.Play(BLOOD_BY_NORMAL_ATTACK_3_3, -1, 0f);
                    }
                    break;
                }
        }
    }

    private void SetComponentsEnabled(bool isEnable)
    {
        _animator.enabled = isEnable;
        _spriteRenderer.enabled = isEnable;
    }
}
