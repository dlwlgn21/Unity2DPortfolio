using UnityEngine;
using Cinemachine;
using JetBrains.Annotations;

public enum ECamShakeType
{
    PLAYER_HITTED_BY_MONSTER,
    PLAYER_BLOCK_SUCCES,
    MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK,
    MONSTER_HITTED_BY_KNOCKBACK_BOMB,
    MONSTER_HITTED_BY_REAPER_ATTACK,
}

public class CamShakeManager
{
    private GameObject _camShakeManager;
    private CinemachineImpulseSource _playerImpulseSource;
    private CinemachineImpulseSource _monsterImpulseSource;


    private const float PLAYER_HITTED_BY_MONSTER_FORCE = 0.7f;
    private const float PLAYER_BLOCK_SUCCES_FORCE = 0.8f;
    private const float MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_FORCE = 1f;
    private const float MONSTER_HITTED_BY_KNOCKBACK_BOMB_FORCE = 0.9f;
    private const float MONSTER_HITTED_BY_REAPER_ATTACK_FORCE = 1.5f;
    public void Init()
    {
        if (GameObject.Find("CamShakeManager") == null)
        {
            _camShakeManager = Managers.Resources.Load<GameObject>("Prefabs/Managers/CamShakeManager");
            _camShakeManager.name = "CamShakeManager";
            _playerImpulseSource = Utill.GetComponentInChildrenOrNull<CinemachineImpulseSource>(_camShakeManager, "PlayerImpulseSource");
            _monsterImpulseSource = Utill.GetComponentInChildrenOrNull<CinemachineImpulseSource>(_camShakeManager, "MonsterImpulseSource");
            Object.DontDestroyOnLoad(_camShakeManager);
        }
    }
    public void CamShake(ECamShakeType eType)
    {
        switch (eType)
        {
            case ECamShakeType.PLAYER_HITTED_BY_MONSTER:
                _playerImpulseSource.GenerateImpulseWithForce(PLAYER_HITTED_BY_MONSTER_FORCE);
                break;
            case ECamShakeType.PLAYER_BLOCK_SUCCES:
                _monsterImpulseSource.GenerateImpulseWithForce(PLAYER_BLOCK_SUCCES_FORCE);
                break;
            case ECamShakeType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK:
                _monsterImpulseSource.GenerateImpulseWithForce(MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK_FORCE);
                break;
            case ECamShakeType.MONSTER_HITTED_BY_KNOCKBACK_BOMB:
                _monsterImpulseSource.GenerateImpulseWithForce(MONSTER_HITTED_BY_KNOCKBACK_BOMB_FORCE);
                break;
            case ECamShakeType.MONSTER_HITTED_BY_REAPER_ATTACK:
                _monsterImpulseSource.GenerateImpulseWithForce(MONSTER_HITTED_BY_REAPER_ATTACK_FORCE);
                break;
        }
    }

}
