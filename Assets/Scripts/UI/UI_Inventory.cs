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
    public List<Image> InventoryItemSlots { get; private set; } = new(INVENTORY_SLOT_COUNT);

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
            InventoryItemSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, $"UI_ItemIcon{i:00}"));
            InventoryItemSlots[i - 1].enabled = false;
        }
    }

    public Image GetEmptySlotIconOrNull()
    {
        for (int i = 0; i < MAX_ROW_COUNT; ++i)
        {
            for (int j = 0; j < MAX_COL_COUNT; ++j)
            {
                if (!_itemSlotMatrix[i, j])
                {
                    _itemSlotMatrix[i, j] = true;
                    return InventoryItemSlots[i * MAX_COL_COUNT + j];
                }
            }
        }
        return null;
    }

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
