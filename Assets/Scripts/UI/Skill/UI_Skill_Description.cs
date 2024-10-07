using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
public sealed class UI_Skill_Description : UI_BaseDescription
{
    protected override void Init()
    {
    }

    public void ShowSkillDesc(data.SkillInfo skillInfo)
    {
        SetImagesEnabled(true);
        _nameText.text = skillInfo.name;
        _descText.text = skillInfo.description;
    }
    
}
