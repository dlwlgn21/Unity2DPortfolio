using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HitParticleManager
{
    // TODO : 플레이어, 몬스터 HitEffectParticle 구현 해야 함.
    private ParticleSystem _particle;
    public void Init()
    {
        if (GameObject.Find("HitParticleManager") == null)
        {
            GameObject ori = Managers.Resources.Load<GameObject>("Prefabs/HitParticleManager");
            Debug.Assert(ori != null);
            ori.transform.position = Vector3.zero;
            GameObject go = Object.Instantiate(ori);
            go.name = "HitParticleManager";
            _particle = go.GetComponent<ParticleSystem>();
            Object.DontDestroyOnLoad(go);
            Debug.Assert(_particle != null);
        }
    }

    public void Play(Vector3 pos)
    {
        _particle.transform.position = pos;
        _particle.Play();
    }
}
