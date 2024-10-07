using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class UI_Skill_Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] ESkillType _eSkillType;
    public data.SkillInfo SkillInfo { get; private set; } 
    private void Awake()
    {
        SkillInfo = Managers.Data.SkillInfoDict[(int)_eSkillType];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Managers.UI.SkillDesc.ShowSkillDesc(SkillInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Managers.UI.SkillDesc.HideDescription();
    }
}
