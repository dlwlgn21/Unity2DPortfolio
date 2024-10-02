using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using define;
using UnityEngine.Events;
using System;

public class UI_PlayerConsumableSlot : MonoBehaviour, IDropHandler
{
    public static Func<ItemInfo, int, bool> SameConsumableDropEventHandelr;
    PlayerController _pc;
    UI_PlayerConsumableIcon _icon;
    public ItemInfo ItemInfo { get; private set; }
    public int SlotIdx { get; private set; }
    private void Awake()
    {
        _icon = Utill.GetFirstComponentInChildrenOrNull<UI_PlayerConsumableIcon>(gameObject);
        Debug.Assert(_icon != null);
        ItemInfo.Init();
        SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
        Managers.Input.KeyboardHandler += OnKeyDown;

    }

    private void Start()
    {
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Assert(_pc != null);
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dragedObject = eventData.pointerDrag;
        if (dragedObject != null)
        {
            UI_Inventory_ItemIcon dragedIcon = dragedObject.GetComponent<UI_Inventory_ItemIcon>();
            if (dragedIcon != null && dragedIcon.ItemInfo.EItemType == EItemType.Consumable)
            {
                if (SameConsumableDropEventHandelr != null)
                {
                    ItemInfo = dragedIcon.ItemInfo;
                    if (SameConsumableDropEventHandelr.Invoke(dragedIcon.ItemInfo, SlotIdx) == false)
                    {
                        _icon.OnDropConsumableIcon(dragedIcon.Image.sprite, dragedIcon.ConsumableItemCountText.text);
                    }
                }
            }
        }
    }
    public void Swap(UI_PlayerConsumableSlot a)
    {
        ItemInfo tmpItemInfo = a.ItemInfo;
        bool tmpImgEnabled = a._icon.Image.enabled;
        Sprite tmpSprite = a._icon.Image.sprite;
        string tmpText = a._icon.CountText.text;

        a.ItemInfo = this.ItemInfo;
        a._icon.Image.enabled = this._icon.Image.enabled;
        a._icon.Image.sprite = this._icon.Image.sprite;
        a._icon.CountText.text = this._icon.CountText.text;

        this.ItemInfo = tmpItemInfo;
        this._icon.Image.enabled = tmpImgEnabled;
        this._icon.Image.sprite = tmpSprite;
        this._icon.CountText.text = tmpText;
    }

    void OnKeyDown()
    {
        if (SlotIdx == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && _icon.IsPossibleConsum() && _pc.Stat.HP < _pc.Stat.MaxHP)
            {
                UseConsumableItem();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha2) && _icon.IsPossibleConsum() && _pc.Stat.HP < _pc.Stat.MaxHP)
            {
                UseConsumableItem();
            }
        }
    }

    bool UseConsumableItem()
    {
        UI_Inventory_ItemIcon itemIcon = Managers.UI.GetSpecifiedConsumableOrNull(EItemConsumableType.Hp, 1);
        if (itemIcon != null)
        {
            if (itemIcon.ConsumableItemCount > 0)
            {
                itemIcon.DecreaseConsuambleText();
                data.HealingPotionInfo info = Managers.Data.HealingPotionDict[itemIcon.ItemInfo.ItemId];
                Debug.Assert(info != null);
                _icon.CountText.text = itemIcon.ConsumableItemCount.ToString();
                _pc.OnCousumableItemUsed(EItemConsumableType.Hp, info.healAmount);
                if (int.Parse(_icon.CountText.text) <= 0)
                {
                    _icon.ResetItemIcon();
                    ItemInfo.Init();
                }
                return true;
            }
        }
        return false;
    }

    private void OnDestroy()
    {
        if (SameConsumableDropEventHandelr != null)
        {
            SameConsumableDropEventHandelr = null;
        }
        Managers.Input.KeyboardHandler -= OnKeyDown;

    }
}
