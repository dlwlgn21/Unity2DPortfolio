using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public sealed class Skill_SpawnShooterObject : MonoBehaviour
{
    public ESkillType ESkillType { get; set; }
    const string SPAWN_SHOOTER_ANIM_KEY = "SpawnShooter";

    ECharacterLookDir _ePlayerLookDir;
    Animator _animator;
    Transform _shootPoint;
    GameObject _headLight;
    GameObject _weapenLight;
    LightController _attackLightController;
    LightController _dieLightController;
    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            _shootPoint = transform.Find("ShootPoint");
            _headLight = transform.Find("HeadLight").gameObject;
            _weapenLight = transform.Find("WeaponLight").gameObject;
            _attackLightController = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "AttackLight");
            _dieLightController = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "DieLight");
            _attackLightController.TurnOffGraduallyLightTimeInSec = 0.7f;
            _dieLightController.TurnOffGraduallyLightTimeInSec = 0.7f;
            gameObject.SetActive(false);
        }
    }


    public void SpawnShooter(Vector2 pos, ECharacterLookDir eLookDir)
    {
        gameObject.SetActive(true);
        TurnOnLights();
        transform.position = pos;
        _ePlayerLookDir = eLookDir;
        if (eLookDir == ECharacterLookDir.Left)
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        _animator.Play(SPAWN_SHOOTER_ANIM_KEY, -1, 0f);
    }
    public void OnValidShootTiming()
    {
        GameObject go = Managers.ProjectilePool.GetPlayerKnockbackBoom(_shootPoint.position);
        Debug.Assert(go != null);
        go.GetComponent<PlayerSkillProjectileController>().Launch(_ePlayerLookDir);
    }


    public void OnAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }

    private void TurnOnLights()
    {
        _headLight.SetActive(true);
        _weapenLight.SetActive(true);
    }

    void OnTurnOffHeadLightTiming()
    {
        _headLight.SetActive(false);
        _weapenLight.SetActive(false);
    }

    void OnTurnOffWeaponLightTiming()
    {
        _weapenLight.SetActive(false);
    }
    void OnTurnOnAttackLightTiming()
    {
        _attackLightController.TurnOnLight();
    }
    void OnTurnOffAttackLightTiming()
    {
        _attackLightController.TurnOffLightGradually();
    }
    void OnTurnOnDieLightTiming()
    {
        _dieLightController.TurnOnLight();
    }
    void OnTurnOffDieLightTiming()
    {
        _dieLightController.TurnOffLightGradually();
    }
}