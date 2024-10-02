using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public struct ItemInfo
{
    public EItemType EItemType;
    public EItemEquippableType EEquippableType;
    public EItemConsumableType EConsumableType;
    public int ItemId;

    public void Init()
    {
        EItemType = define.EItemType.Count;
        EEquippableType = define.EItemEquippableType.Count;
        EConsumableType = define.EItemConsumableType.Count;
        ItemId = int.MinValue;
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
    public UI_Inventory_ItemDesc ItemDesc { get; private set; }

    Dictionary<string, Sprite> _spriteMap = new();
    public void Init()
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
            ItemDesc = Utill.GetComponentInChildrenOrNull<UI_Inventory_ItemDesc>(go, "UI_ItemDescription");
            _inven.gameObject.SetActive(false);
        }

        Managers.Input.KeyboardHandler += OnIKeyDowned;
    }
    
    public void PushItemToInventory(ItemInfo itemInfo)
    {
        Sprite sprite = null;
        UI_Inventory_ItemIcon emptyIcon = null;
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
                break;
            default:
                Debug.Assert(false);
                break;

        }
        // TODO : Inventory가 꽉 찼을때 처리 이곳에서 해주어야 함.
        if (emptyIcon == null)
        {
            Debug.Log("Inventory Full!!!!");
            return;
        }
        emptyIcon.Image.sprite = sprite;
        emptyIcon.Image.enabled = true;

    }

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

    public void ClearSlotAt(int slotIdx)
    {
        Debug.Assert(slotIdx < UI_Inventory.INVENTORY_SLOT_COUNT);
        _inven.GetIconAtOrNull(slotIdx).Clear();
    }
    public Sprite GetEquipableItemSprite(ItemInfo itemInfo, EItemEquippableType eType)
    {
        Sprite sprite;
        AssignSpriteEquipable(eType, itemInfo.ItemId, out sprite);
        Debug.Assert(sprite != null);
        return sprite;
    }

    public void OnIKeyDowned()
    {
        if (Managers.Scene.ECurrentScene != define.ESceneType.MAIN_MENU)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowIvenUI();
            }
        }
    }
    public void Clear()
    {
        Managers.Input.KeyboardHandler -= OnIKeyDowned;
    }
    void ShowStatIvenUI()
    {
        if (!_stat.gameObject.activeSelf)
        {
            _stat.gameObject.SetActive(true);
            _inven.gameObject.SetActive(true);
            _stat.RefreshUI();
            _inven.RefreshUI();

        }
        else
        {
            _stat.gameObject.SetActive(false);
            _inven.gameObject.SetActive(false);
        }
    }

    void ShowIvenUI()
    {
        if (!_inven.gameObject.activeSelf)
        {
            _inven.gameObject.SetActive(true);
            _inven.RefreshUI();

        }
        else
        {
            _inven.gameObject.SetActive(false);
        }
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


    #region PushItems
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
    #endregion
}
