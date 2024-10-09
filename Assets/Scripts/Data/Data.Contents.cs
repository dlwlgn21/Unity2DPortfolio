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

    #region PlayerSkill
    [Serializable]
    public class SkillInfo
    {
        public string name;
        public string description;
        public int id;
        public int attackType;
        public float coolTime;
        public float parallysisTime;
        public float knockbackForceX;
        public float knockbackForceY;
        public int damage;
        public string objectPrefabPath;
        public string controllerPrefabPath;
        public string animKey;
    }

    [Serializable]
    public class SkillInfoLoader : ILoader<int, SkillInfo>
    {
        public List<SkillInfo> skillInfos = new();

        public Dictionary<int, SkillInfo> MakeDict()
        {
            Dictionary<int, SkillInfo> dict = new();
            foreach (SkillInfo info in skillInfos)
            {
                dict.Add(info.id, info);
            }
            return dict;
        }
    }
    #endregion

    #region items
    [Serializable]
    public class BaseItemInfoData
    {
        public string name;
        public string description;
        public string iconSpritePath;
    }

    #region HealingPotion
    [Serializable]
    public class HealingPotionInfo : BaseItemInfoData
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
    public class SwordInfo : BaseItemInfoData
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
    public class ArmorInfo : BaseItemInfoData
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
    public class HelmetInfo : BaseItemInfoData
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
    #endregion
}