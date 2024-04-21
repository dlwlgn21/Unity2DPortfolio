using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HitParticleManager
{
    // TODO : 플레이어, 몬스터 HitEffectParticle 구현 해야 함.
    public ParticleSystem Particle { get; set; }
    public void Init()
    {
        GameObject ori = Managers.Resources.Load<GameObject>("Prefabs/HitParticleManager");
        Debug.Assert(ori != null);
        ori.transform.position = Vector3.zero;
        GameObject go = Object.Instantiate(ori);
        go.name = "HitParticleManager";
        Particle = go.GetComponent<ParticleSystem>();
        Object.DontDestroyOnLoad(go);
        Debug.Assert(Particle != null);
    }

    public void Play(Vector3 pos)
    {
        Particle.transform.position = pos;
        Particle.Play();
    }
}
