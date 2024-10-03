using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory_ItemDiscardIcon : UI_Inventory_BaseItemIcon
{
    public void OnDropDiscardIcon(ItemInfo itemInfo, Sprite sprite)
    {
        Debug.Assert(sprite != null);
        ItemInfo = itemInfo;
        Image.enabled = true;
        Image.sprite = sprite;
    }
    public override void Clear()
    {
        Image.enabled = false;
    }

    protected override void Init()
    {
    }
}
