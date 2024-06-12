using define;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillSpawnReaper : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    private Animator _animator;
    private Transform _attackPoint;
    private const int MONSTER_LAYER_MASK = 1 << ((int)define.EColliderLayer.MONSTERS);
    private AttackLightController _attackLightController;
    
    private void Start()
    {
        _attackPoint = transform.Find("AttackPoint").gameObject.transform;
        _animator = GetComponent<Animator>();
        _attackLightController = Utill.GetComponentInChildrenOrNull<AttackLightController>(gameObject, "AttackLight");
        _attackLightController.SetTurnOffLightTime(0.7f);
        gameObject.SetActive(false);
    }

    public void SpawnReaper(Vector2 pos, ECharacterLookDir eLookDir)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        if (eLookDir == ECharacterLookDir.LEFT)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        _attackLightController.TurnOnLight();
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
        _attackLightController.TurnOffLightGradually();
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
