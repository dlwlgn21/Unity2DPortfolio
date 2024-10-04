using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Inventory_ItemDiscardSlot : MonoBehaviour, IDropHandler
{
    static public UnityAction<ItemInfo> ItemDiscardEventHandler;
    UI_Inventory_ItemDiscardIcon _icon;
    
    private void Awake()
    {
        _icon = Utill.GetFirstComponentInChildrenOrNull<UI_Inventory_ItemDiscardIcon>(gameObject);
        Debug.Assert(_icon != null);
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = eventData.pointerDrag;
        if (dragedObject != null)
        {
            UI_Inventory_ItemIcon dragedIcon = dragedObject.GetComponent<UI_Inventory_ItemIcon>();
            if (dragedIcon != null)
            {
                _icon.OnDropDiscardIcon(dragedIcon.ItemInfo, Managers.UI.GetSpriteByItemInfoOrNull(dragedIcon.ItemInfo), dragedIcon.ConsumableItemCount);
                Managers.UI.ClearInventorySlotAt(dragedIcon.SlotIdx);
            }
        }
    }

    public void OnDiscardBtnClicked()
    {
        if (ItemDiscardEventHandler != null)
            ItemDiscardEventHandler.Invoke(_icon.ItemInfo);
        _icon.Clear();
    }
}
