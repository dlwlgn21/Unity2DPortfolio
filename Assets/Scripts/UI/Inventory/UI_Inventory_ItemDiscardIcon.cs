using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Inventory_ItemDiscardIcon : UI_Inventory_BaseItemIcon
{
    static public UnityAction DiscardIconOnDropEventHandler;
    static public UnityAction DiscardIconOnClearEventHandler;
    public int ConsumableItemCount { get; private set; }
    public void OnDropDiscardIcon(ItemInfo itemInfo, Sprite sprite, int cousumableCount)
    {
        Debug.Assert(sprite != null);
        ItemInfo = itemInfo;
        Image.enabled = true;
        Image.sprite = sprite;
        ConsumableItemCount = cousumableCount;
        if (DiscardIconOnDropEventHandler != null)
            DiscardIconOnDropEventHandler.Invoke();
    }
    public override void Clear()
    {
        Image.enabled = false;
        ConsumableItemCount = 0;
        if (DiscardIconOnClearEventHandler != null)
            DiscardIconOnClearEventHandler.Invoke();
    }

    protected override void Init()
    {

    }

    private void OnDestroy()
    {
        DiscardIconOnDropEventHandler = null;
        DiscardIconOnClearEventHandler = null;
    }
}
