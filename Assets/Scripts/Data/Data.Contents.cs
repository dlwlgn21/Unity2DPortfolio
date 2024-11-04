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
        public int maxMana;
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
        public float knockbackForceX;
        public float knockbackForceY;
        public float slowTimeInSec;
        public float burnTimeInSec;
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
        public int level;
        public int attackType;
        public float coolTime;
        public float parallysisTime;
        public float knockbackForceX;
        public float knockbackForceY;
        public int manaCost;
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

    [Serializable]
    public class SfxKeyContainer
    {
        public string SFX_BGM_TUTORIAL;
        public string SFX_BGM_ABANDON_ROAD;
        public string SFX_BGM_CAVE_COLOSSAL;
        public string SFX_BGM_COLOSSAL_BATTLE;
        public string SFX_UI_POINTER_ENTER;
        public string SFX_UI_DENIED;
        public string SFX_UI_DROP_OR_ITEM_GET_SUCESS;
        public string SFX_UI_EQUP_SUCESS;
        public string SFX_UI_SKILL_LEVEL_UP;
        public string SFX_UI_DIALOG_BOX_POPUP;
        public string SFX_UI_DIALOG_START;
        public string SFX_TELEPORT;
        public string SFX_QUEST_SUCESS;
        public string SFX_PALY_SCENE_BGM_PATH;
        public string SFX_PLAYER_SWING_1_PATH;
        public string SFX_PLAYER_SWING_2_PATH;
        public string SFX_PLAYER_SWING_3_PATH;
        public string SFX_PLAYER_ROLLING_PATH;
        public string SFX_PLAYER_HIT_1_PATH;
        public string SFX_PLAYER_HIT_2_PATH;
        public string SFX_PLAYER_FOOT_STEP_1_PATH;
        public string SFX_PLAYER_FOOT_STEP_2_PATH;
        public string SFX_PLAYER_JUMP_PATH;
        public string SFX_PLAYER_LAND_PATH;
        public string SFX_PLAYER_BACKATTACK_PATH;
        public string SFX_PLAYER_BLOCK_SUCESS_PATH;
        public string SFX_PLAYER_FLY_USING_SKILL;
        public string SFX_PLAYER_LEVEL_UP;
        public string SFX_PLAYER_HEALD;
        public string SFX_MENU_MOVE_PATH;
        public string SFX_MENU_CHOICE_PATH;
        public string SFX_SKILL_BLACK_FLAME_PATH;
        public string SFX_SKILL_SWORD_STRIKE_PATH;
        public string SFX_SKILL_SPAWN_REAPER_PATH;
        public string SFX_SKILL_SPAWN_SHOOTER_SHOOT;
        public string SFX_SKILL_KNOCKBACK_PROJECTILE_DROPED;
        public string SFX_SKILL_KNOCKBACK_PROJECTILE_BOMB;
        public string SFX_MONSTER_SWING_1_PATH;
        public string SFX_MONSTER_SWING_2_PATH;
        public string SFX_MONSTER_ARCHER_GUNNER_LUANCH_PATH;
        public string SFX_MONSTER_FLAMER_ATTACK_PATH;
        public string SFX_MONSTER_BLASTER_WARDEN_ATTACK_PATH;
        public string SFX_MONSTER_RED_GHOUL_ATTACK_PATH;
        public string SFX_MONSTER_DIE_EXPOLOSION_1;
        public string SFX_MONSTER_DIE_EXPOLOSION_2;
        public string SFX_MONSTER_DIE_EXPOLOSION_3;
        public string SFX_MONSTER_DIE_EXPOLOSION_4;
        public string SFX_MONSTER_HIT_BY_NORMAL_ATTACK_1;
        public string SFX_MONSTER_HIT_BY_NORMAL_ATTACK_2;
        public string SFX_MONSTER_HIT_BY_NORMAL_ATTACK_3;
        public string SFX_MONSTER_HIT_BY_PLAYER_CAST_SKILL;
        public string SFX_MONSTER_HIT_BY_PLAYER_SKILL_REAPER;
        public string SFX_MONSTER_PROJECTILE_HIT;
        public string SFX_ENV_DOOR_OPEN;
    }

    [Serializable]
    public class PlayerFigureContainer
    {
        public float NormalAttackKnockbackForceX;
        public float NormalAttackKnockbackForceY;
        public float NormalAttack1DashForceX;
        public float NormalAttack1DashForceY;
        public float BlockSuccessKnockbackForceX;
        public float BlockSuccessKnockbackForceY;
        public float BigAttackForceCoeff;
        public float NormalAttack2DamageCoeff;
        public float NormalAttack3DamageCoeff;
        public float BackAttackDamageCoeff;
    }
}