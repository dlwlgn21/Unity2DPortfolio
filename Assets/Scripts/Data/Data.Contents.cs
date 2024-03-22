using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace data
{
    #region PlayerStat
    [Serializable]
    public class PlayerStat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int defence;
        public int moveSpeed;
        public int totalExp;
    }
    [Serializable]
    public class PlayerStatData : ILoader<int, PlayerStat>
    { 
        public List<PlayerStat> playerStats = new List<PlayerStat>();

        public Dictionary<int, PlayerStat> MakeDict()
        {
            Dictionary<int, PlayerStat> dict = new Dictionary<int, PlayerStat>();
            foreach (PlayerStat stat in playerStats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }
    }
    #endregion

    #region MonstersStat
    [Serializable]
    public class MonsterStat
    {
        public int monsterType;
        public int maxHp;
        public int attack;
        public int defence;
        public int moveSpeed;
        public int exp;
    }
    [Serializable]
    public class MonsterStatData : ILoader<int, MonsterStat>
    {
        public List<MonsterStat> monsterStats = new List<MonsterStat>();

        public Dictionary<int, MonsterStat> MakeDict()
        {
            Dictionary<int, MonsterStat> dict = new Dictionary<int, MonsterStat>();
            foreach (MonsterStat stat in monsterStats)
            {
                dict.Add(stat.monsterType, stat);
            }
            return dict;
        }
    }
    #endregion
}
