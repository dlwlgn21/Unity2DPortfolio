using define;
using monster_states;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HSlicerController : NormalMonsterController, IMelleAttackable, IDeadBodyReamainable
{

    static readonly Vector2 sRushForce = new Vector2(5f, 1f);
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
        AddForceToFront(sRushForce);
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_MONSTER_SWING_1_PATH);
    }
    void OnAttackSwing2Timing()
    {
        AddForceToFront(sRushForce * 0.35f);
        Managers.Sound.Play(Managers.Data.SFXKeyContainer.SFX_MONSTER_SWING_2_PATH);
    }

    void AddForceToFront(Vector2 force)
    {
        if (ELookDir == ECharacterLookDir.Left)
            RigidBody.AddForce(new Vector2(-force.x, force.y), ForceMode2D.Impulse);
        else
            RigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public override void OnPlayerBlockSuccess()
    {
        _attackLightController.ForceToStopCoroutineAndTurnOffLight();
        ChangeState(ENormalMonsterState.HitByPlayerBlockSucces);
        AddKnockbackForceOppossiteByPlayer(new Vector3(6f, 3f));
    }
}