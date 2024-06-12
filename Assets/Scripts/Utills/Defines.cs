using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace define
{
    public enum ESceneType
    {
        MAIN_MENU,
        TUTORIAL,
        MAIN_PLAY,
        COUNT,
    }

    public enum ECharacterLookDir
    { 
        LEFT,
        RIGHT
    }

    public enum EColliderLayer
    {
        PLAYER = 6,
        MONSTERS = 7,
        PLATFORM = 10,
        ENV = 11,
        EVENT_BOX = 12,
        CAM_CONFINER = 13,
        LEDGE_CLIMB = 14,
        PROJECTILE = 15,
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
        Dagger,
        HeabySlicer,
        LightSlicer,
        Sweeper,
        Warden,
    }


    public enum ESoundType
    {
        SFX,
        BGM,
        COUNT
    }
}
