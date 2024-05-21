using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIPlayerRollCollTimer : MonoBehaviour
{
    private Image _coolTimeImg;
    private Color _showColor;
    private Color _hideColor;
    private PlayerController _pc;
    private void Start()
    {
        _coolTimeImg = GetComponent<Image>();
        _showColor = _coolTimeImg.color;
        _hideColor = new Color(_showColor.r, _showColor.g, _showColor.b, 0f);
        _coolTimeImg.color = _hideColor;
        _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void StartCoolTime()
    {
        _coolTimeImg.color = _showColor;
        _coolTimeImg.DOFillAmount(0f, _pc.RollCollTime).OnComplete(OnFillAmountTWEnd);
        
    }


    private void OnFillAmountTWEnd()
    {
        _coolTimeImg.color = _hideColor;
        _coolTimeImg.fillAmount = 1f;
    }

}
