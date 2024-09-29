using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_Inventory_ItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image _image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_image.enabled)
        {
            Debug.Log($"{gameObject.name} PointerEnter!!");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_image.enabled)
        {
            Debug.Log($"{gameObject.name} PointerExit!!");
        }
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
}
