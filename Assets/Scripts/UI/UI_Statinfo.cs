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
        _damageValueText.text = $"{_pc.Stat.Attack}";
        _defenceValueText.text = $"{_pc.Stat.Defence}";
    }
}
