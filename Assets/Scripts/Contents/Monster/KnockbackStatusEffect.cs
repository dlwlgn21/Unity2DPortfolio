using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackStatusEffect : NormalMonsterAttackStatusEffect
{
    public KnockbackStatusEffect() : base(EStatusEffectType.KNOCKBACK)
    {}

    public override void OnPlayerHitted(PlayerController pc, BaseMonsterController mc)
    {

    }
}
