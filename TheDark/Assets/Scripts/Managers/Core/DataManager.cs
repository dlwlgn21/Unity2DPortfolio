using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager
{
    public Dictionary<int, data.PlayerStat> PlayerStatDict { get; private set; } = new Dictionary<int, data.PlayerStat>();
    public Dictionary<int, data.MonsterStat> MonsterStatDict { get; private set; } = new Dictionary<int, data.MonsterStat>();

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
        PlayerStatDict = LoadJson<data.PlayerStatData, int, data.PlayerStat>("Player/Data_PlayerStat").MakeDict();
        Debug.Assert(PlayerStatDict.Count != 0);
        MonsterStatDict = LoadJson<data.MonsterStatData, int, data.MonsterStat>("Monsters/Data_MonstersStat").MakeDict();
        Debug.Assert(MonsterStatDict.Count != 0);
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

}
