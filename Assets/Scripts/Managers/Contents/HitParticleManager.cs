using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public enum EHitParticleType
{
    PLAYER_HITTED_BY_MONSTER,
    MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_1,
    MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_2,
    MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_3,
}

public class HitParticleManager
{
    // TODO : 플레이어, 몬스터 HitEffectParticle 구현 해야 함.
    private ParticleSystem _bigHittedParticle;
    public void Init()
    {
        if (GameObject.Find("HitParticleManager") == null)
        {
            GameObject ori = Managers.Resources.Load<GameObject>("Prefabs/Particles/BigHittedParticle");
            Debug.Assert(ori != null);
            ori.transform.position = Vector3.zero;
            GameObject go = Object.Instantiate(ori);
            go.name = "HitParticleManager";
            _bigHittedParticle = go.GetComponent<ParticleSystem>();
            Object.DontDestroyOnLoad(go);
            Debug.Assert(_bigHittedParticle != null);
        }
    }

    public void PlayBigHittedParticle(Vector2 pos)
    {
        _bigHittedParticle.transform.position = pos;
        //switch (eType)
        //{
        //    case EHitParticleType.PLAYER_HITTED_BY_MONSTER:
        //        break;
        //    case EHitParticleType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_1:
        //        break;
        //    case EHitParticleType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_2:
        //        break;
        //    case EHitParticleType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_3:
        //        break;
        //}
        _bigHittedParticle.Play();
    }
}
