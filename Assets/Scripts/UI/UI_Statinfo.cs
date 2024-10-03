using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Statinfo : MonoBehaviour
{
    TextMeshProUGUI _damageValueText;
    TextMeshProUGUI _defenceValueText;
    PlayerController _pc;
    private void Awake()
    {
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
        _damageValueText.text = $"{_pc.Stat.Attack + _pc.Stat.SwordPlusDamage} (+{_pc.Stat.SwordPlusDamage})";
        _defenceValueText.text = $"{_pc.Stat.Defence + _pc.Stat.HelmetPlusDefence + _pc.Stat.ArmorPlusDefence} (+{_pc.Stat.HelmetPlusDefence + _pc.Stat.ArmorPlusDefence})";
    }
}
