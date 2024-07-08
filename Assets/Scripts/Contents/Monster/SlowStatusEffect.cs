using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowStatusEffect : NormalMonsterAttackStatusEffect
{
    public SlowStatusEffect() : base(EStatusEffectType.SLOW)
    { }

    public override void OnPlayerHitted(PlayerController pc, BaseMonsterController mc)
    {
        
    }
}