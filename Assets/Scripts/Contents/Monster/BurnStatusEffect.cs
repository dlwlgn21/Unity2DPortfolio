using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnStatusEffect : NormalMonsterAttackStatusEffect
{
    public BurnStatusEffect() : base(EStatusEffectType.BURN)
    { }

    public override void OnPlayerHitted(PlayerController pc, BaseMonsterController mc)
    {
    }
}