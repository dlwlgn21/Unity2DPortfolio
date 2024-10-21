using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Skill_Slot : MonoBehaviour, IDropHandler
{
    public static UnityAction<ESkillSlot, EActiveSkillType> SkillIconDropEventHandler;
    [SerializeField] ESkillSlot _eSlot;
    UI_Skill_SlotIcon _icon;
    private void Awake()
    {
        _icon = Utill.GetFirstComponentInChildrenOrNull<UI_Skill_SlotIcon>(gameObject);
        Debug.Assert(_icon != null);
    }

    public void SwapIcon(UI_Skill_Slot slot)
    {
        _icon.Swap(slot._icon);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = eventData.pointerDrag;
        UI_Skill_Icon dragedIcon = dragedObject.GetComponent<UI_Skill_Icon>();
        if (dragedIcon != null)
        {
            if (dragedIcon.SkillLevel == 0)
            {
                PlayDeniedSoundAndPunchTW();
                Managers.Tween.StartUIDoPunchPos(transform);
                return;
            }
            if (Managers.PlayerSkill.IsAandSSlotUsingAnySkill())
            {
                PlayDeniedSoundAndPunchTW();
                return;
            }

            if (Managers.PlayerSkill.SwapIfSameNextToSlot(_eSlot, dragedIcon.ESkillType))
            {
                TryDropIcon(dragedIcon.ESkillType, dragedIcon.Image.sprite);
                PlayDropSucessSound();
                return;
            }
            TryDropIcon(dragedIcon.ESkillType, dragedIcon.Image.sprite);
            PlayDropSucessSound();
            return;
        }
        else
        {
            PlayDeniedSoundAndPunchTW();
        }
    }


    bool TryDropIcon(EActiveSkillType eSkillType, Sprite sprite)
    {
        if (_icon.TryDrop(eSkillType, sprite))
        {
            if (SkillIconDropEventHandler != null)
                SkillIconDropEventHandler.Invoke(_eSlot, eSkillType);
            Managers.Tween.StartUIScaleTW(_icon.transform, OnScaleTWEnd);
            return true;
        }
        else
            return false;
    }

    void OnScaleTWEnd()
    {
        Managers.Tween.EndToOneUIScaleTW(_icon.transform);
    }
    private void OnDestroy()
    {
        SkillIconDropEventHandler = null;
    }

    void PlayDeniedSoundAndPunchTW()
    {
        Managers.Tween.StartUIDoPunchPos(transform);
        Managers.Sound.Play(DataManager.SFX_UI_DENIED);
    }
    void PlayDropSucessSound()
    {
        Managers.Sound.Play(DataManager.SFX_UI_EQUP_SUCESS);
    }
}
