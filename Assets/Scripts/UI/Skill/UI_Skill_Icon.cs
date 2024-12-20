using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class UI_Skill_Icon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    static public UnityAction<EActiveSkillType, int> SkillLevelUpEventHandler;
    [SerializeField] EActiveSkillType _eSkillType;
    public int SkillLevel { get; set; } = 0;
    public EActiveSkillType ESkillType 
    { 
        get 
        {
            Debug.Assert(_eSkillType != EActiveSkillType.Roll &&_eSkillType != EActiveSkillType.Count);
            return _eSkillType; 
        }
    }
    public Image Image { get; private set; }
    Transform _cacheParent;
    Vector3 _cachePos;
    public data.SkillInfo SkillInfo { get; private set; } 
    private void Awake()
    {
        SkillInfo = Managers.Data.ActiveSkillInfoDict[ESkillType][0];
        Image = GetComponent<Image>();
        _cacheParent = transform.parent;
        _cachePos = transform.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (SkillLevel == 0)
        {
            UIPunchTWStart();
            return;
        }
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SkillLevel == 0)
            return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (SkillLevel == 0)
            return;
        transform.SetParent(_cacheParent);
        transform.localPosition = _cachePos;
        Image.raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SkillLevel == 0)
            Managers.UI.SkillDesc.ShowSkillDesc(ESkillType, 0);
        else
            Managers.UI.SkillDesc.ShowSkillDesc(ESkillType, SkillLevel);
        Managers.Tween.StartUIScaleTW(transform);
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_UI_POINTER_ENTER);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Managers.UI.SkillDesc.HideDescription();
        Managers.Tween.EndToOneUIScaleTW(transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Managers.PlayerLevel.CurrSkillPoint >= 1 && SkillLevel < PlayerLevelManager.MAX_SKILL_LEVEL)
        {
            ++SkillLevel;
            if (SkillLevel > 1)
            {
                SkillInfo = Managers.Data.ActiveSkillInfoDict[_eSkillType][SkillLevel - 1];
            }
            Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_UI_DROP_OR_ITEM_GET_SUCESS);
            Managers.UI.SkillDesc.ShowSkillDesc(ESkillType, SkillLevel);
            if (SkillLevelUpEventHandler != null)
                SkillLevelUpEventHandler.Invoke(_eSkillType, SkillLevel);
            Managers.Tween.StartUIScaleTW(transform.parent.gameObject.transform, () => { Managers.Tween.EndToOneUIScaleTW(transform.parent.gameObject.transform); });
        }
        else
        {
            Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_UI_DENIED);
            UIPunchTWStart();
        }
    }

    void UIPunchTWStart()
    {
        Managers.Tween.StartUIDoPunchPos(transform.parent.gameObject.transform);
    }

    private void OnDestroy()
    {
        SkillLevelUpEventHandler = null;
    }

}
