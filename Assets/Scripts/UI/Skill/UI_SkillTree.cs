using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public sealed class UI_SkillTree : MonoBehaviour
{
    TextMeshProUGUI _skillPointText;
    
    private void Awake()
    {
        _skillPointText = Utill.GetComponentInChildrenOrNull<TextMeshProUGUI>(gameObject, "SkillPointText");
    }


    public void SetSkillPoint(int skillPoint)
    {
        _skillPointText.text = skillPoint.ToString();
    }

}
