using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inventory_ItemSlot : MonoBehaviour, IDropHandler
{
    UI_Inventory_ItemIcon _icon;
    public int SlotIdx { get; private set; }
    private void Awake()
    {
        _icon = Utill.GetFirstComponentInChildrenOrNull<UI_Inventory_ItemIcon>(gameObject);
        Debug.Log($"{gameObject.name}'s Icon : {_icon.gameObject.name}");
        SlotIdx = int.Parse(gameObject.name.Substring(gameObject.name.Length - 2)) - 1;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name}'s OnDrop!!");
        // TODO : 자기 자신이 PointerClick에 들어감여..
        GameObject dragedObject = eventData.pointerDrag;
        if (dragedObject != null)
        {
            UI_Inventory_ItemIcon dragedIcon = dragedObject.GetComponent<UI_Inventory_ItemIcon>();
            if (dragedIcon != null && SlotIdx != dragedIcon.SlotIdx)
            {
                Managers.UI.SwapItemIcon(SlotIdx, dragedIcon.SlotIdx);
            }
        }
    }
}
