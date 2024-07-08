using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallysisStatusEffect : NormalMonsterAttackStatusEffect
{
    public ParallysisStatusEffect() : base(EStatusEffectType.PARALLYSIS)
    { }

    public override void OnPlayerHitted(PlayerController pc, BaseMonsterController mc)
    {
    }
}