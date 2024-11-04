using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public sealed class MonsterFlashController : MaterialFlashController
{
    [SerializeField] Color _flashColor = Color.red;
    [SerializeField] float _flashTimeInSec = 0.2f;
    Coroutine _flashCoOrNull;
    public void StartDamageFlash()
    {
        if (_flashCoOrNull == null)
            _flashCoOrNull = StartCoroutine(DamageFlashCo());
    }
    IEnumerator DamageFlashCo()
    {
        float currentFlashAmount = 0f;
        float elapsedTimeInSec = 0f;
        SetMaterialAndColor(_damageFlashMat, _flashColor);
        while (elapsedTimeInSec < _flashTimeInSec)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTimeInSec, _flashTimeInSec);
            yield return null;
        }
        SetMaterial(_normalMat);
        _flashCoOrNull = null;
    }
}
