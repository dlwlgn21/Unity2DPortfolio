using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class UI_Inventory_ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemInfo ItemInfo { get; set; }
    public Image Image { get; set; }
    public int ItemId { get; set; }
    public int SlotIdx { get; private set; }
    public int ConsumableItemCount { get; private set; }
    public TextMeshProUGUI ConsumableItemCountText { get; private set; }
    Transform _cacheParent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Image.enabled)
        {
            Managers.UI.ItemDesc.ShowItemDesc(ItemInfo, ItemId);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Image.enabled)
        {
            Managers.UI.ItemDesc.HideItemDesc();
        }
    }

    private void Awake()
    {
        Image = GetComponent<Image>();
        Image.enabled = false;
        _cacheParent = transform.parent;
        SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
        ConsumableItemCountText = Utill.GetFirstComponentInChildrenOrNull<TextMeshProUGUI>(gameObject);
        Debug.Assert(ConsumableItemCountText != null);
        ConsumableItemCountText.gameObject.SetActive(false);
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_cacheParent);
        Image.raycastTarget = true;
    }
    #endregion

    public void Swap(UI_Inventory_ItemIcon a)
    {
        Sprite tmpSprite = null;
        bool tmpImgEnabled = false;
        ItemInfo tmpIteminfo;
        int tmpItemId = 0;
        int tmpCoumsumableItemCount = 0;
        string tmpConsumableString;
        bool tmpConsumableTextEnabled = false;
        
        tmpSprite = a.Image.sprite;
        tmpImgEnabled = a.Image.enabled;
        tmpIteminfo = a.ItemInfo;
        tmpItemId = a.ItemId;
        tmpCoumsumableItemCount = a.ConsumableItemCount;
        tmpConsumableString = a.ConsumableItemCountText.text;
        tmpConsumableTextEnabled = a.ConsumableItemCountText.gameObject.activeSelf;

        a.Image.sprite = this.Image.sprite;
        a.Image.enabled = this.Image.enabled;
        a.ItemInfo = this.ItemInfo;
        a.ItemId = this.ItemId;
        a.ConsumableItemCount = this.ConsumableItemCount;
        a.ConsumableItemCountText.text = this.ConsumableItemCountText.text;
        a.ConsumableItemCountText.gameObject.SetActive(this.ConsumableItemCountText.gameObject.activeSelf);


        this.Image.sprite = tmpSprite;
        this.Image.enabled = tmpImgEnabled;
        this.ItemInfo = tmpIteminfo;
        this.ItemId = tmpItemId;
        this.ConsumableItemCount = tmpCoumsumableItemCount;
        this.ConsumableItemCountText.text = tmpConsumableString;
        this.ConsumableItemCountText.gameObject.SetActive(tmpConsumableTextEnabled);
    }
}
