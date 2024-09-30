using data;
using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_ItemDesc : MonoBehaviour
{
    Image _backgroundImg;
    TextMeshProUGUI _nameText;
    Image _itemImg;
    TextMeshProUGUI _descText;
    private void Awake()
    {
        _backgroundImg = GetComponent<Image>();
        _nameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "NameText");
        _itemImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "ItemImg");
        _descText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "DescriptionText");
        Debug.Assert(_backgroundImg != null && _nameText != null && _itemImg != null && _descText != null);
        SetImagesEnabled(false);
    }

    public void ShowItemDesc(ItemInfo itemInfo, int itemId)
    {
        SetImagesEnabled(true);
        switch (itemInfo.EItemType)
        {
            case EItemType.Equippable:
                FullfillEquipableItemDesc(itemInfo.EEquippableName, itemId);
                break;
            case EItemType.Consumable:
                FullfillConsumableItemDesc(itemInfo.EConsumableName, itemId);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    public void HideItemDesc()
    {
        SetImagesEnabled(false);
        _nameText.text = "";
        _descText.text = "";
    }

    void FullfillEquipableItemDesc(EItemEquippableName eType, int itemId)
    {
        switch (eType)
        {
            case EItemEquippableName.Helmet:
                {
                    data.HelmetInfo helmetStat;
                    Managers.Data.HelmetItemDict.TryGetValue(itemId, out helmetStat);
                    Debug.Assert(helmetStat != null);
                    _itemImg.sprite = Managers.UI.GetSpriteOrNull(helmetStat.iconSpritePath);
                    SetTexts(helmetStat);
                    break;
                }
            case EItemEquippableName.Armor:
                {
                    data.ArmorInfo armorStat;
                    Managers.Data.ArmorItemDict.TryGetValue(itemId, out armorStat);
                    Debug.Assert(armorStat != null);
                    _itemImg.sprite = Managers.UI.GetSpriteOrNull(armorStat.iconSpritePath);
                    SetTexts(armorStat);
                    break;

                }
            case EItemEquippableName.Sword:
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
    void FullfillConsumableItemDesc(EItemConsumableName eType, int itemId)
    {
        switch (eType)
        {
            case EItemConsumableName.Hp:
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

    void SetImagesEnabled(bool isEnabled)
    {
        _backgroundImg.enabled = isEnabled;
        _itemImg.enabled = isEnabled;
    }
    void SetTexts(data.ItemInfoData info)
    {
        _nameText.text = info.name;
        _descText.text = info.description;
    }
}
