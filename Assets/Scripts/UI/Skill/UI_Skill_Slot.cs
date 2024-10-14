using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Skill_Slot : MonoBehaviour, IDropHandler
{
    public static UnityAction<ESkillSlot, ESkillType> OnSkillIocnDropEventHandler;
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
                Managers.Tween.StartUIDoPunchPos(transform);
                return;
            }
            if (Managers.PlayerSkill.IsAandSSlotUsingAnySkill())
            {
                // TODO : 메시지 띄워야 한다..??
                Debug.Log("Can't Drop!! Using Skill!!");
                Managers.Tween.StartUIDoPunchPos(transform);
                return;
            }

            if (Managers.PlayerSkill.SwapIfSameNextToSlot(_eSlot, dragedIcon.ESkillType))
            {
                TryDropIcon(dragedIcon.ESkillType, dragedIcon.Image.sprite);
                return;
            }
            TryDropIcon(dragedIcon.ESkillType, dragedIcon.Image.sprite);
            return;
        }
    }


    bool TryDropIcon(ESkillType eSkillType, Sprite sprite)
    {
        if (_icon.TryDrop(eSkillType, sprite))
        {
            if (OnSkillIocnDropEventHandler != null)
                OnSkillIocnDropEventHandler.Invoke(_eSlot, eSkillType);
            return true;
        }
        else
            return false;
    }

    private void OnDestroy()
    {
        OnSkillIocnDropEventHandler = null;
    }
}
