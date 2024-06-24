using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillSpawnShooterObject : MonoBehaviour
{
    private Animator _animator;
    private Transform _shootPoint;

    private ECharacterLookDir _ePlayerLookDir;
    private Animator _spawnEffectAnimator;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            _shootPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "ShootPoint");
            _spawnEffectAnimator = Utill.GetComponentInChildrenOrNull<Animator>(gameObject, "SpawnEffect");
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
        GameObject go = Managers.SkillPool.GetKnockbackBoom(_shootPoint.position);
        Debug.Assert(go != null);
        go.GetComponent<PlayerSkillKnockbackBoomObject>().Launch(_ePlayerLookDir);
    }

    public void OnAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }
}
