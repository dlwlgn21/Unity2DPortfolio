using define;
using System.Collections;
using UnityEngine;

public sealed class MonsterSpawnManager
{
    public void Init()
    {
        GameObject spawnPoints = GameObject.FindGameObjectWithTag("MonsterSpawnPoint");
        if (spawnPoints != null)
        {
            for (int i = 0; i < spawnPoints.transform.childCount; ++i)
            {
               EMonsterNames eType = spawnPoints.transform.GetChild(i).GetComponent<SpawnMonsterType>().EMonsterType;
               Managers.MonsterPool.Get(eType, spawnPoints.transform.GetChild(i).transform.position);
            }
        }
        else
        {
            Debug.DebugBreak();
        }
    }
}