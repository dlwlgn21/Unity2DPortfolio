using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ProjectilePoolManager
{
    public const int MAX_BOMB_COUNT = 10;

    private Queue<GameObject> _bombs = new Queue<GameObject>(MAX_BOMB_COUNT);
    private Queue<GameObject> _monsterDamageProjectiles = new Queue<GameObject>(MAX_BOMB_COUNT);
    private Queue<GameObject> _monsterKnockbackProjectiles = new Queue<GameObject>(MAX_BOMB_COUNT);
    private Queue<GameObject> _monsterStunProjectiles = new Queue<GameObject>(MAX_BOMB_COUNT);

    private GameObject _oriPlayerBomb;
    private GameObject _oriMonsterDamageProjectile;
    private GameObject _oriMonsterKnockbackProjectile;
    private GameObject _oriMonsterStunProjectile;
    public void Init()
    {
        if (_oriPlayerBomb == null)
        {
            _oriPlayerBomb = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/Projectile");
            _oriMonsterDamageProjectile = Managers.Resources.Load<GameObject>("Prefabs/Monsters/Projectile/MonsterDamageProjectile");
            _oriMonsterKnockbackProjectile = Managers.Resources.Load<GameObject>("Prefabs/Monsters/Projectile/MonsterKnockbackProjectile");
            _oriMonsterStunProjectile = Managers.Resources.Load<GameObject>("Prefabs/Monsters/Projectile/MonsterStunProjectile");
        }
    }

    public GameObject GetPlayerKnockbackBoom(Vector2 spawnPos)
    {
        GameObject retGo = null;
        if (_bombs.Count > 0)
        {
            retGo = _bombs.Dequeue();
            retGo.GetComponent<PlayerSkillKnockbackBoomObject>().Init(spawnPos);
        }
        else
        {
            retGo = MakeBomb(_oriPlayerBomb, spawnPos);
        }
        Debug.Assert(retGo != null);
        retGo.transform.position = spawnPos;
        retGo.SetActive(true);
        return retGo;
    }

    //public BaseMonsterProjectile GetMonsterProjectile(EMonsterProjectileType eType)
    //{
    //    switch (eType)
    //    {
    //        case EMonsterProjectileType.DAMAGE:
    //            return DequeOrMakeProjectile(_monsterDamageProjectiles, eType);
    //        case EMonsterProjectileType.KNOCKBACK:
    //            return DequeOrMakeProjectile(_monsterKnockbackProjectiles, eType);
    //        case EMonsterProjectileType.STUN:
    //            return DequeOrMakeProjectile(_monsterStunProjectiles, eType);
    //    }
    //    Debug.Assert(false);
    //    return null;
    //}

    public MonsterKnockbackProjectile GetMonsterKnockbackProjectile(Vector2 force)
    {
        MonsterKnockbackProjectile ret = (MonsterKnockbackProjectile)DequeOrMakeProjectile(_monsterKnockbackProjectiles, EMonsterProjectileType.KNOCKBACK);
        Debug.Assert(ret != null);
        ret.KnockbackForce = force;
        return ret;
    }

    public MonsterDamageProjectile GetMonsterDamageProjectile(int damage)
    {
        MonsterDamageProjectile ret = (MonsterDamageProjectile)DequeOrMakeProjectile(_monsterDamageProjectiles, EMonsterProjectileType.DAMAGE);
        Debug.Assert(ret != null);
        ret.Damage = damage;
        return ret;
    }

    public void ReturnMonsterProjectile(GameObject go, EMonsterProjectileType eType)
    {
        go.SetActive(false);
        EnqueOrDestroy(go, eType);
    }

    public void ReturnPlayerKnockbackBoom(GameObject go)
    {
        go.SetActive(false);
        if (_bombs.Count > MAX_BOMB_COUNT)
        {
            Debug.Assert(false);
            Object.Destroy(go);
        }
        else
        {
            _bombs.Enqueue(go);
        }
    }

    private GameObject MakeBomb(GameObject original, Vector2 spawnPos)
    {
        GameObject go = Object.Instantiate(original, spawnPos, Quaternion.identity);
        Object.DontDestroyOnLoad(go);
        return go;
    }


    private BaseMonsterProjectile DequeOrMakeProjectile(Queue<GameObject> q, EMonsterProjectileType eType)
    {
        BaseMonsterProjectile retProjectile;
        if (q.Count > 0)
        {
            retProjectile = q.Dequeue().GetComponent<BaseMonsterProjectile>();
        }
        else
        {
            retProjectile = MakeProjectile(eType);
        }
        retProjectile.Init();
        Debug.Assert(retProjectile != null);
        return retProjectile;
    }

    private void EnqueOrDestroy(GameObject go, EMonsterProjectileType eType)
    {
        switch (eType)
        {
            case EMonsterProjectileType.DAMAGE:
                if (_monsterDamageProjectiles.Count > MAX_BOMB_COUNT)
                {
                    Debug.Assert(false);
                    Object.Destroy(go);
                }
                else
                {
                    _monsterDamageProjectiles.Enqueue(go);
                }
                break;
            case EMonsterProjectileType.KNOCKBACK:
                if (_monsterKnockbackProjectiles.Count > MAX_BOMB_COUNT)
                {
                    Debug.Assert(false);
                    Object.Destroy(go);
                }
                else
                {
                    _monsterKnockbackProjectiles.Enqueue(go);
                }
                break;
            case EMonsterProjectileType.STUN:
                if (_monsterStunProjectiles.Count > MAX_BOMB_COUNT)
                {
                    Debug.Assert(false);
                    Object.Destroy(go);
                }
                else
                {
                    _monsterStunProjectiles.Enqueue(go);
                }
                break;
        }

    }
    private BaseMonsterProjectile MakeProjectile(EMonsterProjectileType eType)
    {
        GameObject go = null;
        switch (eType)
        {
            case EMonsterProjectileType.DAMAGE:
                go = Object.Instantiate(_oriMonsterDamageProjectile);
                break;
            case EMonsterProjectileType.KNOCKBACK:
                go = Object.Instantiate(_oriMonsterKnockbackProjectile);
                break;
            case EMonsterProjectileType.STUN:
                go = Object.Instantiate(_oriMonsterStunProjectile);
                break;
        }
        Debug.Assert(go != null);
        Object.DontDestroyOnLoad(go);
        BaseMonsterProjectile retProjectile = go.GetComponent<BaseMonsterProjectile>();
        return retProjectile;
    }
}
