using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager
{
    private Vector2 _spawnPos;
    public void Init()
    {
        GameObject go = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        if (go != null)
        {
            _spawnPos = go.transform.position;
            _spawnPos.y += _spawnPos.y + 2f;
        }
    }

    public void SpawnPlayer(PlayerController pc)
    {
        pc.Stat.InitHP();
        pc.ChangeState(EPlayerState.IDLE);
        pc.transform.position = _spawnPos;
    }
}
