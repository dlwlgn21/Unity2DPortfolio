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
