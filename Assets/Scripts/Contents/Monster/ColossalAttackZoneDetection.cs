using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public enum EColossalAttackType
{
    FIST,
    SPIN,
    BURST,
    COUNT
}

public class ColossalAttackZoneDetection : MonsterZoneDetection
{
    public static UnityAction<EColossalAttackType> ColossalAttackZoneEnterEvnetHandler;
    public static UnityAction<EColossalAttackType> ColossalAttackZoneExitEvnetHandler;
    [SerializeField] private EColossalAttackType _eAttackType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnterPlayer(collision))
        {
            ColossalAttackZoneEnterEvnetHandler?.Invoke(_eAttackType);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsEnterPlayer(collision))
        {
            ColossalAttackZoneExitEvnetHandler?.Invoke(_eAttackType);
        }
    }
}
