using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager
{
    public Dictionary<int, data.PlayerStat> PlayerStatDict { get; private set; } = new();
    public Dictionary<define.EActiveSkillType, List<data.SkillInfo>> ActiveSkillInfoDict { get; private set; } = new();
    public Dictionary<int, data.MonsterStat> MonsterStatDict { get; private set; } = new();
    public Dictionary<int, data.HealingPotionInfo> HealingPotionDict { get; private set; } = new();
    public Dictionary<int, data.HelmetInfo> HelmetItemDict { get; private set; } = new();
    public Dictionary<int, data.SwordInfo> SwordItemDict { get; private set; } = new();
    public Dictionary<int, data.ArmorInfo> ArmorItemDict { get; private set; } = new();
    Dictionary<int, data.SkillInfo> _skillInfoDict = new();

    public const string SKILL_SPAWN_REAPER_KEY = "SpawnReaper";
    public const string SKILL_SPAWN_SHOOTER_KEY = "SpawnShooter";
    public const string SKILL_BLACK_FLAME_KEY = "BlackFlame";
    public const string SKILL_SWROD_STRIKE_KEY = "SwordStrike";

    public const string SFX_BGM_TUTORIAL = "Sound/SFX_BGM_Tutorial";
    public const string SFX_BGM_ABANDON_ROAD = "Sound/SFX_BGM_AbondonRoad";
    public const string SFX_BGM_CAVE_COLOSSAL = "Sound/SFX_BGM_CaveColossal";
    public const string SFX_BGM_COLOSSAL_BATTLE = "Sound/SFX_BFG_BossColossal";

    public const string SFX_UI_POINTER_ENTER = "Sound/SFX_UI_PointerEnter";
    public const string SFX_UI_DENIED = "Sound/SFX_UI_PointerDenied";
    public const string SFX_UI_DROP_OR_ITEM_GET_SUCESS = "Sound/SFX_UI_DropSucess";
    public const string SFX_UI_EQUP_SUCESS = "Sound/SFX_UI_DropSucess"; // 일단 이렇게 함. 이게 뭔가 통일성이 있어서..
    public const string SFX_UI_SKILL_LEVEL_UP = "Sound/SFX_UI_PlayerSkillLevelUp";
    public const string SFX_UI_DIALOG_BOX_POPUP = "Sound/SFX_UI_DialogPopup";
    public const string SFX_UI_DIALOG_START = "Sound/SFX_UI_DialogStart";
    public const string SFX_TELEPORT = "Sound/SFX_Teleport";
    public const string SFX_QUEST_SUCESS = "Sound/SFX_QuestSucess";

    public const string SFX_PALY_SCENE_BGM_PATH = "Sound/SFX_PlaySceneBgm";

    public const string SFX_PLAYER_SWING_1_PATH = "Sound/SFX_PlayerSwing1";
    public const string SFX_PLAYER_SWING_2_PATH = "Sound/SFX_PlayerSwing2";
    public const string SFX_PLAYER_SWING_3_PATH = "Sound/SFX_PlayerSwing3";
    public const string SFX_PLAYER_ROLLING_PATH = "Sound/SFX_PlayerRolling";
    public const string SFX_PLAYER_HIT_1_PATH = "Sound/SFX_PlayerHit1";
    public const string SFX_PLAYER_HIT_2_PATH = "Sound/SFX_PlayerHit2";
    public const string SFX_PLAYER_FOOT_STEP_1_PATH = "Sound/SFX_PlayerFootStep1";
    public const string SFX_PLAYER_FOOT_STEP_2_PATH = "Sound/SFX_PlayerFootStep2";
    public const string SFX_PLAYER_JUMP_PATH = "Sound/SFX_PlayerJump";
    public const string SFX_PLAYER_LAND_PATH = "Sound/SFX_PlayerLand";
    public const string SFX_PLAYER_BACKATTACK_PATH = "Sound/SFX_PlayerBackAttack";
    public const string SFX_PLAYER_BLOCK_SUCESS_PATH = "Sound/SFX_PlayerBlockSucess";
    public const string SFX_PLAYER_FLY_USING_SKILL = "Sound/SFX_PlayerFlyUsingSkill";
    public const string SFX_PLAYER_LEVEL_UP = "Sound/SFX_PlayerLevelUp";
    public const string SFX_PLAYER_HEALD = "Sound/SFX_PlayerHeald";

    public const string SFX_MENU_MOVE_PATH = "Sound/SFX_UIMenuMove";
    public const string SFX_MENU_CHOICE_PATH = "Sound/SFX_UIMenuChoice";


    public const string SFX_SKILL_BLACK_FLAME_PATH = "Sound/SFX_SkillBlackFlame";
    public const string SFX_SKILL_SWORD_STRIKE_PATH = "Sound/SFX_SkillSwordStrike";
    public const string SFX_SKILL_SPAWN_REAPER_PATH = "Sound/SFX_SpawnReaper";
    public const string SFX_SKILL_SPAWN_SHOOTER_SHOOT = "Sound/SFX_SpawnShooterLaunch";
    public const string SFX_SKILL_KNOCKBACK_PROJECTILE_DROPED = "Sound/SFX_SkillProjectileDroped";
    public const string SFX_SKILL_KNOCKBACK_PROJECTILE_BOMB = "Sound/SFX_SkillProjectileBomb";


    public const string SFX_MONSTER_SWING_1_PATH = "Sound/SFX_MonsterSwing1";
    public const string SFX_MONSTER_SWING_2_PATH = "Sound/SFX_MonsterSwing2";
    public const string SFX_MONSTER_ARCHER_GUNNER_LUANCH_PATH = "Sound/SFX_GunnerLaunch";
    public const string SFX_MONSTER_FLAMER_ATTACK_PATH = "Sound/SFX_FlamerAttack";
    public const string SFX_MONSTER_BLASTER_WARDEN_ATTACK_PATH = "Sound/SFX_BlasterWadenAttack";
    public const string SFX_MONSTER_RED_GHOUL_ATTACK_PATH = "Sound/SFX_RedGhoulAttack";

    public const string SFX_MONSTER_DIE_EXPOLOSION_1 = "Sound/SFX_MonsterDieExpolosion1";
    public const string SFX_MONSTER_DIE_EXPOLOSION_2 = "Sound/SFX_MonsterDieExpolosion2";
    public const string SFX_MONSTER_DIE_EXPOLOSION_3 = "Sound/SFX_MonsterDieExpolosion3";
    public const string SFX_MONSTER_DIE_EXPOLOSION_4 = "Sound/SFX_MonsterDieExpolosion4";
    public const string SFX_MONSTER_HIT_BY_NORMAL_ATTACK_1 = "Sound/SFX_MonsterHit1";
    public const string SFX_MONSTER_HIT_BY_NORMAL_ATTACK_2 = "Sound/SFX_MonsterHit2";
    public const string SFX_MONSTER_HIT_BY_NORMAL_ATTACK_3 = "Sound/SFX_MonsterHit3";
    public const string SFX_MONSTER_HIT_BY_PLAYER_CAST_SKILL = "Sound/SFX_MonsterHitByPlayerSkill";
    public const string SFX_MONSTER_HIT_BY_PLAYER_SKILL_REAPER = "Sound/SFX_MonsterHitByPlayerSkillReaper";
    public const string SFX_MONSTER_PROJECTILE_HIT = "Sound/SFX_MonsterProjectileHit";

    public const string SFX_ENV_DOOR_OPEN = "SFX_DoorOpen";
    public void Init()
    {
        PlayerStatDict = LoadJson<data.PlayerStatLoader, int, data.PlayerStat>("Player/Data_PlayerStat").MakeDict();
        MonsterStatDict = LoadJson<data.MonsterStatLoader, int, data.MonsterStat>("Monsters/Data_MonstersStat").MakeDict();
        _skillInfoDict = LoadJson<data.SkillInfoLoader, int, data.SkillInfo>("Skill/Data_SkillInfo").MakeDict();
        HealingPotionDict = LoadJson<data.HealingPotionLoader, int, data.HealingPotionInfo>("Item/Data_HealingPotionStat").MakeDict();

        HelmetItemDict = LoadJson<data.HelmetLoader, int, data.HelmetInfo>("Item/Data_HelmetStat").MakeDict();
        SwordItemDict = LoadJson<data.SwordLoader, int, data.SwordInfo>("Item/Data_SwordStat").MakeDict();
        ArmorItemDict = LoadJson<data.ArmorLoader, int, data.ArmorInfo>("Item/Data_ArmorStat").MakeDict();

        #region InitActiveSkillDict

        foreach (var info in _skillInfoDict)
        {
            switch (info.Value.animKey)
            {
                case "Roll":
                    AddSkills(define.EActiveSkillType.Roll, info.Value);
                    break;
                case SKILL_SPAWN_REAPER_KEY:
                    AddSkills(define.EActiveSkillType.Spawn_Reaper, info.Value);
                    break;
                case SKILL_SPAWN_SHOOTER_KEY:
                    AddSkills(define.EActiveSkillType.Spawn_Shooter, info.Value);
                    break;
                case SKILL_BLACK_FLAME_KEY:
                    AddSkills(define.EActiveSkillType.Cast_BlackFlame, info.Value);
                    break;
                case SKILL_SWROD_STRIKE_KEY:
                    AddSkills(define.EActiveSkillType.Cast_SwordStrike, info.Value);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    void AddSkills(define.EActiveSkillType eSkillType, data.SkillInfo skillInfo)
    {
        List<data.SkillInfo> skills;
        if (ActiveSkillInfoDict.TryGetValue(eSkillType, out skills) == false)
        {
            ActiveSkillInfoDict.Add(eSkillType, new List<data.SkillInfo>());
            ActiveSkillInfoDict[eSkillType].Add(skillInfo);
        }
        else
        {
            skills.Add(skillInfo);
        }
    }
}
