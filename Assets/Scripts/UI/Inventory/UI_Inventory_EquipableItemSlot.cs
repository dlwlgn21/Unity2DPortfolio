using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inventory_EquipableItemSlot : MonoBehaviour, IDropHandler
{
    UI_Inventory_EquipableItemIcon _icon;
    [SerializeField] EItemEquippableType _eEquippableType;
    private void Awake()
    {
        _icon = Utill.GetFirstComponentInChildrenOrNull<UI_Inventory_EquipableItemIcon>(gameObject);
        Debug.Assert(_icon != null);
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = eventData.pointerDrag;
        if (dragedObject != null)
        {
            {
                // case InventoryItem -> Equip
                UI_Inventory_ItemIcon dragedIcon = dragedObject.GetComponent<UI_Inventory_ItemIcon>();
                if (dragedIcon != null && 
                    dragedIcon.ItemInfo.EItemType == EItemType.Equippable &&
                    dragedIcon.ItemInfo.EEquippableType == _eEquippableType)
                {
                    _icon.EqiupItem(dragedIcon.ItemInfo);
                    Managers.UI.ClearInventorySlotAt(dragedIcon.SlotIdx);
                }
            }
            {
                // case DiscardItem -> Equip
                UI_Inventory_ItemDiscardIcon discardIcon = dragedObject.GetComponent<UI_Inventory_ItemDiscardIcon>();
                if (discardIcon != null &&
                    discardIcon.ItemInfo.EItemType == EItemType.Equippable &&
                    discardIcon.ItemInfo.EEquippableType == _eEquippableType)
                {
                    _icon.EqiupItem(discardIcon.ItemInfo);
                    discardIcon.Clear();
                }


            }
        }
    }
}