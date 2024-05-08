using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonsterHPBar : UIHealthBar
{
    protected override void Init()
    {
        _stat = transform.parent.GetComponent<MonsterStat>();
        Debug.Assert(_stat != null);
    }
}
