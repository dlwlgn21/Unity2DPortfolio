using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPlayerHpBar : UIHealthBar
{
    protected override void Init()
    {
        SetFullHpBarRatio();
    }
}
