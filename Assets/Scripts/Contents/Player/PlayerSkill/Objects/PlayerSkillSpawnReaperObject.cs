using define;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillSpawnReaperObject : MonoBehaviour
{
    private Animator _animator;
    private AttackLightController _attackLightController;
    private const float TURN_OFF_LIGHT_TIME = 0.7f;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _attackLightController = Utill.GetComponentInChildrenOrNull<AttackLightController>(gameObject, "AttackLight");
        _attackLightController.SetTurnOffLightTime(TURN_OFF_LIGHT_TIME);
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
        _attackLightController.TurnOffLightGradually();
    }

    public void OnSpawnReaperAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<BaseMonsterController>()?.OnHittedByPlayerSpawnReaper();
        }
    }
}
