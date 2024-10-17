using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager
{
    public Dictionary<int, data.PlayerStat> PlayerStatDict { get; private set; } = new();
    public Dictionary<int, data.SkillInfo> SkillInfoDict { get; private set; } = new();
    public Dictionary<int, data.MonsterStat> MonsterStatDict { get; private set; } = new();
    public Dictionary<int, data.HealingPotionInfo> HealingPotionDict { get; private set; } = new();

    public Dictionary<int, data.HelmetInfo> HelmetItemDict { get; private set; } = new();
    public Dictionary<int, data.SwordInfo> SwordItemDict { get; private set; } = new();
    public Dictionary<int, data.ArmorInfo> ArmorItemDict { get; private set; } = new();



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

    public const string SFX_MENU_MOVE_PATH = "Sound/SFX_UIMenuMove";
    public const string SFX_MENU_CHOICE_PATH = "Sound/SFX_UIMenuChoice";


    public const string SFX_SKILL_BLACK_FLAME_PATH = "Sound/SFX_SkillBlackFlame";
    public const string SFX_SKILL_SWORD_STRIKE_PATH = "Sound/SFX_SkillSwordStrike";
    public const string SFX_SKILL_SPAWN_REAPER_PATH = "Sound/SFX_SpawnReaper";


    public const string SFX_MONSTER_SWING_1_PATH = "Sound/SFX_MonsterSwing1";
    public const string SFX_MONSTER_SWING_2_PATH = "Sound/SFX_MonsterSwing2";
    public const string SFX_MONSTER_ARCHER_LUANCH_PATH = "Sound/SFX_ArcherLaunch";
    public const string SFX_MONSTER_GUNNER_LUANCH_PATH = "Sound/SFX_GunnerLaunch";
    public const string SFX_MONSTER_FLAMER_ATTACK_PATH = "Sound/SFX_FlamerAttack";
    public const string SFX_MONSTER_BLASTER_WARDEN_ATTACK_PATH = "Sound/SFX_BlasterWadenAttack";
    public const string SFX_MONSTER_RED_GHOUL_ATTACK_PATH = "Sound/SFX_RedGhoulAttack";



    public void Init()
    {
        PlayerStatDict = LoadJson<data.PlayerStatLoader, int, data.PlayerStat>("Player/Data_PlayerStat").MakeDict();
        MonsterStatDict = LoadJson<data.MonsterStatLoader, int, data.MonsterStat>("Monsters/Data_MonstersStat").MakeDict();
        SkillInfoDict = LoadJson<data.SkillInfoLoader, int, data.SkillInfo>("Skill/Data_SkillInfo").MakeDict();
        HealingPotionDict = LoadJson<data.HealingPotionLoader, int, data.HealingPotionInfo>("Item/Data_HealingPotionStat").MakeDict();

        HelmetItemDict = LoadJson<data.HelmetLoader, int, data.HelmetInfo>("Item/Data_HelmetStat").MakeDict();
        SwordItemDict = LoadJson<data.SwordLoader, int, data.SwordInfo>("Item/Data_SwordStat").MakeDict();
        ArmorItemDict = LoadJson<data.ArmorLoader, int, data.ArmorInfo>("Item/Data_ArmorStat").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
