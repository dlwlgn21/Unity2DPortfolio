using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class UI_Inventory_ItemSlot : MonoBehaviour, IDropHandler
{
    UI_Inventory_ItemIcon _icon;
    public int SlotIdx { get; private set; }
    private void Awake()
    {
        _icon = Utill.GetFirstComponentInChildrenOrNull<UI_Inventory_ItemIcon>(gameObject);
        SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = eventData.pointerDrag;
        if (dragedObject != null)
        {
            {
                // case Icon Drop
                UI_Inventory_ItemIcon dragedIcon = dragedObject.GetComponent<UI_Inventory_ItemIcon>();
                if (dragedIcon != null && SlotIdx != dragedIcon.SlotIdx)
                {
                    Managers.UI.SwapItemIcon(SlotIdx, dragedIcon.SlotIdx);
                    return;
                }
            }

            {
                // case EuqipedIcon Drop
                UI_Inventory_EquipableItemIcon equipedItemIcon = dragedObject.GetComponent<UI_Inventory_EquipableItemIcon>();
                if (equipedItemIcon != null)
                {
                    if (Managers.UI.TryPushEquipableItemToInventoryAt(equipedItemIcon.ItemInfo, SlotIdx))
                    {
                        equipedItemIcon.Clear();
                        return;
                    }
                }
            }
            {
                // case DiscardIcon Drop
                UI_Inventory_ItemDiscardIcon discardItemIcon = dragedObject.GetComponent<UI_Inventory_ItemDiscardIcon>();
                if (discardItemIcon != null)
                {
                    if (Managers.UI.TryPushDiscardItemToInventoryAt(discardItemIcon.ItemInfo, SlotIdx, discardItemIcon.ConsumableItemCount))
                    {
                        discardItemIcon.Clear();
                        return;
                    }
                }
            }
        }
    }
}
