using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class UI_Inventory_ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemInfo ItemInfo { get; set; }
    public Image Image { get; set; }
    public int ItemId { get; set; }
    public int SlotIdx { get; private set; }

    Transform _cacheParent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Image.enabled)
        {
            Managers.UI.ItemDesc.ShowItemDesc(ItemInfo, ItemId);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Image.enabled)
        {
            Managers.UI.ItemDesc.HideItemDesc();
        }
    }

    private void Awake()
    {
        Image = GetComponent<Image>();
        Image.enabled = false;
        _cacheParent = transform.parent;
        SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
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
        Image.raycastTarget = true;
    }
}
