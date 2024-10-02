using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerConsumableSlotManager : MonoBehaviour
{
    List<UI_PlayerConsumableSlot> _slots = new(2);

    private void Awake()
    {
        _slots.Add(Utill.GetComponentInChildrenOrNull<UI_PlayerConsumableSlot>(gameObject, "PlayerConsumableSlot01"));
        _slots.Add(Utill.GetComponentInChildrenOrNull<UI_PlayerConsumableSlot>(gameObject, "PlayerConsumableSlot02"));
        UI_PlayerConsumableSlot.SameConsumableDropEventHandelr += SwapIfSameItemMoving;
        
    }

    bool SwapIfSameItemMoving(ItemInfo itemInfo, int slotIdx)
    {
        if (_slots[0].ItemInfo == _slots[1].ItemInfo)
        {
            _slots[0].Swap(_slots[1]);
            return true;
        }
        return false;
    }


    private void OnDestroy()
    {
        UI_PlayerConsumableSlot.SameConsumableDropEventHandelr -= SwapIfSameItemMoving;
    }

}
