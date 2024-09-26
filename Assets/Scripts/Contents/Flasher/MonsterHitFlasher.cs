using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class MonsterHitFlasher : MaterialFlasher
{
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _flashTime = 0.2f;
    private BaseMonsterController _mc;
    private void Start()
    {
        base.Init();
        _mc = GetComponent<BaseMonsterController>();
    }

    private void OnEnable()
    {
        BaseMonsterController.HittedByNormalAttackNoArgsEventHandler += OnHittedMonsterByPlayerNormalAttack;
    }

    private void OnDisable()
    {
        BaseMonsterController.HittedByNormalAttackNoArgsEventHandler -= OnHittedMonsterByPlayerNormalAttack;
    }
    public void OnHittedMonsterByPlayerNormalAttack()
    {
        if (_mc.IsHittedByPlayerNormalAttack)
        {
            StartCoroutine(DamageFlashCo());
        }
    }
    private IEnumerator DamageFlashCo()
    {
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        SetMaterialAndColor(_damageFlashMat, _flashColor);
        while (elapsedTime < _flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, _flashTime);
            yield return null;
        }
        SetMaterial(_normalMat);
    }
}
