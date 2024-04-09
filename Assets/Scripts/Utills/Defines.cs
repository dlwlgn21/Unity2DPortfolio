using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace define
{
    public enum ECharacterLookDir
    { 
        LEFT,
        RIGHT
    }

    public enum EColliderLayer
    {
        PLAYER = 6,
        MONSTERS = 7,
        GROUND = 8,
        PLATFORM = 10,
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

    public enum EHitCameraShake
    {
        WEAK_SHAKE_2D,
        STRONG_SHAKE_2D,
        WEAK_SHAKE_3D,
        STRONG_SHAKE_3D
    }
}
