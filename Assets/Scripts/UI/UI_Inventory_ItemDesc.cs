using define;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UI_Inventory_ItemDesc : MonoBehaviour
{
    TextMeshProUGUI _nameText;
    Image _itemImg;
    TextMeshProUGUI _descText;

    private void Awake()
    {
        _nameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "NameText");
        _itemImg = Utill.GetComponentInChildrenOrNull<Image>(gameObject, "ItemImg");
        _descText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "DescriptionText");
        Debug.Assert(_nameText != null && _itemImg != null && _descText != null);
    }

    public void ShowItemDesc(ItemInfo itemInfo, int itemId)
    {

    }


    void AssignSpriteEquipable(EItemEquippableName eType, int itemId)
    {
        switch (eType)
        {
            case EItemEquippableName.Helmet:
                {
                    data.HelmetStat helmetStat;
                    Managers.Data.HelmetItemDict.TryGetValue(itemId, out helmetStat);
                    Debug.Assert(helmetStat != null);

                    break;
                }
            case EItemEquippableName.Armor:
                {
                    data.ArmorStat armorStat;
                    Managers.Data.ArmorItemDict.TryGetValue(itemId, out armorStat);
                    Debug.Assert(armorStat != null);
  
                }
                break;
            case EItemEquippableName.Sword:
                {
                    data.SwordStat swordStat;
                    Managers.Data.SwordItemDict.TryGetValue(itemId, out swordStat);
                    Debug.Assert(swordStat != null);
                }
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    void AssignSpriteConsumable(EItemConsumableName eType, int itemId)
    {
        switch (eType)
        {
            case EItemConsumableName.Hp:
                data.HealingPotionStat potionStat;
                Managers.Data.HealingPotionDict.TryGetValue(itemId, out potionStat);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

}
