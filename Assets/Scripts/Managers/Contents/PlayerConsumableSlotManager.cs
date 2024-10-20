using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerConsumableSlotManager : MonoBehaviour
{
    List<UI_PlayerConsumableSlot> _slots = new(2);

    private void Awake()
    {
        Init();
    }

    public bool IsUsingConsumable()
    {
        if (!_slots[0].IsCanUseConsumable || !_slots[1].IsCanUseConsumable)
            return true;
        return false;
    }

    void DisacrdIfSameItemDroped(ItemInfo itemInfo, int slotIdx)
    {
        Init();
        if (slotIdx == 0)
        {
            if (itemInfo == _slots[1].Info)
            {
                _slots[1].Discard();
            }
        }
        else
        {
            if (itemInfo == _slots[0].Info)
            {
                _slots[0].Discard();
            }
        }
    }

    void OnDiscardBtnClicked(ItemInfo itemInfo)
    {
        for (int i = 0; i < 2; ++i)
        {
            if (itemInfo == _slots[i].Info)
            {
                _slots[i].Discard();
                return;
            }
        }
    }

    void Init()
    {
        if (_slots.Count == 0)
        {
            _slots.Add(Utill.GetComponentInChildrenOrNull<UI_PlayerConsumableSlot>(gameObject, "PlayerConsumableSlot01"));
            _slots.Add(Utill.GetComponentInChildrenOrNull<UI_PlayerConsumableSlot>(gameObject, "PlayerConsumableSlot02"));
            UI_PlayerConsumableSlot.SameConsumableDropEventHandelr -= DisacrdIfSameItemDroped;
            UI_PlayerConsumableSlot.SameConsumableDropEventHandelr += DisacrdIfSameItemDroped;
            UI_Inventory_ItemDiscardSlot.ItemDiscardEventHandler -= OnDiscardBtnClicked;
            UI_Inventory_ItemDiscardSlot.ItemDiscardEventHandler += OnDiscardBtnClicked;
            PlayScene.OnSceneInitEventHandelr -= InitForNextSceneLoad;
            PlayScene.OnSceneInitEventHandelr += InitForNextSceneLoad;

        }
    }

    void InitForNextSceneLoad()
    {
        for (int i = 0; i < 2; ++i)
        {
            _slots[i].InitForNextSceneLoad();
        }
    }

    //private void OnDestroy()
    //{
    //    UI_PlayerConsumableSlot.SameConsumableDropEventHandelr -= SwapIfSameItemMoving;
    //}

}
