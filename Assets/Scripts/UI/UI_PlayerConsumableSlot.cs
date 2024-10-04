using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using define;
using UnityEngine.Events;
using System;

public class UI_PlayerConsumableSlot : MonoBehaviour, IDropHandler
{
    public static UnityAction<ItemInfo, int> SameConsumableDropEventHandelr;
    PlayerController _pc;
    UI_PlayerConsumableIcon _icon;
    public ItemInfo Info { get; private set; }
    public int SlotIdx { get; private set; }
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        Init();
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Debug.Assert(_pc != null);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Init();
        GameObject dragedObject = eventData.pointerDrag;
        if (dragedObject != null)
        {
            UI_Inventory_ItemIcon dragedIcon = dragedObject.GetComponent<UI_Inventory_ItemIcon>();
            if (dragedIcon != null && dragedIcon.ItemInfo.EItemType == EItemType.Consumable)
            {
                if (SameConsumableDropEventHandelr != null)
                {
                    SameConsumableDropEventHandelr.Invoke(dragedIcon.ItemInfo, SlotIdx);
                    Info = dragedIcon.ItemInfo;
                    _icon.OnDropConsumableIcon(dragedIcon.Image.sprite, dragedIcon.ConsumableItemCountText.text);
                }
            }
        }
    }

    public void Discard()
    {
        _icon.ResetItemIcon();
        Info = new ItemInfo();
    }
    public void Swap(UI_PlayerConsumableSlot a)
    {
        Debug.Assert(a != null);
        ItemInfo tmpItemInfo = a.Info;
        bool tmpImgEnabled = a._icon.Image.enabled;
        Sprite tmpSprite = a._icon.Image.sprite;
        string tmpText = a._icon.CountText.text;

        a.Info = this.Info;
        a._icon.Image.enabled = this._icon.Image.enabled;
        a._icon.Image.sprite = this._icon.Image.sprite;
        a._icon.CountText.text = this._icon.CountText.text;

        this.Info = tmpItemInfo;
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
                    Discard();
                }
                return true;
            }
        }
        return false;
    }

    void Init()
    {
        if (_icon == null)
        {
            _icon = Utill.GetFirstComponentInChildrenOrNull<UI_PlayerConsumableIcon>(gameObject);
            Debug.Assert(_icon != null);
            Info = new ItemInfo(EItemType.Count, EItemEquippableType.Count, EItemConsumableType.Count, int.MinValue);
            SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
            Managers.Input.KeyboardHandler -= OnKeyDown;
            Managers.Input.KeyboardHandler += OnKeyDown;
        }
    }
    //private void OnDestroy()
    //{
    //    if (SameConsumableDropEventHandelr != null)
    //    {
    //        SameConsumableDropEventHandelr = null;
    //    }
    //    Managers.Input.KeyboardHandler -= OnKeyDown;
    //}
}
