using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class UI_Statinfo : MonoBehaviour
{
    public Canvas Canvas { get; set; }
    TextMeshProUGUI _levelValueText;
    TextMeshProUGUI _damageValueText;
    TextMeshProUGUI _defenceValueText;
    PlayerController _pc;
    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
        _levelValueText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "LevelValueText");
        _damageValueText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "DamageValueText");
        _defenceValueText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "DefenceValueText");
    }
    private void Start()
    {
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void RefreshUI()
    {
        if (_pc == null)
            _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _levelValueText.text = $"{_pc.Stat.Level}";
        _damageValueText.text = $"{_pc.Stat.Attack + _pc.Stat.SwordPlusDamage} (+{_pc.Stat.SwordPlusDamage})";
        _defenceValueText.text = $"{_pc.Stat.Defence + _pc.Stat.HelmetPlusDefence + _pc.Stat.ArmorPlusDefence} (+{_pc.Stat.HelmetPlusDefence + _pc.Stat.ArmorPlusDefence})";
    }
}
