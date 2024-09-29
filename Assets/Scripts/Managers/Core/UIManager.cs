using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public struct ItemInfo
{
    public EItemType EItemType;
    public EItemEquippableName EEquippableName;
    public EItemConsumableName EConsumableName;
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
    
    public void PushItemToInventory(ItemInfo itemInfo, int itemId)
    {
        Image emptyImg = _inven.GetEmptySlotIconOrNull();
        Sprite sprite = null;

        switch (itemInfo.EItemType)
        {
            case EItemType.Equippable:
                AssignSpriteEquipable(itemInfo.EEquippableName, itemId, out sprite);
                break;
            case EItemType.Consumable:
                AssignSpriteConsumable(itemInfo.EConsumableName, itemId, out sprite);
                break;
            default:
                Debug.Assert(false);
                break;

        }
        Debug.Assert(sprite != null);
        emptyImg.sprite = sprite;
        emptyImg.enabled = true;
    }

    public void OnIKeyDowned()
    {
        if (Managers.Scene.ECurrentScene != define.ESceneType.MAIN_MENU)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowStatIvenUI();
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
    void AssignSpriteEquipable(EItemEquippableName eType, int itemId, out Sprite sprite)
    {
        sprite = null;
        switch (eType)
        {
            case EItemEquippableName.Helmet:
                {
                    data.HelmetStat helmetStat;
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
            case EItemEquippableName.Armor:
                {
                    data.ArmorStat armorStat;
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
            case EItemEquippableName.Sword:
                {
                    data.SwordStat swordStat;
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
    void AssignSpriteConsumable(EItemConsumableName eType, int itemId, out Sprite sprite)
    {
        sprite = null;
        switch (eType)
        {
            case EItemConsumableName.Hp:
                data.HealingPotionStat potionStat;
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
