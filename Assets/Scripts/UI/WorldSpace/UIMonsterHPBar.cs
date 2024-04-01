using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterHPBar : UIHealthBar
{
    protected override void Init()
    {
        mStat = transform.parent.GetComponent<MonsterStat>();
        Debug.Assert(mStat != null);
    }
}
