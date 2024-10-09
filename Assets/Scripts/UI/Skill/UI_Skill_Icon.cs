using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class UI_Skill_Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] ESkillType _eSkillType;
    public ESkillType ESkillType 
    { 
        get 
        {
            Debug.Assert(_eSkillType != ESkillType.Roll &&_eSkillType != ESkillType.Count);
            return _eSkillType; 
        }
    }
    public Image Image { get; private set; }
    Transform _cacheParent;
    Vector3 _cachePos;
    public data.SkillInfo SkillInfo { get; private set; } 
    private void Awake()
    {
        SkillInfo = Managers.Data.SkillInfoDict[(int)_eSkillType];
        Image = GetComponent<Image>();
        _cacheParent = transform.parent;
        _cachePos = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_cacheParent);
        transform.localPosition = _cachePos;
        Image.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Managers.UI.SkillDesc.ShowSkillDesc(SkillInfo);
        Managers.Tween.StartUIScaleTW(transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Managers.UI.SkillDesc.HideDescription();
        Managers.Tween.EndToOneUIScaleTW(transform);
    }
}
