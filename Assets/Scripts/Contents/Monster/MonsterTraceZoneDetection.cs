using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MonsterTraceZoneDetection : MonsterZoneDetection
{
    ITraceZoneDetectable _detectable;

    private void Awake()
    {
        _detectable = transform.parent.GetComponent<NormalMonsterController>();
        Debug.Assert(_detectable != null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnterPlayer(collision))
        {
            _detectable.OnTraceZoneEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsEnterPlayer(collision))
        {
            _detectable.OnTraceZoneExit();
        }
    }
}
