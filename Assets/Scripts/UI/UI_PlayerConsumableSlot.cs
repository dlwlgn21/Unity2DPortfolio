using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using define;
using UnityEngine.Events;
using System;
using DG.Tweening;

public class UI_PlayerConsumableSlot : MonoBehaviour, IDropHandler
{
    public static float CONSUMABLE_COOL_TIME_IN_SEC = 5f;
    public static UnityAction<ItemInfo, int> SameConsumableDropEventHandelr;
    public static UnityAction UseConsumableHandler;
    PlayerController _pc;
    UI_PlayerConsumableIcon _icon;
    UI_PlayerCoolTimer _coolTimer;
    public ItemInfo Info { get; private set; }
    public int SlotIdx { get; private set; }

    public bool IsCanUseConsumable { get; private set; } = true;
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
    void OnKeyDown()
    {
        if (SlotIdx == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TryUseConsumableItem();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TryUseConsumableItem();
            }
        }
    }

    bool TryUseConsumableItem()
    {
        StartScaleTW();
        // TODO : 여기 ItemId 매직넘버 고쳐야 함.
        if (IsCanUseConsumable && _icon.IsPossibleConsum() && _pc.Stat.HP < _pc.Stat.MaxHP)
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
                    _coolTimer.StartCoolTime(CONSUMABLE_COOL_TIME_IN_SEC);
                    StartCoroutine(AfterGivenCoolTimeCanUseConsuamble(CONSUMABLE_COOL_TIME_IN_SEC));

                    if (UseConsumableHandler != null)
                        UseConsumableHandler.Invoke();
                    if (int.Parse(_icon.CountText.text) <= 0)
                    {
                        Discard();
                    }
                    return true;
                }
            }
        }

        return false;
    }

    void Init()
    {
        if (_icon == null)
        {
            _icon = Utill.GetFirstComponentInChildrenOrNull<UI_PlayerConsumableIcon>(gameObject);
            _coolTimer = Utill.GetFirstComponentInChildrenOrNull<UI_PlayerCoolTimer>(gameObject);
            Debug.Assert(_icon != null && _coolTimer != null);
            Info = new ItemInfo(EItemType.Count, EItemEquippableType.Count, EItemConsumableType.Count, int.MinValue);
            SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
            Managers.Input.KeyboardHandler -= OnKeyDown;
            Managers.Input.KeyboardHandler += OnKeyDown;
        }
    }

    IEnumerator AfterGivenCoolTimeCanUseConsuamble(float coolTimeInSec)
    {
        IsCanUseConsumable = false;
        yield return new WaitForSeconds(coolTimeInSec);
        IsCanUseConsumable = true;
    }

    void StartScaleTW()
    {
        transform.DOScale(DoTweenValueContainer.TWEEN_SCALE_END_VALUE, DoTweenValueContainer.TWEEN_SCALE_END_TIME_IN_SEC).SetEase(Ease.InOutElastic).OnComplete(OnScaleTWEnd);
    }
    void OnScaleTWEnd()
    {
        transform.DOScale(Vector3.one, DoTweenValueContainer.TWEEN_SCALE_END_TIME_IN_SEC).SetEase(Ease.InOutElastic);
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
