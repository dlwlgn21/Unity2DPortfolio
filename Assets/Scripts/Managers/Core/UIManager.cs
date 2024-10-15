using data;
using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Progress;

public struct ItemInfo
{
    public EItemType EItemType;
    public EItemEquippableType EEquippableType;
    public EItemConsumableType EConsumableType;
    public int ItemId;

    public ItemInfo(EItemType eItemType, 
                    EItemEquippableType eEquipType, 
                    EItemConsumableType eConType, 
                    int itemId)
    {
        EItemType = eItemType;
        EEquippableType = eEquipType;
        EConsumableType = eConType;
        ItemId = itemId;
    }
    public static bool operator ==(ItemInfo a, ItemInfo b)
    {
        if (a.EItemType == EItemType.Equippable)
        {
            return a.EItemType == b.EItemType && a.EEquippableType == b.EEquippableType && a.ItemId == b.ItemId;
        }
        else
        {
            return a.EItemType == b.EItemType && a.EConsumableType == b.EConsumableType && a.ItemId == b.ItemId;
        }
    }
    public static bool operator !=(ItemInfo a, ItemInfo b)
    {
        if (a.EItemType == EItemType.Equippable)
        {
            return a.EItemType != b.EItemType || a.EEquippableType != b.EEquippableType || a.ItemId != b.ItemId;
        }
        else
        {
            return a.EItemType != b.EItemType || a.EConsumableType != b.EConsumableType || a.ItemId != b.ItemId;
        }
    }
    public override bool Equals(object o)
    {
        Debug.Assert(false, "Don't Call This Method!!!");
        return true;
    }
    public override int GetHashCode()
    {
        Debug.Assert(false, "Don't Call This Method!!!");
        return 0;
    }
}

public class UIManager
{
    UI_Statinfo _stat;
    UI_Inventory _inven;
    UI_SkillTree _skillTree;
    UI_PlayerHUD _playerHud;
    public UnityAction UI_IventroyConsumablePushedEventHandler; 
    public UI_Inventory_ItemDescription ItemDesc { get; private set; }
    public UI_Skill_Description SkillDesc { get; private set; }

    Dictionary<string, Sprite> _spriteMap = new();

    int _sortOrder;

    public void Init()
    {
        if (_stat == null)
        {
            {
                // Stat
                GameObject go = Managers.Resources.Instantiate<GameObject>("Prefabs/UI/UI_Statinfo");
                Debug.Assert(go != null);
                Object.DontDestroyOnLoad(go);
                go.name = "UI_Statinfo";
                _stat = go.GetComponent<UI_Statinfo>();
                _stat.gameObject.SetActive(false);
            }
            {
                // Inven
                GameObject go = Managers.Resources.Instantiate<GameObject>("Prefabs/UI/UI_Inventory");
                Debug.Assert(go != null);
                Object.DontDestroyOnLoad(go);
                go.name = "UI_Inventory";
                _inven = go.GetComponent<UI_Inventory>();
                ItemDesc = Utill.GetComponentInChildrenOrNull<UI_Inventory_ItemDescription>(go, "UI_ItemDescription");
                _inven.gameObject.SetActive(false);
            }
            {
                // SkillTree
                GameObject go = Managers.Resources.Instantiate<GameObject>("Prefabs/UI/Skill/UI_SkillTree");
                Debug.Assert(go != null);
                Object.DontDestroyOnLoad(go);
                go.name = "UI_SkillTree";
                _skillTree = go.GetComponent<UI_SkillTree>();
                SkillDesc = Utill.GetComponentInChildrenOrNull<UI_Skill_Description>(go, "UI_SkillDescription");
                _skillTree.gameObject.SetActive(false);
            }
            {
                // PlayerHUD
                GameObject go = Managers.Resources.Instantiate<GameObject>("Prefabs/UI/Player/UI_PlayerHUD");
                Debug.Assert(go != null);
                Object.DontDestroyOnLoad(go);
                go.name = "UI_PlayerHUD";
                _playerHud = go.GetComponent<UI_PlayerHUD>();
            }
            Managers.Input.KeyboardHandler -= OnUIKeyDowned;
            Managers.Input.KeyboardHandler += OnUIKeyDowned;
        }
    }

    #region PushIventoryItem
    public bool TryPushItemToInventory(ItemInfo itemInfo)
    {
        Sprite sprite;
        UI_Inventory_ItemIcon emptyIcon;
        switch (itemInfo.EItemType)
        {
            case EItemType.Equippable:
                AssignSpriteEquipable(itemInfo.EEquippableType, itemInfo.ItemId, out sprite);
                emptyIcon = _inven.GetEmptyIconOrNull();
                Debug.Assert(sprite != null && emptyIcon != null);
                emptyIcon.ItemInfo = itemInfo;
                break;
            case EItemType.Consumable:
                AssignSpriteConsumable(itemInfo.EConsumableType, itemInfo.ItemId, out sprite);
                emptyIcon = _inven.GetSameCousmableIconOrNull(itemInfo.ItemId);
                if (emptyIcon == null)
                    emptyIcon = _inven.GetEmptyIconOrNull();
                emptyIcon.ItemInfo = itemInfo;
                emptyIcon.IncreaseConsuambleText();
                if (UI_IventroyConsumablePushedEventHandler != null)
                    UI_IventroyConsumablePushedEventHandler.Invoke();
                break;
            default:
                Debug.Assert(false);
                return false;

        }
        // TODO : Inventory가 꽉 찼을때 처리 이곳에서 해주어야 함.
        if (emptyIcon == null)
        {
            Debug.Log("Inventory Full!!!!");
            return false;
        }
        emptyIcon.Image.sprite = sprite;
        emptyIcon.Image.enabled = true;
        return true;
    }

    public bool TryPushDiscardItemToInventoryAt(ItemInfo itemInfo, int slotIdx, int consumableCount)
    {
        if (!IsEmptySlotAt(slotIdx))
            return false;
        Sprite sprite;
        UI_Inventory_ItemIcon icon = _inven.GetIconAtOrNull(slotIdx);
        if (icon == null)
        {
            Debug.Assert(false);
            return false;
        }

        switch (itemInfo.EItemType)
        {
            case EItemType.Equippable:
                AssignSpriteEquipable(itemInfo.EEquippableType, itemInfo.ItemId, out sprite);
                icon.ItemInfo = itemInfo;
                break;
            case EItemType.Consumable:
                AssignSpriteConsumable(itemInfo.EConsumableType, itemInfo.ItemId, out sprite);
                icon.ItemInfo = itemInfo;
                icon.AssignConsumableCount(consumableCount);
                break;
            default:
                Debug.Assert(false);
                return false;

        }
        Debug.Assert(sprite != null);
        icon.Image.sprite = sprite;
        icon.Image.enabled = true;
        return true;
    }

    public bool TryPushEquipableItemToInventoryAt(ItemInfo itemInfo, int slotIdx)
    {
        if (!IsEmptySlotAt(slotIdx))
            return false;

        Sprite sprite;
        UI_Inventory_ItemIcon emptyIcon;
        switch (itemInfo.EItemType)
        {
            case EItemType.Equippable:
                AssignSpriteEquipable(itemInfo.EEquippableType, itemInfo.ItemId, out sprite);
                emptyIcon = _inven.GetIconAtOrNull(slotIdx);
                Debug.Assert(sprite != null && emptyIcon != null);
                emptyIcon.ItemInfo = itemInfo;
                emptyIcon.Image.sprite = sprite;
                emptyIcon.Image.enabled = true;
                break;
            default:
                Debug.Assert(false);
                break;

        }
        return true;
    }
    #endregion

    #region IconHelpers
    public UI_Inventory_ItemIcon GetSpecifiedConsumableOrNull(EItemConsumableType eConsumableName, int itemId)
    {
        return _inven.GetSpecifiedConsumableOrNull(eConsumableName, itemId);
    }
    public void SwapItemIcon(int aIdx, int bIdx)
    {
        Debug.Assert(aIdx != bIdx);
        UI_Inventory_ItemIcon a = _inven.GetIconAtOrNull(aIdx);
        UI_Inventory_ItemIcon b = _inven.GetIconAtOrNull(bIdx);
        Debug.Assert(a != null && b != null);
        a.Swap(b);
    }

    public void ClearInventorySlotAt(int slotIdx)
    {
        Debug.Assert(slotIdx < UI_Inventory.INVENTORY_SLOT_COUNT);
        _inven.GetIconAtOrNull(slotIdx).Clear();
    }
    public Sprite GetEquipableItemSprite(ItemInfo itemInfo)
    {
        Sprite sprite;
        AssignSpriteEquipable(itemInfo.EEquippableType, itemInfo.ItemId, out sprite);
        Debug.Assert(sprite != null);
        return sprite;
    }

    public Sprite GetConsumableItemSprite(ItemInfo itemInfo)
    {
        Sprite sprite;
        AssignSpriteConsumable(itemInfo.EConsumableType, itemInfo.ItemId, out sprite);
        Debug.Assert(sprite != null);
        return sprite;
    }
    public Sprite GetSpriteOrNull(string key)
    {
        Debug.Assert(!string.IsNullOrEmpty(key));
        Sprite sprite;
        if (_spriteMap.TryGetValue(key, out sprite))
            return sprite;
        else
            return null;
    }
    public Sprite GetSpriteByItemInfoOrNull(ItemInfo itemInfo)
    {
        switch (itemInfo.EItemType)
        {
            case EItemType.Equippable:
                switch (itemInfo.EEquippableType)
                {
                    case EItemEquippableType.Helmet:
                        data.HelmetInfo helmetInfo;
                        Managers.Data.HelmetItemDict.TryGetValue(itemInfo.ItemId, out helmetInfo);
                        return GetSpriteOrNull(helmetInfo.iconSpritePath);
                    case EItemEquippableType.Armor:
                        data.ArmorInfo armorInfo;
                        Managers.Data.ArmorItemDict.TryGetValue(itemInfo.ItemId, out armorInfo);
                        return GetSpriteOrNull(armorInfo.iconSpritePath);
                    case EItemEquippableType.Sword:
                        data.SwordInfo swordInfo;
                        Managers.Data.SwordItemDict.TryGetValue(itemInfo.ItemId, out swordInfo);
                        return GetSpriteOrNull(swordInfo.iconSpritePath);
                    default:
                        Debug.Assert(false);
                        return null;
                }
            case EItemType.Consumable:
                switch (itemInfo.EConsumableType)
                {
                    case EItemConsumableType.Hp:
                        data.HealingPotionInfo helingPotionInfo;
                        Managers.Data.HealingPotionDict.TryGetValue(itemInfo.ItemId, out helingPotionInfo);
                        return GetSpriteOrNull(helingPotionInfo.iconSpritePath);
                    default:
                        Debug.Assert(false);
                        return null;
                }
            default:
                Debug.Assert(false);
                return null;
        }
    }
    #endregion
    public void OnUIKeyDowned()
    {
        if (Managers.Scene.ECurrentScene != define.ESceneType.MainMenu)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowOrHideStatInvenUI();
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ShowOrHideSkillUI();
            }
        }
    }
    public void RefreshStatUI()
    {
        _stat.RefreshUI();
    }

    public void SetSkillPointText(int skillPoint)
    {
        _skillTree.SetSkillPoint(skillPoint);
    }
    #region ShowUI
    void ShowOrHideStatInvenUI()
    {
        if (!_stat.gameObject.activeSelf)
        {
            _stat.gameObject.SetActive(true);
            _stat.RefreshUI();
            _inven.gameObject.SetActive(true);
            _inven.RefreshUI();
            ++_sortOrder;
            SetStatAndInvenSortOrder(_sortOrder);
        }
        else
        {
            _inven.gameObject.SetActive(false);
            _stat.gameObject.SetActive(false);
            --_sortOrder;
        }
    }
    void ShowOrHideSkillUI()
    {
        if (!_skillTree.gameObject.activeSelf)
        {
            _skillTree.gameObject.SetActive(true);
            ++_sortOrder;
            SetSkillTreeSortOrder(_sortOrder);
        }
        else
        {
            _skillTree.gameObject.SetActive(false);
            --_sortOrder;
        }
    }

    #endregion

    #region PushItems

    bool IsEmptySlotAt(int slotIdx)
    {
        return _inven.GetIconAtOrNull(slotIdx).IsEmpty();
    }
    void AssignSpriteEquipable(EItemEquippableType eType, int itemId, out Sprite sprite)
    {
        sprite = null;
        switch (eType)
        {
            case EItemEquippableType.Helmet:
                {
                    data.HelmetInfo helmetStat;
                    Managers.Data.HelmetItemDict.TryGetValue(itemId, out helmetStat);
                    Debug.Assert(helmetStat != null);
                    _spriteMap.TryGetValue(helmetStat.iconSpritePath, out sprite);
                    if (sprite == null)
                    {
                        Sprite icon = Managers.Resources.Load<Sprite>($"Art/{helmetStat.iconSpritePath}");
                        Debug.Assert(icon != null);
                        _spriteMap.Add(helmetStat.iconSpritePath, icon);
                        sprite = icon;
                    }
                    break;
                }
            case EItemEquippableType.Armor:
                {
                    data.ArmorInfo armorStat;
                    Managers.Data.ArmorItemDict.TryGetValue(itemId, out armorStat);
                    Debug.Assert(armorStat != null);
                    _spriteMap.TryGetValue(armorStat.iconSpritePath, out sprite);
                    if (sprite == null)
                    {
                        Sprite icon = Managers.Resources.Load<Sprite>($"Art/{armorStat.iconSpritePath}");
                        Debug.Assert(icon != null);
                        _spriteMap.Add(armorStat.iconSpritePath, icon);
                        sprite = icon;
                    }
                }
                break;
            case EItemEquippableType.Sword:
                {
                    data.SwordInfo swordStat;
                    Managers.Data.SwordItemDict.TryGetValue(itemId, out swordStat);
                    Debug.Assert(swordStat != null);
                    _spriteMap.TryGetValue(swordStat.iconSpritePath, out sprite);
                    if (sprite == null)
                    {
                        Sprite icon = Managers.Resources.Load<Sprite>($"Art/{swordStat.iconSpritePath}");
                        Debug.Assert(icon != null);
                        _spriteMap.Add(swordStat.iconSpritePath, icon);
                        sprite = icon;
                    }
                }
                break;
            default:
                Debug.Assert(false);
                break;
        }
        Debug.Assert(sprite != null);
    }
    void AssignSpriteConsumable(EItemConsumableType eType, int itemId, out Sprite sprite)
    {
        sprite = null;
        switch (eType)
        {
            case EItemConsumableType.Hp:
                data.HealingPotionInfo potionStat;
                Managers.Data.HealingPotionDict.TryGetValue(itemId, out potionStat);
                _spriteMap.TryGetValue(potionStat.iconSpritePath, out sprite);
                if (sprite == null)
                {
                    Sprite icon = Managers.Resources.Load<Sprite>($"Art/{potionStat.iconSpritePath}");
                    Debug.Assert(icon != null);
                    _spriteMap.Add(potionStat.iconSpritePath, icon);
                    sprite = icon;
                }
                break;
            default:
                Debug.Assert(false);
                break;
        }
        Debug.Assert(sprite != null);
    }

    void SetStatAndInvenSortOrder(int order)
    {
        Debug.Assert(order >= 0);
        //Debug.Log($"StatInven SortOrder : {_sortOrder}");
        _stat.Canvas.sortingOrder = order;
        _inven.Canvas.sortingOrder = order + 1;
    }
    void SetSkillTreeSortOrder(int order)
    {
        Debug.Assert(order >= 0);
        //Debug.Log($"SkillTree SortOrder : {_sortOrder}");
        _skillTree.Canvas.sortingOrder = order + 1;
    }
    #endregion

    public void Clear()
    {

    }
}
