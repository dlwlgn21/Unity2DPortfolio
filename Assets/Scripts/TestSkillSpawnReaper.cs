using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TestSkillSpawnReaper : MonoBehaviour
{
    private const int COEFFICIENT_VALUE_FOR_LIGHTING = 6;
    [SerializeField] private float _attackRange;
    private Animator _animator;
    private Transform _attackPoint;
    private const int MONSTER_LAYER_MASK = 1 << ((int)define.EColliderLayer.MONSTERS);
    private Vector2 _cachedLocalPos;
    private Light2D _light;
    private float _objectIntencity;


    private void Start()
    {
        _attackPoint = transform.Find("AttackPoint").gameObject.transform;
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        _cachedLocalPos = transform.localPosition;
        _light = transform.GetChild(1).GetComponent<Light2D>();
        _objectIntencity = _light.intensity;
        _light.intensity = 0f;
    }

    public void SpawnReaper(ECharacterLookDir eLookDir)
    {
        gameObject.SetActive(true);
        if (eLookDir == ECharacterLookDir.LEFT)
        {
            transform.localPosition = new Vector2(-_cachedLocalPos.x, _cachedLocalPos.y);
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else
        {
            transform.localPosition = _cachedLocalPos;
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        StartCoroutine(IncreaseLightIntencityGradually());
        _animator.Play("SpawnReaper", -1, 0f);
    }
    public void OnValidAttackTiming()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, MONSTER_LAYER_MASK);
        if (monsters == null)
            return;

        foreach (Collider2D mon in monsters)
        {
            BaseMonsterController controller = mon.gameObject.GetComponent<BaseMonsterController>();
            Debug.Assert(controller != null);
            controller.HittedByPlayerSpawnReaper();
        }
        StartCoroutine(DcreaseLightIntencityGradually());
    }

    public void OnSpawnReaperAnimFullyPlayed()
    {
        _light.intensity = 0f;
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    IEnumerator IncreaseLightIntencityGradually()
    {
        while (_light.intensity < _objectIntencity)
        {
            _light.intensity = _light.intensity + (Time.deltaTime * COEFFICIENT_VALUE_FOR_LIGHTING);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DcreaseLightIntencityGradually()
    {
        while (_light.intensity > 0f)
        {
            _light.intensity = _light.intensity - (Time.deltaTime * COEFFICIENT_VALUE_FOR_LIGHTING);
            yield return new WaitForEndOfFrame();
        }
    }
}
