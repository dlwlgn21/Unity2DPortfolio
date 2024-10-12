using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using define;
public sealed class UI_Skill_Description : UI_BaseDescription
{
    Animator _animator;
    const string SPAWN_REAPER_ANIM_KEY = "UISkillSpawnReaper";
    const string SPAWN_PANDA_ANIM_KEY = "UISkillSpawnPanda";
    const string CAST_BLACK_FLAME_ANIM_KEY = "UISkillBlackFlame";
    const string CAST_SWORD_STRIKE_ANIM_KEY = "UISkillSwordStrike";
    protected override void Init()
    {
        _animator = Utill.GetFirstComponentInChildrenOrNull<Animator>(gameObject);
        _animator.gameObject.SetActive(false);
    }

    public void ShowSkillDesc(data.SkillInfo skillInfo)
    {
        SetImagesEnabled(true);
        _nameText.text = skillInfo.name;
        _descText.text = skillInfo.description;
        _animator.gameObject.SetActive(true);
        ESkillType eType = (ESkillType)skillInfo.id;
        switch (eType)
        {
            case ESkillType.Spawn_Reaper_LV1:
            case ESkillType.Spawn_Reaper_LV2:
            case ESkillType.Spawn_Reaper_LV3:
                _animator.Play(SPAWN_REAPER_ANIM_KEY, -1, 0f);
                break;
            case ESkillType.Spawn_Shooter_LV1:
            case ESkillType.Spawn_Shooter_LV2:
            case ESkillType.Spawn_Shooter_LV3:
                _animator.Play(SPAWN_PANDA_ANIM_KEY, -1, 0f);
                break;
            case ESkillType.Cast_BlackFlame_LV1:
            case ESkillType.Cast_BlackFlame_LV2:
            case ESkillType.Cast_BlackFlame_LV3:
                _animator.Play(CAST_BLACK_FLAME_ANIM_KEY, -1, 0f);
                break;
            case ESkillType.Cast_SwordStrike_LV1:
            case ESkillType.Cast_SwordStrike_LV2:
            case ESkillType.Cast_SwordStrike_LV3:
                _animator.Play(CAST_SWORD_STRIKE_ANIM_KEY, -1, 0f);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    public override void HideDescription()
    {
        base.HideDescription();
        _animator.gameObject.SetActive(false);
    }

}
