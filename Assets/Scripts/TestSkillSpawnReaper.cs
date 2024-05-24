using define;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class TestSkillSpawnReaper : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    private Animator _animator;
    private Transform _attackPoint;
    private const int MONSTER_LAYER_MASK = 1 << ((int)define.EColliderLayer.MONSTERS);
    private Vector2 _cachedLocalPos;

    private void Start()
    {
        _attackPoint = transform.Find("AttackPoint").gameObject.transform;
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        _cachedLocalPos = transform.localPosition;
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
    }

    public void OnSpawnReaperAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
