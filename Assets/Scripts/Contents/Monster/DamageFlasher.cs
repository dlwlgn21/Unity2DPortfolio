using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class DamageFlasher : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _flashTime = 0.2f;
    [SerializeField] private Material _normalMat;
    [SerializeField] private Material _damageFlashMat;

    const string FLASH_COLOR = "_FlashColor";
    const string FLASH_AMOUNT = "_FlashAmount";

    private SpriteRenderer _spriteRenderer;
    private BaseMonsterController _mc;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(_normalMat != null && _damageFlashMat != null);
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
            StartDamageFlash();
        }
    }
    private void StartDamageFlash()
    {
        StartCoroutine(DamageFlashCo());
    }

    private IEnumerator DamageFlashCo()
    {
        _spriteRenderer.material = _damageFlashMat;

        _damageFlashMat.SetColor(FLASH_COLOR, _flashColor);

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            _damageFlashMat.SetFloat(FLASH_AMOUNT, currentFlashAmount);
            yield return null;
        }

        _spriteRenderer.material = _normalMat;
    }
}
