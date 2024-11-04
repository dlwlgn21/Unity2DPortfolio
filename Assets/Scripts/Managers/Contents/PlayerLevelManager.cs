using data;
using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class PlayerLevelManager
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
            PlayerStat.LevelUpEventHandler -= OnPlayerLevelUp;
            PlayerStat.LevelUpEventHandler += OnPlayerLevelUp;
            UI_Skill_Icon.SkillLevelUpEventHandler -= OnPlayerSkillLevelUp;
            UI_Skill_Icon.SkillLevelUpEventHandler += OnPlayerSkillLevelUp;
            monster_states.Die.DieEventEnterStateHandler -= OnMonsterDied;
            monster_states.Die.DieEventEnterStateHandler += OnMonsterDied;
            #endregion
        }
    }

    void OnMonsterDied(NormalMonsterController mc)
    {
        _playerStat.Exp += mc.Stat.Exp;
    }
    public void AddExp(int exp)
    {
        _playerStat.Exp += exp;
    }

    void OnPlayerLevelUp(int levelUpCount)
    {
        CurrSkillPoint += levelUpCount;
        Managers.UI.SetSkillPointText(CurrSkillPoint);
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_PLAYER_LEVEL_UP);
    }

    void OnPlayerSkillLevelUp(EActiveSkillType eType, int skillLevel)
    {
        --CurrSkillPoint;
        CurrSkillPoint = Mathf.Max(0, CurrSkillPoint);
        Managers.UI.SetSkillPointText(CurrSkillPoint);
    }

    public void Clear()
    {
    }
}
