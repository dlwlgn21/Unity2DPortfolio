using define;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterBloodEffectController : WorldSpaceEffectController
{
    private readonly static string BLOOD_BY_NORMAL_ATTACK_1_1 = "b3";
    //private static string BLOOD_BY_NORMAL_ATTACK_1_2 = "b8";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_2_1 = "b2";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_2_2 = "b5";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_3_1 = "b4";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_3_2 = "b6";
    private readonly static string BLOOD_BY_NORMAL_ATTACK_3_3 = "b9";
    //private static string BLOOD_BY_NORMAL_ATTACK_DIE = "b7";

    private BaseMonsterController _mc;

    private void Awake()
    {
        Init();
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
        FixSpriteDirection();
        FixPosition();
    }

    public void OnHittedByPlayerNormalAttack(EPlayerNoramlAttackType eType)
    {
        if (!_mc.IsHittedByPlayerNormalAttack)
        {
            return;
        }

        if (_spriteRenderer == null)
        {
            AssignComponents();
        }
        SetComponentsEnabled(true);
        int randTwoIdx = UnityEngine.Random.Range(0, 2);
        _fixedWorldPos = _mc.transform.position;

        if (_mc.ELookDir == ECharacterLookDir.Right)
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
                    _spriteRenderer.flipX = !_spriteRenderer.flipX;
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
  

}
