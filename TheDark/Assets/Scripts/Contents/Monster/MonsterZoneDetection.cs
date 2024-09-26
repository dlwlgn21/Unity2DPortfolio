using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterZoneDetection : MonoBehaviour
{
    protected bool IsEnterPlayer(Collider2D collider)
    {
        if (collider.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            return true;
        }
        return false;
    }
}
