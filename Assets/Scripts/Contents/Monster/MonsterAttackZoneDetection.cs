using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MonsterAttackZoneDetection : MonsterZoneDetection
{
    private IAttackZoneDetectable _iDetacteable;
    private void Awake()
    {
        _iDetacteable = transform.parent.GetComponent<NormalMonsterController>();
        Debug.Assert(_iDetacteable != null);  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnterPlayer(collision))
        {
            _iDetacteable.OnEnterAttackZone();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsEnterPlayer(collision))
        {
            _iDetacteable.OnExitAttackZone();
        }
    }
}
