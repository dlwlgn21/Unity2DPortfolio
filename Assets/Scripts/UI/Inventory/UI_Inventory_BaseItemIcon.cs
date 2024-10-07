using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using define;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class UI_Inventory_BaseItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemInfo ItemInfo { get; set; }
    public Image Image { get; set; }
    protected Transform _cacheParent;
    private void Awake()
    {
        Image = GetComponent<Image>();
        Image.enabled = false;
        _cacheParent = transform.parent;
        Init();
    }
    protected abstract void Init();
    public abstract void Clear();

    public bool IsEmpty()
    {
        return Image.enabled == false;
    }
    #region ItemDescription
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Image.enabled)
        {
            Managers.UI.ItemDesc.ShowItemDesc(ItemInfo);
            transform.DOScale(DoTweenValueContainer.TWEEN_SCALE_END_VALUE, DoTweenValueContainer.TWEEN_SCALE_END_TIME_IN_SEC).SetEase(Ease.InOutElastic);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Image.enabled)
        {
            Managers.UI.ItemDesc.HideDescription();
            transform.DOScale(Vector3.one, DoTweenValueContainer.TWEEN_SCALE_END_TIME_IN_SEC).SetEase(Ease.InOutElastic);
        }
    }
    #endregion
    #region ItemDrag
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_cacheParent);
        Image.raycastTarget = true;
    }
    #endregion

}
