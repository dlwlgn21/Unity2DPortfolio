using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using define;
using UnityEngine.Assertions.Must;

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

    public void ShowSkillDesc(EActiveSkillType eType, int skillLevel)
    {
        SetImagesEnabled(true);
        data.SkillInfo info;
        if (skillLevel == 0)
            info = Managers.Data.ActiveSkillInfoDict[eType][0];
        else
            info = Managers.Data.ActiveSkillInfoDict[eType][Mathf.Min(skillLevel - 1, PlayerLevelManager.MAX_SKILL_LEVEL)];

        _nameText.text = info.name;
        _descText.text = info.description;
        _animator.gameObject.SetActive(true);

        switch (eType)
        {
            case EActiveSkillType.Spawn_Reaper:
                _animator.Play(SPAWN_REAPER_ANIM_KEY, -1, 0f);
                break;
            case EActiveSkillType.Spawn_Shooter:
                _animator.Play(SPAWN_PANDA_ANIM_KEY, -1, 0f);
                break;
            case EActiveSkillType.Cast_BlackFlame:
                _animator.Play(CAST_BLACK_FLAME_ANIM_KEY, -1, 0f);
                break;
            case EActiveSkillType.Cast_SwordStrike:
                _animator.Play(CAST_SWORD_STRIKE_ANIM_KEY, -1, 0f);
                break;
            default:
                Debug.DebugBreak();
                break;
        }
    }

    public override void HideDescription()
    {
        base.HideDescription();
        _animator.gameObject.SetActive(false);
    }

}
