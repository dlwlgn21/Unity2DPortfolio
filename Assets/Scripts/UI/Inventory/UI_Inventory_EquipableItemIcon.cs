using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class UI_Inventory_EquipableItemIcon : UI_Inventory_BaseItemIcon
{
    [SerializeField] EItemEquippableType _eEquipableType;
    PlayerController _pc;
    protected override void Init()
    {
    }

    public void EqiupItem(ItemInfo itemInfo)
    {
        if (_pc == null)
        {
            _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Debug.Assert(_pc != null);
        }
        Debug.Log("Called EqiupItem()");
        Sprite sprite = Managers.UI.GetEquipableItemSprite(itemInfo);
        ItemInfo = itemInfo;
        Image.enabled = true;
        Image.sprite = sprite;
        transform.localPosition = Vector3.zero;
        _pc.OnItemEqiuped(itemInfo);
        Managers.UI.RefreshStatUI();
    }

    public override void Clear()
    {
        _pc.OnItemUnequiped(_eEquipableType);
        Managers.UI.RefreshStatUI();
        Image.enabled = false;
    }
}
