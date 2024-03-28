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
}
