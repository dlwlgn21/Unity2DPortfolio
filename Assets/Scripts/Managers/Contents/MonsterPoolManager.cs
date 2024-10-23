using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public sealed class MonsterPoolManager
{
    public const int MAX_POOL_MONSTER_COUNT = 5;
    private Queue<GameObject> _archers      = new(MAX_POOL_MONSTER_COUNT); 
    private Queue<GameObject> _wardens      = new(MAX_POOL_MONSTER_COUNT); 
    private Queue<GameObject> _gunners      = new(MAX_POOL_MONSTER_COUNT); 
    private Queue<GameObject> _cagedShokers = new(MAX_POOL_MONSTER_COUNT); 
    private Queue<GameObject> _redGhouls    = new(MAX_POOL_MONSTER_COUNT); 
    private Queue<GameObject> _blasters     = new(MAX_POOL_MONSTER_COUNT); 
    private Queue<GameObject> _hSlicers     = new(MAX_POOL_MONSTER_COUNT);
    //private Queue<GameObject> _shielders    = new Queue<GameObject>(MAX_MONSTER_COUNT);
    private Queue<GameObject> _flamers      = new(MAX_POOL_MONSTER_COUNT);

    private GameObject _oriArcher;
    private GameObject _oriWarden;
    private GameObject _oriGunner;
    private GameObject _oriBlaster;
    private GameObject _oriRedGhoul;
    private GameObject _oriCagedShokcer;
    private GameObject _oriHSlicer;
    //private GameObject _oriShielder;
    private GameObject _oriFlamer;
    int monId = 0;
    public int MonsterCountInCurrScene { get; private set; }
    public void Init()
    {
        if (_oriWarden == null)
        {
            _oriWarden = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonWarden");
            _oriGunner = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonGunner");
            _oriBlaster = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonBlaster");
            _oriRedGhoul = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonRedGhoul");
            _oriCagedShokcer = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonCagedShoker");
            _oriHSlicer = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonHSlicer");
            _oriArcher = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonArcher");
            //_oriShielder = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonShielder");
            _oriFlamer = Managers.Resources.Load<GameObject>("Prefabs/Monsters/MonFlamer");
            monster_states.Die.DieEventAnimFullyPlayedHandler -= OnMonsterDied;
            monster_states.Die.DieEventAnimFullyPlayedHandler += OnMonsterDied;
        }
    }

    public void Clear()
    {
    }

    void OnMonsterDied(NormalMonsterController mc)
    {
        Return(mc);
    }

    public GameObject Get(EMonsterNames eMonName, Vector2 spawnPos)
    {
        GameObject retGo = null;
        switch (eMonName)
        {
            case EMonsterNames.Archer:
                retGo = DequeOrMakeMonster(_archers, _oriArcher, spawnPos);
                break;
            case EMonsterNames.Blaster:
                retGo = DequeOrMakeMonster(_blasters, _oriBlaster, spawnPos);
                break;
            case EMonsterNames.CagedShoker:
                retGo = DequeOrMakeMonster(_cagedShokers, _oriCagedShokcer, spawnPos);
                break;
            case EMonsterNames.RedGhoul:
                retGo = DequeOrMakeMonster(_redGhouls, _oriRedGhoul, spawnPos);
                break;
            case EMonsterNames.HeabySlicer:
                retGo = DequeOrMakeMonster(_hSlicers, _oriHSlicer, spawnPos);
                break;
            case EMonsterNames.Gunner:
                retGo = DequeOrMakeMonster(_gunners, _oriGunner, spawnPos);
                break;
            case EMonsterNames.Shielder:
                //retGo = DequeOrMakeMonster(_shielders, _oriShielder, spawnPos);
                break;
            case EMonsterNames.Warden:
                retGo = DequeOrMakeMonster(_wardens, _oriWarden, spawnPos);
                break;
            case EMonsterNames.Flamer:
                retGo = DequeOrMakeMonster(_flamers, _oriFlamer, spawnPos);
                break;
        }
        Debug.Assert(retGo != null);
        InitForRespawn(retGo, spawnPos);
        MonsterCountInCurrScene += 1;
        return retGo;
    }

    public void Return(BaseMonsterController mc)
    {
        mc.gameObject.SetActive(false);
        MonsterCountInCurrScene -= 1;
        if (MonsterCountInCurrScene < 0)
            Debug.DebugBreak();
        switch (mc.EMonsterType)
        {
            case EMonsterNames.Archer:
                DestroyOrEnque(_archers, mc.gameObject);
                break;
            case EMonsterNames.Blaster:
                DestroyOrEnque(_blasters, mc.gameObject);
                break;
            case EMonsterNames.CagedShoker:
                DestroyOrEnque(_cagedShokers, mc.gameObject);
                break;
            case EMonsterNames.RedGhoul:
                DestroyOrEnque(_redGhouls, mc.gameObject);
                break;
            case EMonsterNames.HeabySlicer:
                DestroyOrEnque(_hSlicers, mc.gameObject);
                break;
            case EMonsterNames.Gunner:
                DestroyOrEnque(_gunners, mc.gameObject);
                break;
            case EMonsterNames.Shielder:
                //DestroyOrEnque(_shielders, mc.gameObject);
                Debug.Assert(false);
                break;
            case EMonsterNames.Warden:
                DestroyOrEnque(_wardens, mc.gameObject);
                break;
            case EMonsterNames.Flamer:
                DestroyOrEnque(_flamers, mc.gameObject);
                break;
        }
    }

    private GameObject MakeMonsters(GameObject original, Vector2 spawnPos)
    {
        GameObject go = Object.Instantiate(original, spawnPos, Quaternion.identity);
        Object.DontDestroyOnLoad(go);
        return go;
    }

    private void InitForRespawn(GameObject go, Vector2 spawnPos)
    {
        Debug.Assert(go != null);
        go.transform.position = spawnPos;
        go.SetActive(true);
        NormalMonsterController mc = go.GetComponent<NormalMonsterController>();
        mc.InitForRespawn();
    }

    private void DestroyOrEnque(Queue<GameObject> q, GameObject go)
    {
        if (q.Count > MAX_POOL_MONSTER_COUNT)
        {
            Debug.Assert(false);
            Object.Destroy(go);
        }
        else
        {
            q.Enqueue(go);
        }
    }

    private GameObject DequeOrMakeMonster(Queue<GameObject> q, GameObject oriGo, Vector2 spawnPos)
    {
        GameObject retGo = null;
        if (q.Count > 0)
        {
            retGo = q.Dequeue();
        }
        else
        {
            retGo = MakeMonsters(oriGo, spawnPos);
            retGo.name = $"{retGo.name.Substring(0, retGo.name.Length - 7)}{monId++}";
        }
        Debug.Assert(retGo != null);
        return retGo;
    }
}
