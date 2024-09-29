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
    public class Item
    {
        public string name;
        public string description;
        public string iconSpritePath;
    }

    #region HealingPotion
    [Serializable]
    public class HealingPotionStat : Item
    {
        public int id;
        public int level;
        public int healAmount;
    }
    [Serializable]
    public class HealingPotionLoader : ILoader<int, HealingPotionStat>
    {
        public List<HealingPotionStat> healingPotionStats = new();

        public Dictionary<int, HealingPotionStat> MakeDict()
        {
            Dictionary<int, HealingPotionStat> dict = new();
            foreach (HealingPotionStat potion in healingPotionStats)
            {
                dict.Add(potion.id, potion);
            }
            return dict;
        }
    }
    #endregion


    #region Sword
    [Serializable]
    public class SwordStat : Item
    {
        public int id;
        public int damage;
    }
    [Serializable]
    public class SwordLoader : ILoader<int, SwordStat>
    {
        public List<SwordStat> swordStats = new();

        public Dictionary<int, SwordStat> MakeDict()
        {
            Dictionary<int, SwordStat> dict = new();
            foreach (SwordStat sword in swordStats)
            {
                dict.Add(sword.id, sword);
            }
            return dict;
        }
    }
    #endregion

    #region Armor
    [Serializable]
    public class ArmorStat : Item
    {
        public int id;
        public int defence;
    }
    [Serializable]
    public class ArmorLoader : ILoader<int, ArmorStat>
    {
        public List<ArmorStat> armorStats = new();

        public Dictionary<int, ArmorStat> MakeDict()
        {
            Dictionary<int, ArmorStat> dict = new();
            foreach (ArmorStat armor in armorStats)
            {
                dict.Add(armor.id, armor);
            }
            return dict;
        }
    }
    #endregion

    #region Helmet
    [Serializable]
    public class HelmetStat : Item
    {
        public int id;
        public int defence;
    }
    [Serializable]
    public class HelmetLoader : ILoader<int, HelmetStat>
    {
        public List<HelmetStat> helmetStats = new();

        public Dictionary<int, HelmetStat> MakeDict()
        {
            Dictionary<int, HelmetStat> dict = new();
            foreach (HelmetStat helmet in helmetStats)
            {
                dict.Add(helmet.id, helmet);
            }
            return dict;
        }
    }
    #endregion
}