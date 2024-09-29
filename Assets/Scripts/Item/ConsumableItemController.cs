using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsumableItemController : ItemController
{
    [SerializeField] EItemConsumableName _eConsumableName;



    public override void PushItem()
    {
        Managers.UI.PushItemToInventory(
            new ItemInfo()
            {
                EItemType = _eItemType,
                EEquippableName = EItemEquippableName.Count,
                EConsumableName = _eConsumableName
            },
            _id
        );
    }
}
