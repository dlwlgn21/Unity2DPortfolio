using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class UI_Inventory_ItemIcon : UI_Inventory_BaseItemIcon
{
    public int SlotIdx { get; private set; }
    public int ConsumableItemCount { get; private set; }
    public TextMeshProUGUI ConsumableItemCountText { get; private set; }

    protected override void Init()
    {
        SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
        ConsumableItemCountText = Utill.GetFirstComponentInChildrenOrNull<TextMeshProUGUI>(gameObject);
        Debug.Assert(ConsumableItemCountText != null);
        ConsumableItemCountText.gameObject.SetActive(false);
    }


    public override void Clear()
    {
        ItemInfo = new ItemInfo();
        Image.enabled = false;
        ConsumableItemCountText.gameObject.SetActive(false);
    }

    public void AssignConsumableCount(int count)
    {
        if (ItemInfo.EItemType == define.EItemType.Consumable)
        {
            ConsumableItemCountText.gameObject.SetActive(true);
            ConsumableItemCount = count;
            ConsumableItemCountText.text = ConsumableItemCount.ToString();
        }
    }
    public void IncreaseConsuambleText()
    {
        if (ItemInfo.EItemType == define.EItemType.Consumable)
        {
            ConsumableItemCountText.gameObject.SetActive(true);
            ++ConsumableItemCount;
            ConsumableItemCountText.text = ConsumableItemCount.ToString();
        }
    }
    public void DecreaseConsuambleText()
    {
        if (ItemInfo.EItemType == define.EItemType.Consumable)
        {
             --ConsumableItemCount;
            if (ConsumableItemCount <= 0)
            {
                ConsumableItemCountText.gameObject.SetActive(false);
                Image.enabled = false;
                return;
            }
            ConsumableItemCountText.text = ConsumableItemCount.ToString();
        }
    }

    #region ItemDrag
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        ConsumableItemCountText.enabled = false;
    }


    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ConsumableItemCountText.enabled = true;
    }
    #endregion

    public void Swap(UI_Inventory_ItemIcon a)
    {
        Sprite tmpSprite = null;
        bool tmpImgEnabled = false;
        ItemInfo tmpIteminfo;
        int tmpCoumsumableItemCount = 0;
        string tmpConsumableString;
        bool tmpConsumableTextEnabled = false;
        
        tmpSprite = a.Image.sprite;
        tmpImgEnabled = a.Image.enabled;
        tmpIteminfo = a.ItemInfo;
        tmpCoumsumableItemCount = a.ConsumableItemCount;
        tmpConsumableString = a.ConsumableItemCountText.text;
        tmpConsumableTextEnabled = a.ConsumableItemCountText.gameObject.activeSelf;

        a.Image.sprite = this.Image.sprite;
        a.Image.enabled = this.Image.enabled;
        a.ItemInfo = this.ItemInfo;
        a.ConsumableItemCount = this.ConsumableItemCount;
        a.ConsumableItemCountText.text = this.ConsumableItemCountText.text;
        a.ConsumableItemCountText.gameObject.SetActive(this.ConsumableItemCountText.gameObject.activeSelf);


        this.Image.sprite = tmpSprite;
        this.Image.enabled = tmpImgEnabled;
        this.ItemInfo = tmpIteminfo;
        this.ConsumableItemCount = tmpCoumsumableItemCount;
        this.ConsumableItemCountText.text = tmpConsumableString;
        this.ConsumableItemCountText.gameObject.SetActive(tmpConsumableTextEnabled);
    }
}
