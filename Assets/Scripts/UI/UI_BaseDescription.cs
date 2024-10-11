using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_BaseDescription : MonoBehaviour
{
    protected Image _backgroundImg;
    protected TextMeshProUGUI _nameText;
    protected TextMeshProUGUI _descText;

    private void Awake()
    {
        _backgroundImg = GetComponent<Image>();
        _nameText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "NameText");
        _descText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "DescriptionText");
        Debug.Assert(_backgroundImg != null && _nameText != null && _descText != null);
        Init();
        SetImagesEnabled(false);
    }


    protected abstract void Init();

    protected virtual void SetImagesEnabled(bool isEnabled)
    {
        _backgroundImg.enabled = isEnabled;
    }

    public virtual void HideDescription()
    {
        SetImagesEnabled(false);
        _nameText.text = "";
        _descText.text = "";
    }
}
