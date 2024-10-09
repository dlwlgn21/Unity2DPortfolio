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
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = eventData.pointerDrag;
        UI_Skill_Icon dragedIcon = dragedObject.GetComponent<UI_Skill_Icon>();
        if (dragedIcon != null)
        {
            if (Managers.PlayerSkill.IsDuplicatedIcon(_eSlot, dragedIcon.ESkillType))
            {
                Managers.Tween.StartUIDoPunchPos(transform);
                return;
            }
            if (_icon.TryDrop(dragedIcon.ESkillType, dragedIcon.Image.sprite))
            {
                if (OnSkillIocnDropEventHandler != null)
                    OnSkillIocnDropEventHandler.Invoke(_eSlot, dragedIcon.ESkillType);
            }
            return;
        }
    }

    private void OnDestroy()
    {
        OnSkillIocnDropEventHandler = null;
    }
}
