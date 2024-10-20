using UnityEngine;

namespace define
{
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
    public enum ESkillType
    {
        Roll,
        Spawn_Reaper_LV1,
        Spawn_Reaper_LV2,
        Spawn_Reaper_LV3,
        Spawn_Shooter_LV1,
        Spawn_Shooter_LV2,
        Spawn_Shooter_LV3,
        Cast_BlackFlame_LV1,
        Cast_BlackFlame_LV2,
        Cast_BlackFlame_LV3,
        Cast_SwordStrike_LV1,
        Cast_SwordStrike_LV2,
        Cast_SwordStrike_LV3,
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
        Item = 20,
        CamLookUpOrDownZone = 21,
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
