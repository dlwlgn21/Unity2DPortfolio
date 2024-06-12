using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPlayerHpBar : UIHealthBar
{
    public override void Init()
    {
        SetFullHpBarRatio();
    }
}
