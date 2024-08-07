using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace define
{
    public enum ESceneType
    {
        MAIN_MENU,
        TUTORIAL,
        ABANDON_ROAD_SCENE,
        COLOSSAL_BOSS_CAVE_SCENE,
        COUNT,
    }

    public enum ECharacterLookDir
    { 
        LEFT,
        RIGHT
    }

    public enum EColliderLayer
    {
        PLAYER_BODY = 6,
        MONSTERS_BODY = 7,
        PLAYER_ATTACK_BOX = 8,
        PLATFORM = 10,
        ENV = 11,
        EVENT_BOX = 12,
        CAM_CONFINER = 13,
        LEDGE_CLIMB = 14,
        PROJECTILE = 15,
        MONSTER_ATTACK_BOX = 16,
        MONSTER_BETWEEN_PLAYER_BLOCKING_BOX = 17,
        BOSS_COLOSSAL_BODY = 18,
        BOSS_COLOSSAL_ATTACK_BOX = 19,
    }


    public enum EKeyboardInputType
    { 
        NO_KEY_DOWN,
        ANY_KEY_DOWN,
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
        SFX,
        BGM,
        COUNT
    }
}
