using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Rendering;

public class MonsterPoolManager
{
    public const int MAX_MONSTER_COUNT = 5;
    private Queue<GameObject> _wardens = new Queue<GameObject>(MAX_MONSTER_COUNT); 
    private Queue<GameObject> _cagedShokers = new Queue<GameObject>(MAX_MONSTER_COUNT); 
    private Queue<GameObject> _blasters = new Queue<GameObject>(MAX_MONSTER_COUNT); 
    private Queue<GameObject> _hSlicers = new Queue<GameObject>(MAX_MONSTER_COUNT);

    private GameObject _oriWarden;
    private GameObject _oriBlaster;
    private GameObject _oriCagedShokcer;
    private GameObject _oriHSlicer;
    
    public void Init()
    {
        _oriWarden = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonWarden");
        _oriBlaster = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonBlaster");
        _oriCagedShokcer = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonCagedShoker");
        _oriHSlicer = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonHSlicer");
    }

    public GameObject Get(EMonsterNames eMonName, Vector2 spawnPos)
    {
        GameObject retGo = null;
        switch (eMonName)
        {
            case EMonsterNames.Archer:
                break;
            case EMonsterNames.Blaster:
                if (_blasters.Count > 0)
                {
                    retGo = _blasters.Dequeue();
                    retGo.GetComponent<BaseMonsterController>().InitStatForRespawn();
                }
                else
                {
                    retGo = MakeMonsters(_oriBlaster, spawnPos);
                }
                break;
            case EMonsterNames.CagedShoker:
                if (_cagedShokers.Count > 0)
                {
                    retGo = _blasters.Dequeue();
                    retGo.GetComponent<BaseMonsterController>().InitStatForRespawn();
                }
                else
                {
                    retGo = MakeMonsters(_oriCagedShokcer, spawnPos);
                }
                break;
            case EMonsterNames.Dagger:
                break;
            case EMonsterNames.HeabySlicer:
                if (_hSlicers.Count > 0)
                {
                    retGo = _hSlicers.Dequeue();
                    retGo.GetComponent<BaseMonsterController>().InitStatForRespawn();
                }
                else
                {
                    retGo = MakeMonsters(_oriHSlicer, spawnPos);
                }
                break;
            case EMonsterNames.LightSlicer:
                break;
            case EMonsterNames.Sweeper:
                break;
            case EMonsterNames.Warden:
                if (_hSlicers.Count > 0)
                {
                    retGo = _wardens.Dequeue();
                    retGo.GetComponent<BaseMonsterController>().InitStatForRespawn();
                }
                else
                {
                    retGo = MakeMonsters(_oriWarden, spawnPos);
                }
                break;
        }
        Debug.Assert(retGo != null);
        retGo.transform.position = spawnPos;
        retGo.SetActive(true);
        return retGo;
    }

    public void Return(EMonsterNames eMonName, GameObject go)
    {
        go.SetActive(false);
        switch (eMonName)
        {
            case EMonsterNames.Archer:
                break;
            case EMonsterNames.Blaster:
                if (_blasters.Count > MAX_MONSTER_COUNT)
                    Object.Destroy(go);
                else
                    _blasters.Enqueue(go);
                break;
            case EMonsterNames.CagedShoker:
                if (_cagedShokers.Count > MAX_MONSTER_COUNT)
                    Object.Destroy(go);
                else
                    _cagedShokers.Enqueue(go);
                break;
            case EMonsterNames.Dagger:
                break;
            case EMonsterNames.HeabySlicer:
                if (_hSlicers.Count > MAX_MONSTER_COUNT)
                    Object.Destroy(go);
                else
                    _hSlicers.Enqueue(go);
                break;
            case EMonsterNames.LightSlicer:
                break;
            case EMonsterNames.Sweeper:
                break;
            case EMonsterNames.Warden:
                if (_wardens.Count > MAX_MONSTER_COUNT)
                    Object.Destroy(go);
                else
                    _wardens.Enqueue(go);
                break;
        }
    }

    

    private GameObject MakeMonsters(GameObject original, Vector2 spawnPos)
    {
        GameObject go = Object.Instantiate(original, spawnPos, Quaternion.identity);
        Object.DontDestroyOnLoad(go);
        return go;
    }

}
