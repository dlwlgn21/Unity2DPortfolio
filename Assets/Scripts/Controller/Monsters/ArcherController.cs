using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ArcherController : NormalMonsterController, ILaunchAttackable
{
    private Transform _launchPoint;
    private readonly Vector2 PROJECTILE_KNOCKBACK_FORCE = new Vector2(4f, 4f);
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.Archer;
        EMonsterAttackType = ENormalMonsterAttackType.LaunchAttack;
        _launchPoint = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "LaunchPoint");
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.Archer);
        Stat.KnockbackForce = PROJECTILE_KNOCKBACK_FORCE;
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