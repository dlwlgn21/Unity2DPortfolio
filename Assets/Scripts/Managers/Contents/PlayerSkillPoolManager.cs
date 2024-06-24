using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillPoolManager
{
    public const int MAX_BOMB_COUNT = 5;

    private Queue<GameObject> _bombs = new Queue<GameObject>(MAX_BOMB_COUNT);

    private GameObject _oriBomb;

    public void Init()
    {
        if (_oriBomb == null)
        {
            _oriBomb = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/Projectile");
        }
    }

    public GameObject GetKnockbackBoom(Vector2 spawnPos)
    {
        GameObject retGo = null;
        if (_bombs.Count > 0)
        {
            retGo = _bombs.Dequeue();
            retGo.GetComponent<PlayerSkillKnockbackBoomObject>().Init(spawnPos);
        }
        else
        {
            retGo = MakeBomb(_oriBomb, spawnPos);
        }
        Debug.Assert(retGo != null);
        retGo.transform.position = spawnPos;
        retGo.SetActive(true);
        return retGo;
    }

    public void ReturnKnockbackBoom(GameObject go)
    {
        go.SetActive(false);
        if (_bombs.Count > MAX_BOMB_COUNT)
        {
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
}
