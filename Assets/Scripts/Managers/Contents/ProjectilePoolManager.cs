using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static UnityEngine.UI.Image;

public sealed class ProjectilePoolManager
{
    public const int MAX_BOMB_COUNT = 10;

    private Queue<GameObject> _bombs = new Queue<GameObject>(MAX_BOMB_COUNT);
    private Queue<GameObject> _monsterProjectiles = new Queue<GameObject>(MAX_BOMB_COUNT);

    private GameObject _oriPlayerBomb;
    private GameObject _oriMonsterProjectile;
    public void Init()
    {
        if (_oriPlayerBomb == null)
        {
            _oriPlayerBomb = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/PlayerProjectile");
            _oriMonsterProjectile = Managers.Resources.Load<GameObject>("Prefabs/Monsters/Projectile/MonsterProjectile");
        }
    }

    public GameObject GetPlayerKnockbackBoom(Vector2 spawnPos)
    {
        GameObject retGo = null;
        if (_bombs.Count > 0)
        {
            retGo = _bombs.Dequeue();
            retGo.GetComponent<PlayerSkillProjectileController>().Init(spawnPos);
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

    public MonsterProjectileController GetMonsterProjectile()
    {
        MonsterProjectileController ret = DequeOrMakeProjectile(_monsterProjectiles);
        Debug.Assert(ret != null);
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


    private MonsterProjectileController DequeOrMakeProjectile(Queue<GameObject> q)
    {
        MonsterProjectileController retProjectile;
        if (q.Count > 0)
        {
            retProjectile = q.Dequeue().GetComponent<MonsterProjectileController>();
        }
        else
        {
            retProjectile = MakeProjectile();
        }
        retProjectile.Init();
        Debug.Assert(retProjectile != null);
        return retProjectile;
    }

    private void EnqueOrDestroy(GameObject go, EMonsterProjectileType eType)
    {
        if (_monsterProjectiles.Count > MAX_BOMB_COUNT)
        {
            Debug.Assert(false);
            Object.Destroy(go);
        }
        else
        {
            _monsterProjectiles.Enqueue(go);
        }
    }
    private MonsterProjectileController MakeProjectile()
    {
        GameObject go = null;
        go = Object.Instantiate(_oriMonsterProjectile);
        Debug.Assert(go != null);
        Object.DontDestroyOnLoad(go);
        MonsterProjectileController retProjectile = go.GetComponent<MonsterProjectileController>();
        return retProjectile;
    }
}
