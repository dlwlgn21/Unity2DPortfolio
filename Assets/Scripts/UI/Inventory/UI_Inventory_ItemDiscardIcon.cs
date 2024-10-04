using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory_ItemDiscardIcon : UI_Inventory_BaseItemIcon
{
    public int ConsumableItemCount { get; private set; }
    public void OnDropDiscardIcon(ItemInfo itemInfo, Sprite sprite, int cousumableCount)
    {
        Debug.Assert(sprite != null);
        ItemInfo = itemInfo;
        Image.enabled = true;
        Image.sprite = sprite;
        ConsumableItemCount = cousumableCount;
    }
    public override void Clear()
    {
        Image.enabled = false;
        ConsumableItemCount = 0;
    }

    protected override void Init()
    {

    }
}
