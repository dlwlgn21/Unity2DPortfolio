using define;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EquipableItemController : ItemController
{
    [SerializeField] EItemEquippableType _eEquipableName;
    public override void PushItemToInventory()
    {
        Managers.UI.PushItemToInventory(
            new ItemInfo()
            {
                EItemType = _eItemType,
                EEquippableType = _eEquipableName,
                EConsumableType = EItemConsumableType.Count,
                ItemId = _id
            }
        );
    }
}
