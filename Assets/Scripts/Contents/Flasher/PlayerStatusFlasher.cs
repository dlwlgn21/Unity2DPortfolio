using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using define;


public class PlayerStatusFlasher : MaterialFlasher
{
    private bool _isFlashing = false;
    private readonly Color BURN_COLOR = Color.red;
    private readonly Color SLOW_COLOR = Color.blue;
    private const int FLASH_REPEAT_COUNT = 4;

    private void OnEnable()
    {
        PlayerController.PlayerStatusEffectEventHandler += OnPlayerStatusEffect;
    }
    private void OnDestroy()
    {
        PlayerController.PlayerStatusEffectEventHandler -= OnPlayerStatusEffect;
    }
    public void OnPlayerStatusEffect(EAttackStatusEffect eType, float flashTime)
    {
        switch (eType)
        {
            case EAttackStatusEffect.None:
                break;
            case EAttackStatusEffect.Knockback:
                break;
            case EAttackStatusEffect.Blind:
                break;
            case EAttackStatusEffect.Burn:
                if (_isFlashing)
                {
                    return;
                }
                StartCoroutine(BurnFlashCo(flashTime / FLASH_REPEAT_COUNT));
                break;
            case EAttackStatusEffect.Slow:
                if (_isFlashing)
                {
                    return;
                }
                StartCoroutine(SlowFlashCo(flashTime / FLASH_REPEAT_COUNT));
                break;
            case EAttackStatusEffect.Parallysis:
                break;
            default:
                break;
        }

    }

    private IEnumerator BurnFlashCo(float flashTime)
    {
        _isFlashing = true;
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        SetMaterialAndColor(_damageFlashMat, BURN_COLOR);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);
            yield return null;
        }
        SetMaterial(_normalMat);

        SetMaterialAndColor(_damageFlashMat, BURN_COLOR);
        InitAmountAndTimeVariable(ref currentFlashAmount, ref elapsedTime);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);

            yield return null;
        }
        SetMaterial(_normalMat);


        SetMaterialAndColor(_damageFlashMat, BURN_COLOR);
        InitAmountAndTimeVariable(ref currentFlashAmount, ref elapsedTime);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);

            yield return null;
        }
        SetMaterial(_normalMat);

        SetMaterialAndColor(_damageFlashMat, BURN_COLOR);
        InitAmountAndTimeVariable(ref currentFlashAmount, ref elapsedTime);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);

            yield return null;
        }
        SetMaterial(_normalMat);

        _isFlashing = false;
    }

    private IEnumerator SlowFlashCo(float flashTime)
    {
        _isFlashing = true;
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        SetMaterialAndColor(_damageFlashMat, SLOW_COLOR);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);
            yield return null;
        }
        SetMaterial(_normalMat);

        SetMaterialAndColor(_damageFlashMat, SLOW_COLOR);
        InitAmountAndTimeVariable(ref currentFlashAmount, ref elapsedTime);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);

            yield return null;
        }
        SetMaterial(_normalMat);


        SetMaterialAndColor(_damageFlashMat, SLOW_COLOR);
        InitAmountAndTimeVariable(ref currentFlashAmount, ref elapsedTime);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);
            yield return null;
        }
        SetMaterial(_normalMat);

        SetMaterialAndColor(_damageFlashMat, SLOW_COLOR);
        InitAmountAndTimeVariable(ref currentFlashAmount, ref elapsedTime);
        while (elapsedTime < flashTime)
        {
            DecreaseFlashAmount(ref currentFlashAmount, ref elapsedTime, flashTime);
            yield return null;
        }
        SetMaterial(_normalMat);
        _isFlashing = false;
    }

}
