using define;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public sealed class UI_SkillTree : MonoBehaviour
{
    TextMeshProUGUI _skillPointText;

    TextMeshProUGUI _spawnReaperSkillPointText;
    TextMeshProUGUI _spawnPandaSkillPointText;
    TextMeshProUGUI _castBlackFlameSkillPointText;
    TextMeshProUGUI _castSwordStrikeSkillPointText;

    TextMeshProUGUI _spawnReaperSkillNameText;
    TextMeshProUGUI _spawnPandaSkillNameText;
    TextMeshProUGUI _castBlackFlameSkillNameText;
    TextMeshProUGUI _castSwordStrikeSkillNameText;

    UI_Skill_Icon _spawnReaperIcon;
    UI_Skill_Icon _spawnPandaIcon;
    UI_Skill_Icon _castBlackFlameIcon;
    UI_Skill_Icon _castSwordStrikeIcon;

    private void Awake()
    {
        _skillPointText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SkillPointText");

        #region SkillPoint
        _spawnReaperSkillPointText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SpawnReaperSkillLevelText");
        _spawnPandaSkillPointText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SpawnPandaSkillLevelText");
        _castBlackFlameSkillPointText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "BlackFlameSkillLevelText");
        _castSwordStrikeSkillPointText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SwordStrikeSkillLevelText");
        #endregion
        #region SkillName
        _spawnReaperSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SpawnReaperSkillNameText");
        _spawnPandaSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SpawnPandaSkillNameText");
        _castBlackFlameSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "BlackFlameSkillNameText");
        _castSwordStrikeSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SwordStrikeSkillNameText");
        #endregion
        #region SKillIcon
        _spawnReaperIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "SpawnReaperSkillIcon");
        _spawnPandaIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "SpawnPandaSkillIcon");
        _castBlackFlameIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "BlackFlameSkillIcon");
        _castSwordStrikeIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "SwordStrikeSkillIcon");
        #endregion

        var dict = Managers.Data.SkillInfoDict;
        _spawnReaperSkillNameText.text = dict[(int)_spawnReaperIcon.ESkillType].name;
        _spawnPandaSkillNameText.text = dict[(int)_spawnPandaIcon.ESkillType].name;
        _castBlackFlameSkillNameText.text = dict[(int)_castBlackFlameIcon.ESkillType].name;
        _castSwordStrikeSkillNameText.text = dict[(int)_castSwordStrikeIcon.ESkillType].name;

        UI_Skill_Icon.OnSkillLevelUpEventHandler -= OnSkillLevelUp;
        UI_Skill_Icon.OnSkillLevelUpEventHandler += OnSkillLevelUp;
    }


    public void SetSkillPoint(int skillPoint)
    {
        _skillPointText.text = skillPoint.ToString();
    }

    void OnSkillLevelUp(ESkillType eType)
    {
        var dict = Managers.Data.SkillInfoDict;
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
            case ESkillType.Spawn_Reaper_LV2:
            case ESkillType.Spawn_Reaper_LV3:
                _spawnReaperSkillPointText.text = GetPlusOneStr(int.Parse(_spawnReaperSkillPointText.text));
                _spawnReaperSkillNameText.text = dict[(int)eType].name;
                break;
            case ESkillType.Spawn_Panda_LV1:
            case ESkillType.Spawn_Panda_LV2:
            case ESkillType.Spawn_Panda_LV3:
                _spawnPandaSkillPointText.text = GetPlusOneStr(int.Parse(_spawnPandaSkillPointText.text));
                _spawnPandaSkillNameText.text = dict[(int)eType].name;
                break;
            case ESkillType.Cast_BlackFlame_LV1:
            case ESkillType.Cast_BlackFlame_LV2:
            case ESkillType.Cast_BlackFlame_LV3:
                _castBlackFlameSkillPointText.text = GetPlusOneStr(int.Parse(_castBlackFlameSkillPointText.text));
                _castBlackFlameSkillNameText.text = dict[(int)eType].name;
                break;
            case ESkillType.Cast_SwordStrike_LV1:
            case ESkillType.Cast_SwordStrike_LV2:
            case ESkillType.Cast_SwordStrike_LV3:
                _castSwordStrikeSkillPointText.text = GetPlusOneStr(int.Parse(_castSwordStrikeSkillPointText.text));
                _castSwordStrikeSkillNameText.text = dict[(int)eType].name;
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    string GetPlusOneStr(int num)
    {
        return (num + 1).ToString();
    }
}
