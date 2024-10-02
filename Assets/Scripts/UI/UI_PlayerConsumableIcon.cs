using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_PlayerConsumableIcon : MonoBehaviour
{
    public Image Image { get; set; }
    public TextMeshProUGUI CountText { get; set; }
    private void Awake()
    {
        Image = GetComponent<Image>();
        Image.enabled = false;
        CountText = Utill.GetFirstComponentInChildrenOrNull<TextMeshProUGUI>(gameObject);
        CountText.text = "";
        Debug.Assert(Image != null && CountText != null);
    }

    public void OnDropConsumableIcon(Sprite sprite, string countText)
    {
        Debug.Assert(sprite != null && !string.IsNullOrEmpty(countText));
        Image.enabled = true;
        Image.sprite = sprite;
        CountText.text = countText;
    }

    public void ResetItemIcon()
    {
        Image.enabled = false;
        CountText.text = "";
    }
    public bool IsPossibleConsum()
    {
        return Image.enabled;
    }
}
