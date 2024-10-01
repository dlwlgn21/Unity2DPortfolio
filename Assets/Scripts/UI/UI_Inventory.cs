using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    static readonly int INVENTORY_SLOT_COUNT = 20;
    static readonly int MAX_ROW_COUNT = 4;
    static readonly int MAX_COL_COUNT = 5;
    public List<Image> EquippableSlots { get; private set; } = new((int)define.EItemEquippableName.Count);
    public List<UI_Inventory_ItemIcon> InventoryItemIcons { get; private set; } = new(INVENTORY_SLOT_COUNT);

    bool[,] _itemSlotMatrix = new bool[MAX_ROW_COUNT, MAX_COL_COUNT];
    private void Awake()
    {
        EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_HelmetItemIcon"));
        EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_ArmorItemIcon"));
        EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_SwordItemIcon"));
        for (int i = 0; i < (int)define.EItemEquippableName.Count; ++i)
        {
            EquippableSlots[i].enabled = false;
        }
        for (int i = 1; i <= INVENTORY_SLOT_COUNT; ++i)
        {
            InventoryItemIcons.Add(Utill.GetComponentInChildrenOrNull<UI_Inventory_ItemIcon>(gameObject, $"UI_ItemIcon{i:00}"));
        }
    }

    public UI_Inventory_ItemIcon GetIconAtOrNull(int idx)
    {
        if (idx >= INVENTORY_SLOT_COUNT)
        {
            Debug.Assert(false);
            return null;
        }
        return InventoryItemIcons[idx];
    }
    public UI_Inventory_ItemIcon GetEmptyIconOrNull()
    {
        //for (int i = 0; i < MAX_ROW_COUNT; ++i)
        //{
        //    for (int j = 0; j < MAX_COL_COUNT; ++j)
        //    {
        //        if (!_itemSlotMatrix[i, j])
        //        {
        //            _itemSlotMatrix[i, j] = true;
        //            return InventoryItemIcons[i * MAX_COL_COUNT + j];
        //        }
        //    }
        //}

        for (int i = 0; i < INVENTORY_SLOT_COUNT; ++i)
        {
            if (!InventoryItemIcons[i].Image.enabled)
                return InventoryItemIcons[i];
        }
        return null;
    }

    public UI_Inventory_ItemIcon GetSameCousmableIconOrNull(int coumableItemId)
    {
        for (int i = 0; i < INVENTORY_SLOT_COUNT; ++i)
        {
            if (InventoryItemIcons[i].Image.enabled && 
                InventoryItemIcons[i].ItemInfo.EItemType == define.EItemType.Consumable &&
                InventoryItemIcons[i].ItemId == coumableItemId)
            {
                return InventoryItemIcons[i];
            }
        }
        return null;
    }

    //public void SetItemSlotMatrix(int idx, bool isTrue)
    //{
    //    _itemSlotMatrix[idx / MAX_COL_COUNT, idx % MAX_COL_COUNT] = isTrue;
    //}
    public void RefreshUI()
    {

    }

    public void Init()
    {

    }

    public void Clear()
    {

    }

}
