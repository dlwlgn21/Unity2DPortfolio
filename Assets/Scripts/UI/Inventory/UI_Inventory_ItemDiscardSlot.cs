using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inventory_ItemDiscardSlot : MonoBehaviour, IDropHandler
{
    UI_Inventory_ItemDiscardIcon _icon;
    UI_Inventory_ItemIcon _currDiscardingIcon;

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
                _currDiscardingIcon = dragedIcon;
                _icon.OnDropDiscardIcon(dragedIcon.ItemInfo, Managers.UI.GetSpriteByItemInfoOrNull(dragedIcon.ItemInfo));
            }
        }
    }

    public void OnDiscardBtnClicked()
    {
        if (_currDiscardingIcon != null)
        {
            _icon.Clear();
            Managers.UI.ClearInventorySlotAt(_currDiscardingIcon.SlotIdx);
            _currDiscardingIcon = null;
        }
    }
}
