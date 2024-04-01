using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHPBar : UIHealthBar
{
    protected override void Init()
    {
        mStat = transform.parent.GetComponent<PlayerStat>();
        Debug.Assert(mStat != null);
    }
}
