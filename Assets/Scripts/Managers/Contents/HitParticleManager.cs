using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HitParticleManager
{
    // TODO : �÷��̾�, ���� HitEffectParticle ���� �ؾ� ��.
    public ParticleSystem Particle { get; set; }
    public void Init()
    {
        var ori = Managers.Resources.Load<GameObject>("Prefabs/HitParticleManager");
        Debug.Assert(ori != null);
        ori.transform.position = Vector3.zero;
        var go = GameObject.Instantiate(ori);
        go.name = "HitParticleManager";
        Particle = go.GetComponent<ParticleSystem>();
        Debug.Assert(Particle != null);
    }

    public void Play(Vector3 pos)
    {
        Particle.transform.position = pos;
        Particle.Play();
    }
}
