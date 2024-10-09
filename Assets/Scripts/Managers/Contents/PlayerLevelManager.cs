using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLevelManager
{
    PlayerStat _playerStat;
    PlayerController _pc;
    LightController _levelUpLightController;
    int _currSkillPoint;

    private const float LEVEL_UP_LIGHT_LIFE_TIME = 4f;
    public void Init()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(player != null);
        _pc = player.GetComponent<PlayerController>();
        _playerStat = player.GetComponent<PlayerStat>();
        _levelUpLightController = Utill.GetComponentInChildrenOrNull<LightController>(player, "LevelUpLight");
        _playerStat.Init();
        monster_states.Die.MonsterDieEventHandelr -= OnMonsterDied;
        monster_states.Die.MonsterDieEventHandelr += OnMonsterDied;
        PlayerStat.OnLevelUpEventHandler -= OnPlayerLevelUp;
        PlayerStat.OnLevelUpEventHandler += OnPlayerLevelUp;
    }


    void OnMonsterDied(NormalMonsterController mc)
    {
        _playerStat.Exp += mc.Stat.Exp;
    }

    void OnPlayerLevelUp()
    {
        Debug.Log($"-----Player Level Up!!!!!-----{_playerStat.Level}");
        _levelUpLightController.TurnOnLight();
        _levelUpLightController.TurnOffLightGraduallyAfterSecond(LEVEL_UP_LIGHT_LIFE_TIME);
    }


    public void Clear()
    {
        monster_states.Die.MonsterDieEventHandelr -= OnMonsterDied;
    }
}
