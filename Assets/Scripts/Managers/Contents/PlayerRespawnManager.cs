using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRespawnManager
{
    PlayerController _pc;
    Vector2 _spawnPos;
    public void Init()
    {
        if (_pc == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                _pc = player.GetComponent<PlayerController>();
            else
                _pc = Managers.Resources.Instantiate<PlayerController>("Prefabs/Player/Player");
        }
        GameObject go = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        if (go != null)
        {
            _spawnPos = go.transform.position;
            _spawnPos.y += _spawnPos.y + 2f;
        }
    }

    public void SpawnPlayer(bool isRespawn)
    {
        _pc.Stat.InitHP();
        _pc.ChangeState(EPlayerState.Idle);
        _pc.transform.position = _spawnPos;
    }
}
