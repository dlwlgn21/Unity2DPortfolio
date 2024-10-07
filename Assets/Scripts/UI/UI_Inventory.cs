using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using define;
public sealed class UI_Inventory : MonoBehaviour
{
    static public readonly int INVENTORY_SLOT_COUNT = 20;
    static public readonly int MAX_ROW_COUNT = 4;
    static public readonly int MAX_COL_COUNT = 5;
    //public List<Image> EquippableSlots { get; private set; } = new((int)define.EItemEquippableType.Count);
    List<UI_Inventory_ItemIcon> _invenItemIcons = new(INVENTORY_SLOT_COUNT);
    private void Awake()
    {
        //EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_HelmetItemIcon"));
        //EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_ArmorItemIcon"));
        //EquippableSlots.Add(Utill.GetComponentInChildrenOrNull<Image>(gameObject, "UI_SwordItemIcon"));
        //for (int i = 0; i < (int)define.EItemEquippableType.Count; ++i)
        //{
        //    EquippableSlots[i].enabled = false;
        //}
        for (int i = 1; i <= INVENTORY_SLOT_COUNT; ++i)
        {
            _invenItemIcons.Add(Utill.GetComponentInChildrenOrNull<UI_Inventory_ItemIcon>(gameObject, $"UI_ItemIcon{i:00}"));
        }
    }

    public UI_Inventory_ItemIcon GetIconAtOrNull(int idx)
    {
        if (idx >= INVENTORY_SLOT_COUNT)
        {
            Debug.Assert(false);
            return null;
        }
        return _invenItemIcons[idx];
    }
    public UI_Inventory_ItemIcon GetEmptyIconOrNull()
    {
        for (int i = 0; i < INVENTORY_SLOT_COUNT; ++i)
        {
            if (!_invenItemIcons[i].Image.enabled)
                return _invenItemIcons[i];
        }
        return null;
    }

    public UI_Inventory_ItemIcon GetSameCousmableIconOrNull(int coumableItemId)
    {
        for (int i = 0; i < INVENTORY_SLOT_COUNT; ++i)
        {
            if (_invenItemIcons[i].Image.enabled && 
                _invenItemIcons[i].ItemInfo.EItemType == define.EItemType.Consumable &&
                _invenItemIcons[i].ItemInfo.ItemId == coumableItemId)
            {
                return _invenItemIcons[i];
            }
        }
        return null;
    }

    public UI_Inventory_ItemIcon GetSpecifiedConsumableOrNull(EItemConsumableType eConsumableName, int itemId)
    {
        for (int i = 0; i < INVENTORY_SLOT_COUNT; ++i)
        {
            if (_invenItemIcons[i].Image.enabled &&
                _invenItemIcons[i].ItemInfo.EConsumableType == eConsumableName &&
                _invenItemIcons[i].ItemInfo.ItemId == itemId)
            {
                return _invenItemIcons[i];
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
