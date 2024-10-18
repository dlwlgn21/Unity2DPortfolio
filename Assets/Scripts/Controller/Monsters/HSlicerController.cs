using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HSlicerController : NormalMonsterController, IMelleAttackable, IDeadBodyReamainable
{
    public override void Init()
    {
        base.Init();
        InitStat();
        EMonsterType = EMonsterNames.HeabySlicer;
        EMonsterAttackType = ENormalMonsterAttackType.MelleAttack;
    }
    public override void InitStat()
    {
        Stat.InitBasicStat(EMonsterNames.HeabySlicer);
        Stat.KnockbackForce = new Vector2(9f, 9f);
    }

    protected override void InitStates()
    {
        base.InitStates();
        AllocateMelleAttackState();
    }
    protected override void SetLightControllersTurnOffTimeInSec()
    {
        _attackLightController.TurnOffGraduallyLightTimeInSec = 0.3f;
        _dieController.TurnOffGraduallyLightTimeInSec = 1f;
    }
    public void AllocateMelleAttackState()
    {
        _states[(uint)ENormalMonsterState.MelleAttack] = new monster_states.MelleAttack(this);
    }

    public void SpawnDeadBody()
    {
        InstantiateDeadBody("Prefabs/Monsters/DeadBody/HSlicerDeadBody");
    }


    void OnAttackSwing1Timing()
    {
        AddForceToFront();
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_1_PATH);
    }
    void OnAttackSwing2Timing()
    {
        AddForceToFront();
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_2_PATH);
    }

    void AddForceToFront()
    {
        if (ELookDir == ECharacterLookDir.Left)
            RigidBody.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
        else
            RigidBody.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
    }


}