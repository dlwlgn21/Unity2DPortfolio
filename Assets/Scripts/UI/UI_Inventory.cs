using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    static readonly int INVENTORY_SLOT_COUNT = 20;
    public List<Image> EquippableSlots { get; private set; } = new((int)define.EItemEquippableType.Count);
    public List<Image> InventoryItemSlots { get; private set; } = new(INVENTORY_SLOT_COUNT);


    private void Awake()
    {
        EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_HelmetItemIcon"));
        EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_ArmorItemIcon"));
        EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_SwordItemIcon"));
        for (int i = 0; i < (int)define.EItemEquippableType.Count; ++i)
        {
            EquippableSlots[i].enabled = false;
        }
        for (int i = 1; i <= INVENTORY_SLOT_COUNT; ++i)
        {
            InventoryItemSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, $"UI_ItemIcon{i:00}"));
            InventoryItemSlots[i - 1].enabled = false;
        }
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
