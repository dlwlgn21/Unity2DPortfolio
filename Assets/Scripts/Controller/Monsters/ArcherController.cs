using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ArcherController : NormalMonsterController, ILaunchAttackable, IDeadBodyReamainable
{
    private Transform _launchPoint;
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
    protected override void SetLightControllersTurnOffTimeInSec()
    {
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.4f;
        _dieController.TurnOffGraduallyLightTimeInSec = 1f;
    }
    public void OnValidLaunchAnimTiming()
    {
        Managers.ProjectilePool
            .GetMonsterProjectile()
            .OnValidShootAnimTiming(ELookDir, _launchPoint.position, this);
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_MONSTER_ARCHER_GUNNER_LUANCH_PATH);
    }

    public void SpawnDeadBody()
    {
        InstantiateDeadBody("Prefabs/Monsters/DeadBody/ArcherDeadBody");
    }
}