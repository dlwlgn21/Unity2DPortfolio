using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSkillManager
{
    PlayerSkillSpawnReaper _spawnReaper;
    PlayerSkillSpawnShooter _spawnShooter;
    public void Init()
    {
        if (_spawnReaper == null)
        {
            GameObject spawnReaper = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/SkillSpawnReaper");
            spawnReaper = Object.Instantiate(spawnReaper);
            _spawnReaper = spawnReaper.GetComponent<PlayerSkillSpawnReaper>();
            Object.DontDestroyOnLoad(spawnReaper);

            GameObject spawnShooter = Managers.Resources.Load<GameObject>("Prefabs/Player/Skills/SkillSpawnShooter");
            spawnShooter = Object.Instantiate(spawnShooter);
            _spawnShooter = spawnShooter.GetComponent<PlayerSkillSpawnShooter>();
            Object.DontDestroyOnLoad(spawnShooter);
        }
    }
    public void CastSpawnReaper(Vector2 pos, ECharacterLookDir eLookDir)
    {
        _spawnReaper.SpawnReaper(pos, eLookDir);
    }
    public void CastSpawnShooter(Vector2 pos, ECharacterLookDir eLookDir)
    {
        _spawnShooter.SpawnShooter(pos, eLookDir);
    }
}
