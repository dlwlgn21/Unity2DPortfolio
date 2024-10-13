using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunnerController : NormalMonsterController, ILaunchAttackable
{
    private Transform _launchPoint;
    private const float PROJECTILE_SLOW_TIME_IN_SEC = 2f;
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Gunner;
        EMonsterAttackType = ENormalMonsterAttackType.LaunchAttack;
        _launchPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LaunchPoint");
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.Gunner);
        Stat.SlowTimeInSec = PROJECTILE_SLOW_TIME_IN_SEC;
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateLaunchAttackState();
    }
    public void AllocateLaunchAttackState()
    {
        _states[(uint)ENormalMonsterState.LaunchAttack] = new monster_states.LaunchAttack(this);
    }

    public void OnValidLaunchAnimTiming()
    {
        Managers.ProjectilePool
            .GetMonsterProjectile()
            .OnValidShootAnimTiming(ELookDir, _launchPoint.position, this);
    }
}