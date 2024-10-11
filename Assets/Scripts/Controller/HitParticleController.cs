using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleController : MonoBehaviour
{
    private ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _particle.Stop();
    }

    public void PlayBigAttackParticle()
    {
        _particle.Play();
    }
}
