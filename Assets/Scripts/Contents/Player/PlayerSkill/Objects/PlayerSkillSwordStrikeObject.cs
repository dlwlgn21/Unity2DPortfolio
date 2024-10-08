using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSwordStrikeObject : MonoBehaviour
{
    private Animator _animator;
    private LightController _attackLightController;
    private const float TURN_OFF_LIGHT_TIME = 0.7f;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _attackLightController = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "AttackLight");
        _attackLightController.SetTurnOffLightTime(TURN_OFF_LIGHT_TIME);
        gameObject.SetActive(false);
    }

    public void CastSwordStrike(Vector2 pos, ECharacterLookDir eLookDir)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        if (eLookDir == ECharacterLookDir.Left)
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        _attackLightController.TurnOnLight();
        _animator.Play("SwordStrike", -1, 0f);
    }
    public void OnValidAttackTiming()
    {
        _attackLightController.TurnOffLightGradually();
    }

    public void OnAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<BaseMonsterController>()?.OnHittedByPlayerSkill(Managers.Data.SkillInfoDict[(int)ESkillType.Cast_SwordStrike]);
        }
    }
}
