using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public sealed class Skill_SpawnPandaObject : MonoBehaviour
{
    private Animator _animator;
    private Transform _shootPoint;

    private ECharacterLookDir _ePlayerLookDir;
    private Animator _spawnEffectAnimator;
    private LightController _headLight;
    private LightController _weapenLight;
    public ESkillType ESkillType { get; set; }
    private const string SPAWN_SHOOTER_ANIM_KEY = "SpawnShooter";
    private const string SPAWN_SHOOTER_EFFECT_ANIM_KEY = "ShooterSpawnEffect";
    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            _shootPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "ShootPoint");
            _spawnEffectAnimator = Utill.GetComponentInChildrenOrNull<Animator>(gameObject, "SpawnEffect");
            _headLight = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "HeadLight");
            _weapenLight = Utill.GetComponentInChildrenOrNull<LightController>(gameObject, "WeaponLight");
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
        _spawnEffectAnimator.Play(SPAWN_SHOOTER_EFFECT_ANIM_KEY, -1, 0);
        _animator.Play(SPAWN_SHOOTER_ANIM_KEY, -1, 0f);
    }
    public void OnValidShootTiming()
    {
        GameObject go = Managers.ProjectilePool.GetPlayerKnockbackBoom(_shootPoint.position);
        Debug.Assert(go != null);
        go.GetComponent<PlayerSkillProjectileController>().Launch(_ePlayerLookDir);
    }
    public void OnTransparenyAnimStart()
    {
        _spawnEffectAnimator.Play(SPAWN_SHOOTER_EFFECT_ANIM_KEY, -1, 0);
        TurnOffLightGradullaly();
    }

    public void OnAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }

    private void TurnOnLights()
    {
        _headLight.TurnOnLight();
        _weapenLight.TurnOnLight();
    }

    private void TurnOffLightGradullaly()
    {
        _headLight.TurnOffLightGradually();
        _weapenLight.TurnOffLightGradually();
    }
}
