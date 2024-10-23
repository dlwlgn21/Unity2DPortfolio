using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaterialFlashController : MonoBehaviour
{
    [SerializeField] protected Material _normalMat;
    [SerializeField] protected Material _damageFlashMat;
    protected const string FLASH_COLOR_KEY = "_FlashColor";
    protected const string FLASH_AMOUNT_KEY = "_FlashAmount";
    protected SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(_normalMat != null && _damageFlashMat != null);
    }


    protected void InitAmountAndTimeVariable(ref float flashAmount, ref float elapsedTime)
    {
        flashAmount = 0f;
        elapsedTime = 0f;
    }
    protected void SetMaterialAndColor(Material mat, Color color)
    {
        SetMaterial(mat);
        _damageFlashMat.SetColor(FLASH_COLOR_KEY, color);
    }
    protected void SetMaterial(Material mat)
    {
        _spriteRenderer.material = mat;
    }
    protected void DecreaseFlashAmount(ref float currFlashAmount, ref float elapsedTime, float flashTime)
    {
        elapsedTime += Time.deltaTime;
        currFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
        _damageFlashMat.SetFloat(FLASH_AMOUNT_KEY, currFlashAmount);
    }
}
