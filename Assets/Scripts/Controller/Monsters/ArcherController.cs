using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : NormalMonsterController, ILaunchAttackable
{
    private Transform _launchPoint;
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Archer;
        EMonsterAttackType = ENormalMonsterAttackType.LAUNCH_ATTACK;
        _launchPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LaunchPoint");
    }
    public override void InitStat()
    {
        Stat.Init(EMonsterNames.Archer);
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateLaunchAttackState();
    }
    public void AllocateLaunchAttackState()
    {
        _states[(uint)ENormalMonsterState.LAUNCH_ATTACK] = new monster_states.LaunchAttack(this);
    }

    public void OnValidLaunchAnimTiming()
    {
        Managers.ProjectilePool
            .GetMonsterDamageProjectile(Stat.Attack)
            .OnValidShootAnimTiming(ELookDir, _launchPoint.position);
    }
}