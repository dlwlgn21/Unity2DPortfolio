using define;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public sealed class UI_PlayerConsumableIcon : MonoBehaviour
{
    public Image Image { get; set; }
    public TextMeshProUGUI CountText { get; set; }
    private void Awake()
    {
        Init();
        Managers.UI.UI_IventroyConsumablePushedEventHandler -= OnConsumablePushed;
        Managers.UI.UI_IventroyConsumablePushedEventHandler += OnConsumablePushed;
    }

    public void OnDropConsumableIcon(Sprite sprite, string countText)
    {
        Debug.Assert(sprite != null && !string.IsNullOrEmpty(countText));
        Image.enabled = true;
        Image.sprite = sprite;
        CountText.text = countText;
        Managers.Tween.StartUIScaleTW(Image.transform, OnScaleTWEnd);
    }

    public void ResetItemIcon()
    {
        if (Image == null)
            Init();
        Image.enabled = false;
        CountText.text = "";
    }
    public bool IsPossibleConsum()
    {
        Debug.Assert(Image != null && CountText != null);
        return Image.enabled;
    }

    void Init()
    {
        Image = GetComponent<Image>();
        Image.enabled = false;
        CountText = Utill.GetFirstComponentInChildrenOrNull<TextMeshProUGUI>(gameObject);
        CountText.text = "";
        Debug.Assert(Image != null && CountText != null);
    }

    void OnConsumablePushed()
    {
        // TODO : 여기 ItemId 매직넘버 고쳐야 함.
        UI_Inventory_ItemIcon itemIcon = Managers.UI.GetSpecifiedConsumableOrNull(EItemConsumableType.Hp, 1);
        if (IsPossibleConsum() && itemIcon.Image.sprite == Image.sprite)
        {
            CountText.text = itemIcon.ConsumableItemCount.ToString();
        }
    }

    void OnScaleTWEnd()
    {
        Managers.Tween.EndToOneUIScaleTW(Image.transform);
    }
}
