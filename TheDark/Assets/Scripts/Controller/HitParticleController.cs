using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleController : MonoBehaviour
{
    private ParticleSystem _particle;
    private BaseMonsterController _mc;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _mc = transform.parent.GetComponent<BaseMonsterController>();
        Debug.Assert(_particle != null && _mc != null);
        _particle.Stop();
    }


    private void OnEnable()
    {
        BaseMonsterController.BigAttackEventHandler += OnHittedPlayerBigAttack;
    }

    private void OnDisable()
    {
        BaseMonsterController.BigAttackEventHandler -= OnHittedPlayerBigAttack;
    }


    private void OnHittedPlayerBigAttack()
    {
        if (_mc.IsHittedByPlayerNormalAttack)
        {
            _particle.Play();
        }
    }
}
