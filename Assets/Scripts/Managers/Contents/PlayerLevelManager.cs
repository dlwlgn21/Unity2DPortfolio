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
    public int CurrSkillPoint { get; private set; }

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

    void OnPlayerLevelUp(int levelUpCount)
    {
        CurrSkillPoint += levelUpCount;
        _levelUpLightController.TurnOnLight();
        _levelUpLightController.TurnOffLightGraduallyAfterSecond(LEVEL_UP_LIGHT_LIFE_TIME);
        Managers.UI.SetSkillPointText(CurrSkillPoint);
    }


    public void Clear()
    {
        monster_states.Die.MonsterDieEventHandelr -= OnMonsterDied;
    }
}
