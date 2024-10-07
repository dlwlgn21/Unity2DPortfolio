using UnityEngine;

namespace define
{
    static class DoTweenValueContainer
    {
        const float T_VAL = 1.1f;
        static public Vector3 TWEEN_SCALE_END_VALUE = new(T_VAL, T_VAL, T_VAL);
        static public float TWEEN_SCALE_END_TIME_IN_SEC = 0.1f;
        static public float TWEEN_SCALE_END_TIME_QUARTER_IN_SEC = 0.25f;
    }
    public enum ESceneType
    {
        MainMenu,
        Tutorial,
        AbandonLoadScene,
        ColossalBossCaveScene,
        Count,
    }

    public enum ECharacterLookDir
    { 
        Left,
        Right
    }
    public enum ESlkillType
    {
        Spawn_Reaper,
        Spawn_Panda,
        Casting_BlackFlame,
        Casting_Grenade,
        Count
    }
    public enum EAttackStatusEffect
    {
        None,
        Knockback,
        Blind,
        Burn,
        Slow,
        Parallysis,
    }

    public enum EColliderLayer
    {
        PlayerBody = 6,
        MonsterBody = 7,
        PlayerAttackBox = 8,
        Platform = 10,
        Env = 11,
        EventBox = 12,
        CamConfiner = 13,
        LedgeClimb = 14,
        Projectile = 15,
        MonsterAttackBox = 16,
        MonsterBetweenPlayerBlockingBox = 17,
        BossColossalBody = 18,
        BossColossalAttackBox = 19,
    }

    public enum EMonsterNames
    {
        Archer = 1,
        Blaster,
        CagedShoker,
        RedGhoul,
        HeabySlicer,
        Gunner,
        Shielder,
        Warden,
        Flamer,
        BossColossal,
    }

    public enum EProjectileState
    {
        Muzzle,
        Projectile,
        Hit
    }
    public enum ESoundType
    {
        Sfx,
        Bgm,
        Count
    }


    public enum EItemType
    {
        Equippable,
        Consumable,
        Count
    }

    public enum EItemEquippableType
    { 
        Helmet,
        Armor,
        Sword,
        Count
    }


    public enum EItemConsumableType
    { 
        Hp,
        Count
    }
}
