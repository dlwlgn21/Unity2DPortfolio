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
    public class PlayerStatLoader : ILoader<int, PlayerStat>
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
        public int monsterStatusEffect;
        public int maxHp;
        public int attack;
        public int defence;
        public int moveSpeed;
        public int exp;
    }
    [Serializable]
    public class MonsterStatLoader : ILoader<int, MonsterStat>
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


    [Serializable]
    public class ItemInfoData
    {
        public string name;
        public string description;
        public string iconSpritePath;
    }

    #region HealingPotion
    [Serializable]
    public class HealingPotionInfo : ItemInfoData
    {
        public int id;
        public int level;
        public int healAmount;
    }
    [Serializable]
    public class HealingPotionLoader : ILoader<int, HealingPotionInfo>
    {
        public List<HealingPotionInfo> healingPotionStats = new();

        public Dictionary<int, HealingPotionInfo> MakeDict()
        {
            Dictionary<int, HealingPotionInfo> dict = new();
            foreach (HealingPotionInfo potion in healingPotionStats)
            {
                dict.Add(potion.id, potion);
            }
            return dict;
        }
    }
    #endregion


    #region Sword
    [Serializable]
    public class SwordInfo : ItemInfoData
    {
        public int id;
        public int damage;
    }
    [Serializable]
    public class SwordLoader : ILoader<int, SwordInfo>
    {
        public List<SwordInfo> swordStats = new();

        public Dictionary<int, SwordInfo> MakeDict()
        {
            Dictionary<int, SwordInfo> dict = new();
            foreach (SwordInfo sword in swordStats)
            {
                dict.Add(sword.id, sword);
            }
            return dict;
        }
    }
    #endregion

    #region Armor
    [Serializable]
    public class ArmorInfo : ItemInfoData
    {
        public int id;
        public int defence;
    }
    [Serializable]
    public class ArmorLoader : ILoader<int, ArmorInfo>
    {
        public List<ArmorInfo> armorStats = new();

        public Dictionary<int, ArmorInfo> MakeDict()
        {
            Dictionary<int, ArmorInfo> dict = new();
            foreach (ArmorInfo armor in armorStats)
            {
                dict.Add(armor.id, armor);
            }
            return dict;
        }
    }
    #endregion

    #region Helmet
    [Serializable]
    public class HelmetInfo : ItemInfoData
    {
        public int id;
        public int defence;
    }
    [Serializable]
    public class HelmetLoader : ILoader<int, HelmetInfo>
    {
        public List<HelmetInfo> helmetStats = new();

        public Dictionary<int, HelmetInfo> MakeDict()
        {
            Dictionary<int, HelmetInfo> dict = new();
            foreach (HelmetInfo helmet in helmetStats)
            {
                dict.Add(helmet.id, helmet);
            }
            return dict;
        }
    }
    #endregion
}