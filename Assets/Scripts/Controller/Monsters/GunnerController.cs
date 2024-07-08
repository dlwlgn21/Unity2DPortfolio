using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunnerController : NormalMonsterController, ILaunchAttackable
{
    private Transform _launchPoint;
    private readonly Vector2 PROJECTILE_KNOCKBACK_FORCE = new Vector2(4f, 4f);
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Gunner;
        EMonsterAttackType = ENormalMonsterAttackType.LAUNCH_ATTACK;
        _launchPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LaunchPoint");
    }
    public override void InitStat()
    {
        Stat.Init(EMonsterNames.Gunner);
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
            .GetMonsterKnockbackProjectile(PROJECTILE_KNOCKBACK_FORCE)
            .OnValidShootAnimTiming(ELookDir, _launchPoint.position);
    }
}