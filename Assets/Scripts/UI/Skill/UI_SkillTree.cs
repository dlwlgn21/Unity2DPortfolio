using define;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public sealed class UI_SkillTree : MonoBehaviour
{
    public Canvas Canvas { get; set; }

    TextMeshProUGUI _skillPointText;
    Sprite[] _skillLevelSprties = new Sprite[3];
    Image _spawnReaperSkillLevelImg;
    Image _spawnShooterSkillLevelImg;
    Image _castBlackFlameSkillLevelImg;
    Image _castSwordStrikeSkillLevelImg;

    TextMeshProUGUI _spawnReaperSkillNameText;
    TextMeshProUGUI _spawnShooterSkillNameText;
    TextMeshProUGUI _castBlackFlameSkillNameText;
    TextMeshProUGUI _castSwordStrikeSkillNameText;

    UI_Skill_Icon _spawnReaperIcon;
    UI_Skill_Icon _spawnShooterIcon;
    UI_Skill_Icon _castBlackFlameIcon;
    UI_Skill_Icon _castSwordStrikeIcon;


    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
        #region Init
        _skillPointText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SkillPointText");
        #region LevelSprties
        for (int i = 0; i < 3; ++i)
        {
            _skillLevelSprties[i] = Managers.Resources.Load<Sprite>($"Art/SkillLevelNumbers/Number{i + 1}");
            if (_skillLevelSprties == null)
                Debug.DebugBreak();
        }
        #endregion
        #region SkillLevel
        _spawnReaperSkillLevelImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "SpawnReaperSkillLevelImg");
        _spawnShooterSkillLevelImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "SpawnShooterSkillLevelImg");
        _castBlackFlameSkillLevelImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "BlackFlameSkillLevelImg");
        _castSwordStrikeSkillLevelImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "SwordStrikeSkillLevelImg");
        #endregion
        #region SkillName
        _spawnReaperSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SpawnReaperSkillNameText");
        _spawnShooterSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SpawnShooterSkillNameText");
        _castBlackFlameSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "BlackFlameSkillNameText");
        _castSwordStrikeSkillNameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SwordStrikeSkillNameText");
        #endregion
        #region SKillIcon
        _spawnReaperIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "SpawnReaperSkillIcon");
        _spawnShooterIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "SpawnShooterSkillIcon");
        _castBlackFlameIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "BlackFlameSkillIcon");
        _castSwordStrikeIcon = Utill.GetComponentInChildrenOrNull<UI_Skill_Icon>(gameObject, "SwordStrikeSkillIcon");
        #endregion
        #region SkillNameTextToZero
        var dict = Managers.Data.SkillInfoDict;
        _spawnReaperSkillNameText.text = dict[(int)_spawnReaperIcon.ESkillType].name.Replace('1', '0');
        _spawnShooterSkillNameText.text = dict[(int)_spawnShooterIcon.ESkillType].name.Replace('1', '0');
        _castBlackFlameSkillNameText.text = dict[(int)_castBlackFlameIcon.ESkillType].name.Replace('1', '0');
        _castSwordStrikeSkillNameText.text = dict[(int)_castSwordStrikeIcon.ESkillType].name.Replace('1', '0');
        #endregion
        #endregion
        UI_Skill_Icon.OnSkillLevelUpEventHandler -= OnSkillLevelUp;
        UI_Skill_Icon.OnSkillLevelUpEventHandler += OnSkillLevelUp;
    }

    private void OnEnable()
    {
        if (DOTween.IsTweening(_skillPointText.transform))
            return;
        _skillPointText.transform.DOScale(new Vector3(2f, 2f, 2f), 0.5f).SetEase(Ease.InOutElastic);
    }

    private void OnDisable()
    {
        _skillPointText.transform.localScale = Vector3.one;
    }

    public void SetSkillPoint(int skillPoint)
    {
        _skillPointText.text = skillPoint.ToString();
    }

    #region Private
    void OnSkillLevelUp(ESkillType eType)
    {
        SetSkillNameText(eType);
        SetSkillLevelImg(eType);
    }

    void SetSkillNameText(ESkillType eType)
    {
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
            case ESkillType.Spawn_Reaper_LV2:
            case ESkillType.Spawn_Reaper_LV3:
                SetSkillName(_spawnReaperSkillNameText, eType);
                break;
            case ESkillType.Spawn_Shooter_LV1:
            case ESkillType.Spawn_Shooter_LV2:
            case ESkillType.Spawn_Shooter_LV3:
                SetSkillName(_spawnShooterSkillNameText, eType);
                break;
            case ESkillType.Cast_BlackFlame_LV1:
            case ESkillType.Cast_BlackFlame_LV2:
            case ESkillType.Cast_BlackFlame_LV3:
                SetSkillName(_castBlackFlameSkillNameText, eType);
                break;
            case ESkillType.Cast_SwordStrike_LV1:
            case ESkillType.Cast_SwordStrike_LV2:
            case ESkillType.Cast_SwordStrike_LV3:
                SetSkillName(_castSwordStrikeSkillNameText, eType);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    void SetSkillLevelImg(ESkillType eType)
    {
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
                _spawnReaperSkillLevelImg.sprite = _skillLevelSprties[0];
                break;
            case ESkillType.Spawn_Reaper_LV2:
                _spawnReaperSkillLevelImg.sprite = _skillLevelSprties[1];
                break;
            case ESkillType.Spawn_Reaper_LV3:
                _spawnReaperSkillLevelImg.sprite = _skillLevelSprties[2];
                break;
            case ESkillType.Spawn_Shooter_LV1:
                _spawnShooterSkillLevelImg.sprite = _skillLevelSprties[0];
                break;
            case ESkillType.Spawn_Shooter_LV2:
                _spawnShooterSkillLevelImg.sprite = _skillLevelSprties[1];
                break;
            case ESkillType.Spawn_Shooter_LV3:
                _spawnShooterSkillLevelImg.sprite = _skillLevelSprties[2];
                break;
            case ESkillType.Cast_BlackFlame_LV1:
                _castBlackFlameSkillLevelImg.sprite = _skillLevelSprties[0];
                break;
            case ESkillType.Cast_BlackFlame_LV2:
                _castBlackFlameSkillLevelImg.sprite = _skillLevelSprties[1];
                break;
            case ESkillType.Cast_BlackFlame_LV3:
                _castBlackFlameSkillLevelImg.sprite = _skillLevelSprties[2];
                break;
            case ESkillType.Cast_SwordStrike_LV1:
                _castSwordStrikeSkillLevelImg.sprite = _skillLevelSprties[0];
                break;
            case ESkillType.Cast_SwordStrike_LV2:
                _castSwordStrikeSkillLevelImg.sprite = _skillLevelSprties[1];
                break;
            case ESkillType.Cast_SwordStrike_LV3:
                _castSwordStrikeSkillLevelImg.sprite = _skillLevelSprties[2];
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    void SetSkillName(TextMeshProUGUI name, ESkillType eType)
    {
        var dict = Managers.Data.SkillInfoDict;
        name.text = dict[(int)eType].name;
    }
    #endregion
}