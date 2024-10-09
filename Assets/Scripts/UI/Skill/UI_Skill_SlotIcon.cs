using define;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_SlotIcon : MonoBehaviour
{
    ESkillType _eCurrSkillType;
    Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
        _eCurrSkillType = ESkillType.Count;
    }
    public bool TryDrop(ESkillType eType, Sprite sprite)
    {
        if (_eCurrSkillType == eType) 
            return false;
        _eCurrSkillType = eType;
        _image.enabled = true;
        _image.sprite = sprite;
        return true;
    }
}
