using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public abstract class Skill_BaseObject : MonoBehaviour
{
    protected Animator _animator;
    protected LightController _attackLightController;
    protected const float TURN_OFF_LIGHT_TIME = 0.7f;
    protected string _animKey;
    public ESkillType ESkillType { get; set; }
    protected abstract void Init();

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _attackLightController = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "AttackLight");
        _attackLightController.SetTurnOffLightTime(TURN_OFF_LIGHT_TIME);
        gameObject.SetActive(false);
        Init();
    }

    public void UseSkill(Vector2 pos, ECharacterLookDir eLookDir)
    {
        Debug.Assert(!string.IsNullOrEmpty(_animKey));
        gameObject.SetActive(true);
        transform.position = pos;
        if (eLookDir == ECharacterLookDir.Left)
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        _attackLightController.TurnOnLight();
        _animator.Play(_animKey, -1, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            BaseMonsterController mc = collision.gameObject.GetComponent<BaseMonsterController>();
            if (mc != null)
            {
                mc.OnHittedByPlayerSkill(Managers.Data.SkillInfoDict[(int)ESkillType]);
            }
        }
    }

    void OnValidAttackTiming()
    {
        _attackLightController.TurnOffLightGradually();
    }

    void OnAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }


}
