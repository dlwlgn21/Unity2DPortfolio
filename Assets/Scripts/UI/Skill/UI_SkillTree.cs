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
        _spawnReaperSkillNameText.text = Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Spawn_Reaper][0].name.Replace('1', '0');
        _spawnShooterSkillNameText.text = Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Spawn_Shooter][0].name.Replace('1', '0');
        _castBlackFlameSkillNameText.text = Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Cast_BlackFlame][0].name.Replace('1', '0');
        _castSwordStrikeSkillNameText.text = Managers.Data.ActiveSkillInfoDict[EActiveSkillType.Cast_SwordStrike][0].name.Replace('1', '0');
        #endregion
        #endregion
        UI_Skill_Icon.SkillLevelUpEventHandler -= OnSkillLevelUp;
        UI_Skill_Icon.SkillLevelUpEventHandler += OnSkillLevelUp;
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
    void OnSkillLevelUp(EActiveSkillType eType, int skillLevel)
    {
        SetSkillNameText(eType, skillLevel);
        SetSkillLevelImg(eType, skillLevel);
    }

    void SetSkillNameText(EActiveSkillType eType, int skillLevel)
    {
        switch (eType)
        {
            case EActiveSkillType.Spawn_Reaper:
                SetSkillName(_spawnReaperSkillNameText, eType, skillLevel);
                break;
            case EActiveSkillType.Spawn_Shooter:
                SetSkillName(_spawnShooterSkillNameText, eType, skillLevel);
                break;
            case EActiveSkillType.Cast_BlackFlame:
                SetSkillName(_castBlackFlameSkillNameText, eType, skillLevel);
                break;
            case EActiveSkillType.Cast_SwordStrike:
                SetSkillName(_castSwordStrikeSkillNameText, eType, skillLevel);
                break;
            default:
                Debug.DebugBreak();
                break;
        }
    }
    void SetSkillLevelImg(EActiveSkillType eType, int skillLevel)
    {
        switch (eType)
        {
            case EActiveSkillType.Spawn_Reaper:
                _spawnReaperSkillLevelImg.sprite = _skillLevelSprties[skillLevel - 1];
                break;
            case EActiveSkillType.Spawn_Shooter:
                _spawnShooterSkillLevelImg.sprite = _skillLevelSprties[skillLevel - 1];
                break;
            case EActiveSkillType.Cast_BlackFlame:
                _castBlackFlameSkillLevelImg.sprite = _skillLevelSprties[skillLevel - 1];
                break;
            case EActiveSkillType.Cast_SwordStrike:
                _castSwordStrikeSkillLevelImg.sprite = _skillLevelSprties[skillLevel - 1];
                break;
            default:
                Debug.DebugBreak();
                break;
        }
    }
    void SetSkillName(TextMeshProUGUI name, EActiveSkillType eType, int skillLevel)
    {
        name.text = Managers.Data.ActiveSkillInfoDict[eType][skillLevel - 1].name;
    }
    #endregion
}