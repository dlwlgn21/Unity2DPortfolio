using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaunchAttackable
{
    public void AllocateLaunchAttackState();
    public void OnValidLaunchAnimTiming();
}
