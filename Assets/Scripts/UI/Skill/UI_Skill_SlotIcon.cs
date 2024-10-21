using define;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class UI_Skill_SlotIcon : MonoBehaviour
{
    EActiveSkillType _eCurrSkillType;
    Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;
        _eCurrSkillType = EActiveSkillType.Count;
    }
    public bool TryDrop(EActiveSkillType eType, Sprite sprite)
    {
        if (_eCurrSkillType == eType) 
            return false;
        _eCurrSkillType = eType;
        _image.enabled = true;
        _image.sprite = sprite;
        return true;
    }

    public void Swap(UI_Skill_SlotIcon icon)
    {
        Sprite tmpSprite = icon._image.sprite;
        bool tmpIsEnabled = icon._image.enabled;
        EActiveSkillType tmpESkillType = icon._eCurrSkillType;
        icon._image.sprite = this._image.sprite;
        icon._image.enabled = this._image.enabled;
        icon._eCurrSkillType = this._eCurrSkillType;
        this._image.sprite = tmpSprite;
        this._image.enabled = tmpIsEnabled;
        this._eCurrSkillType = tmpESkillType;
    }
}
