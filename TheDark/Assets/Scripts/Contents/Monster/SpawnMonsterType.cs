using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsterType : MonoBehaviour
{
    [SerializeField] private EMonsterNames _eMonsterType;
    public EMonsterNames EMonsterType { get { return _eMonsterType; }}
}
