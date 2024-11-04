using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public sealed class DataManager
{
    public Dictionary<int, data.PlayerStat> PlayerStatDict { get; private set; } = new();
    public Dictionary<define.EActiveSkillType, List<data.SkillInfo>> ActiveSkillInfoDict { get; private set; } = new();
    public Dictionary<int, data.MonsterStat> MonsterStatDict { get; private set; } = new();
    public Dictionary<int, data.HealingPotionInfo> HealingPotionDict { get; private set; } = new();
    public Dictionary<int, data.HelmetInfo> HelmetItemDict { get; private set; } = new();
    public Dictionary<int, data.SwordInfo> SwordItemDict { get; private set; } = new();
    public Dictionary<int, data.ArmorInfo> ArmorItemDict { get; private set; } = new();
    Dictionary<int, data.SkillInfo> _skillInfoDict = new();
    public data.SfxKeyContainer SFXKeyContainer { get; private set; } = new();

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
                case "SpawnReaper":
                    AddSkills(define.EActiveSkillType.Spawn_Reaper, info.Value);
                    break;
                case "SpawnShooter":
                    AddSkills(define.EActiveSkillType.Spawn_Shooter, info.Value);
                    break;
                case "BlackFlame":
                    AddSkills(define.EActiveSkillType.Cast_BlackFlame, info.Value);
                    break;
                case "SwordStrike":
                    AddSkills(define.EActiveSkillType.Cast_SwordStrike, info.Value);
                    break;
                default:
                    break;
            }
        }
        #endregion
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/SFX/Data_SFXPaths");
        SFXKeyContainer = JsonUtility.FromJson<data.SfxKeyContainer>(textAsset.text);
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
