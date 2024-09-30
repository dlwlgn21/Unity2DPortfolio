using define;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EquipableItemController : ItemController
{
    [SerializeField] EItemEquippableName _eEquipableName;
    public override void PushItemToInventory()
    {
        Managers.UI.PushItemToInventory(
            new ItemInfo() 
            {
                EItemType = _eItemType,
                EEquippableName = _eEquipableName,
                EConsumableName = EItemConsumableName.Count
            }, 
            _id
        );
    }
}
