using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillSpawnShooter : MonoBehaviour
{
    private Animator _animator;
    //private Light2D _headLight;
    //private Light2D _WeaponLight;
    private Transform _shootPoint;
    
    private PlayerSkillLaunchBomb _launchBomb;
    private ECharacterLookDir _ePlayerLookDir;
    private Animator _spawnEffectAnimator;
    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            //_headLight = Utill.GetComponentInChildrenOrNull<Light2D>(gameObject, "HeadLight");
            //_WeaponLight = Utill.GetComponentInChildrenOrNull<Light2D>(gameObject, "WeaponLight");
            _shootPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "ShootPoint");
            _spawnEffectAnimator = Utill.GetComponentInChildrenOrNull<Animator>(gameObject, "SpawnEffect");
            GameObject bomb = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/SkillLaunchBomb");
            bomb = Instantiate(bomb);
            _launchBomb = bomb.GetComponent<PlayerSkillLaunchBomb>();
            DontDestroyOnLoad(bomb);
            gameObject.SetActive(false);
        }
    }

    public void SpawnShooter(Vector2 pos, ECharacterLookDir eLookDir)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        _ePlayerLookDir = eLookDir;
        if (eLookDir == ECharacterLookDir.LEFT)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        _spawnEffectAnimator.Play("ShooterSpawnEffect", -1, 0);
        _animator.Play("SpawnShooter", -1, 0f);
    }
    public void OnValidShootTiming()
    {
        _launchBomb.LauchBomb(_shootPoint.position, _ePlayerLookDir);
    }

    public void OnAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }
}
