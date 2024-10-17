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
        AddForceByLookDir();
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_1_PATH);
    }
    void OnAttackSwing2Timing()
    {
        AddForceByLookDir();
        Managers.Sound.Play(DataManager.SFX_MONSTER_SWING_2_PATH);
    }

    void AddForceByLookDir()
    {
        if (ELookDir == ECharacterLookDir.Left)
            AddKnockbackForce(new Vector2(-5f, 1f));
        else
            AddKnockbackForce(new Vector2(5f, 1f));
    }
}