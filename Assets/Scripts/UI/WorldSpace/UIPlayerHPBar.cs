using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHPBar : UIHealthBar
{
    protected override void Init()
    {
        _stat = transform.parent.GetComponent<PlayerStat>();
        Debug.Assert(_stat != null);
    }
}
