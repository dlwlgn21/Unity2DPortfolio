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
    public Dictionary<int, data.MonsterStat> MonsterStatDict { get; private set; } = new();
    public Dictionary<int, data.HealingPotionStat> HealingPotionDict { get; private set; } = new();

    public Dictionary<int, data.HelmetStat> HelmetItemDict { get; private set; } = new();
    public Dictionary<int, data.SwordStat> SwordItemDict { get; private set; } = new();
    public Dictionary<int, data.ArmorStat> ArmorItemDict { get; private set; } = new();


    public const string SFX_PALY_SCENE_BGM_PATH = "Sound/SFX_PlaySceneBgm";

    public const string SFX_PLAYER_SWING_1_PATH = "Sound/SFX_PlayerSwing1";
    public const string SFX_PLAYER_SWING_2_PATH = "Sound/SFX_PlayerSwing2";
    public const string SFX_PLAYER_SWING_3_PATH = "Sound/SFX_PlayerSwing3";
    public const string SFX_PLAYER_ROLLING_PATH = "Sound/SFX_PlayerRolling";
    public const string SFX_PLAYER_HIT_1_PATH = "Sound/SFX_PlayerHit1";
    public const string SFX_PLAYER_HIT_2_PATH = "Sound/SFX_PlayerHit2";
    public const string SFX_PLAYER_FOOT_STEP_PATH = "Sound/SFX_PlayerFootStep";

    public const string SFX_MENU_MOVE_PATH = "Sound/SFX_UIMenuMove";
    public const string SFX_MENU_CHOICE_PATH = "Sound/SFX_UIMenuChoice";

    public void Init()
    {
        PlayerStatDict = LoadJson<data.PlayerStatLoader, int, data.PlayerStat>("Player/Data_PlayerStat").MakeDict();
        Debug.Assert(PlayerStatDict.Count != 0);
        MonsterStatDict = LoadJson<data.MonsterStatLoader, int, data.MonsterStat>("Monsters/Data_MonstersStat").MakeDict();
        Debug.Assert(MonsterStatDict.Count != 0);
        HealingPotionDict = LoadJson<data.HealingPotionLoader, int, data.HealingPotionStat>("Item/Data_HealingPotionStat").MakeDict();

        HelmetItemDict = LoadJson<data.HelmetLoader, int, data.HelmetStat>("Item/Data_HelmetStat").MakeDict();
        SwordItemDict = LoadJson<data.SwordLoader, int, data.SwordStat>("Item/Data_SwordStat").MakeDict();
        ArmorItemDict = LoadJson<data.ArmorLoader, int, data.ArmorStat>("Item/Data_ArmorStat").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
