using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunnerController : NormalMonsterController, ILaunchAttackable
{
    private Transform _launchPoint;
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
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateLaunchAttackState();
    }
    protected override void SetLightControllersTurnOffTimeInSec()
    {
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.3f;
        _dieController.TurnOffGraduallyLightTimeInSec = 1f;
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
        Managers.Sound.Play(DataManager.SFX_MONSTER_ARCHER_GUNNER_LUANCH_PATH);
    }

    void OnDieSoundTiming()
    {
        Managers.Sound.Play(DataManager.SFX_MONSTER_DIE_EXPOLOSION_3);
    }
}