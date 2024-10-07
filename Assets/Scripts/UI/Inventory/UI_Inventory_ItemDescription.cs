using data;
using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public sealed class UI_Inventory_ItemDescription : UI_BaseDescription
{
    Image _itemImg;

    public void ShowItemDesc(ItemInfo itemInfo)
    {
        SetImagesEnabled(true);
        switch (itemInfo.EItemType)
        {
            case EItemType.Equippable:
                FullfillEquipableItemDesc(itemInfo.EEquippableType, itemInfo.ItemId);
                break;
            case EItemType.Consumable:
                FullfillConsumableItemDesc(itemInfo.EConsumableType, itemInfo.ItemId);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }


    void FullfillEquipableItemDesc(EItemEquippableType eType, int itemId)
    {
        switch (eType)
        {
            case EItemEquippableType.Helmet:
                {
                    data.HelmetInfo helmetStat;
                    Managers.Data.HelmetItemDict.TryGetValue(itemId, out helmetStat);
                    Debug.Assert(helmetStat != null);
                    _itemImg.sprite = Managers.UI.GetSpriteOrNull(helmetStat.iconSpritePath);
                    SetTexts(helmetStat);
                    break;
                }
            case EItemEquippableType.Armor:
                {
                    data.ArmorInfo armorStat;
                    Managers.Data.ArmorItemDict.TryGetValue(itemId, out armorStat);
                    Debug.Assert(armorStat != null);
                    _itemImg.sprite = Managers.UI.GetSpriteOrNull(armorStat.iconSpritePath);
                    SetTexts(armorStat);
                    break;

                }
            case EItemEquippableType.Sword:
                {
                    data.SwordInfo swordStat;
                    Managers.Data.SwordItemDict.TryGetValue(itemId, out swordStat);
                    Debug.Assert(swordStat != null);
                    _itemImg.sprite = Managers.UI.GetSpriteOrNull(swordStat.iconSpritePath);
                    SetTexts(swordStat);
                    break;
                }
            default:
                Debug.Assert(false);
                break;
        }
    }
    void FullfillConsumableItemDesc(EItemConsumableType eType, int itemId)
    {
        switch (eType)
        {
            case EItemConsumableType.Hp:
                {
                    data.HealingPotionInfo potionStat;
                    Managers.Data.HealingPotionDict.TryGetValue(itemId, out potionStat);
                    _itemImg.sprite =  Managers.UI.GetSpriteOrNull(potionStat.iconSpritePath);
                    SetTexts(potionStat);
                    break;
                }
            default:
                Debug.Assert(false);
                break;
        }
    }

    protected override void SetImagesEnabled(bool isEnabled)
    {
        base.SetImagesEnabled(isEnabled);
        _itemImg.enabled = isEnabled;
    }
    void SetTexts(data.BaseItemInfoData info)
    {
        _nameText.text = info.name;
        _descText.text = info.description;
    }

    protected override void Init()
    {
        _itemImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "ItemImg");
        Debug.Assert(_itemImg != null);
    }
}
