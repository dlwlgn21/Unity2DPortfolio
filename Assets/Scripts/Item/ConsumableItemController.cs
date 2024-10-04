using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableItemController : ItemController
{
    [SerializeField] EItemConsumableType _eConsumableName;

    public override void PushItemToInventory()
    {
        Managers.UI.TryPushItemToInventory(
            new ItemInfo()
            {
                EItemType = _eItemType,
                EEquippableType = EItemEquippableType.Count,
                EConsumableType = _eConsumableName,
                ItemId = _id
            }
        );
    }
}
