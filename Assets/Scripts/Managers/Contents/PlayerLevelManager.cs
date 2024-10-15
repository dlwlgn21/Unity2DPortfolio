using data;
using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLevelManager
{
    PlayerStat _playerStat;
    PlayerController _pc;
    public int CurrSkillPoint { get; private set; }

    public const int MAX_SKILL_LEVEL = 3;
    public void Init()
    {
        if (_pc == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.Assert(player != null);
            _pc = player.GetComponent<PlayerController>();
            _playerStat = player.GetComponent<PlayerStat>();
            _playerStat.Init();
            #region EventSubscribe
            PlayerStat.OnLevelUpEventHandler -= OnPlayerLevelUp;
            PlayerStat.OnLevelUpEventHandler += OnPlayerLevelUp;
            UI_Skill_Icon.OnSkillLevelUpEventHandler -= OnPlayerSkillLevelUp;
            UI_Skill_Icon.OnSkillLevelUpEventHandler += OnPlayerSkillLevelUp;
            #endregion
        }
    }


    public void AddExp(int exp)
    {
        _playerStat.Exp += exp;
    }

    void OnPlayerLevelUp(int levelUpCount)
    {
        CurrSkillPoint += levelUpCount;
        Managers.UI.SetSkillPointText(CurrSkillPoint);
    }

    void OnPlayerSkillLevelUp(ESkillType eType)
    {
        --CurrSkillPoint;
        CurrSkillPoint = Mathf.Max(0, CurrSkillPoint);
        Managers.UI.SetSkillPointText(CurrSkillPoint);
    }

    public void Clear()
    {
    }
}
