using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory_EquipableItemIcon : UI_Inventory_BaseItemIcon
{
    [SerializeField] EItemEquippableType _eEquipableType;
    protected override void Init()
    {

    }
    
    public void EqiupItem(ItemInfo itemInfo)
    {
        Sprite sprite = Managers.UI.GetEquipableItemSprite(itemInfo, _eEquipableType);
        ItemInfo = itemInfo;
        Image.enabled = true;
        Image.sprite = sprite;
    }

}
