using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatusEffectType
{
    NONE,
    KNOCKBACK,
    BURN,
    SLOW,
    PARALLYSIS
}

public abstract class NormalMonsterAttackStatusEffect
{
    public EStatusEffectType EAttackStatusEffectType { get; protected set; }

    public NormalMonsterAttackStatusEffect(EStatusEffectType eType)
    {
        EAttackStatusEffectType = eType;
    }
    public abstract void OnPlayerHitted(PlayerController pc, BaseMonsterController mc);
}
