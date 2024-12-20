using define;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;
using UnityEditor.Overlays;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace monster_states
{
    public abstract class BaseMonsterState : State<NormalMonsterController>
    {
        protected float _distanceFromPlayer;
        protected Vector2 _dirToPlayer;
        protected const string IDLE_ANIM_KEY = "Idle";
        protected const string RUN_ANIM_KEY = "Run";
        protected const string MELLE_ATTACK_ANIM_KEY = "MelleAttack";
        protected const string LAUNCH_ATTACK_ANIM_KEY = "LaunchAttack";
        protected const string HIT_ANIM_KEY = "Hitted";
        protected const string HIT_PARALYSIS_KEY = "HittedParalysis";
        protected const string DIE_ANIM_KEY = "Die";
        public BaseMonsterState(NormalMonsterController controller) : base(controller) {}
        public override void Excute() {  _entity.SetLookDir(); }

        public virtual void OnHittedByPlayerNormalAttack(ECharacterLookDir ePlayerLookDir, int damage, EPlayerNoramlAttackType eType)
        {
        }

        public abstract void OnAnimFullyPlayed();
        protected void PlayAnimation(ENormalMonsterState eState)
        {
            switch (eState)
            {
                case ENormalMonsterState.Idle:
                    _entity.Animator.Play(IDLE_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.Trace:
                    _entity.Animator.Play(RUN_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.MelleAttack:
                    _entity.Animator.Play(MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.LaunchAttack:
                    _entity.Animator.Play(LAUNCH_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.HitByPlayerBlockSucces:
                case ENormalMonsterState.HitByPlayerSkillKnockbackBoom:
                case ENormalMonsterState.HitByPlayerSkillParallysis:
                    _entity.Animator.Play(HIT_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.Die:
                    _entity.Animator.Play(DIE_ANIM_KEY, -1, 0f);
                    return;
            }
            Debug.Assert(false);
        }

        public virtual void MakeSlow() { }

        protected void ChangeAttackStateAcordingAttackType()
        {
            switch (_entity.EMonsterAttackType)
            {
                case ENormalMonsterAttackType.MelleAttack:
                    _entity.ChangeState(ENormalMonsterState.MelleAttack);
                    break;
                case ENormalMonsterAttackType.LaunchAttack:
                    _entity.ChangeState(ENormalMonsterState.LaunchAttack);
                    break;
                case ENormalMonsterAttackType.BothAttack:
                    break;
            }
        }
    }

    public sealed class Idle : BaseMonsterState
    {
        public Idle(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { }
        public override void Enter() { PlayAnimation(ENormalMonsterState.Idle); }
        public override void Excute()
        {
            // Monster가 추락중일 때에는 상태변환 하지 않음.
            if (_entity.RigidBody.velocity.y < 0f)
            {
                return;
            }
            base.Excute();
            if (_entity.IsPlayerInAttackZone)
            {
                ChangeAttackStateAcordingAttackType();
                return;
            }
            if (_entity.IsPlayerInTraceZone)
            {
                _entity.ChangeState(ENormalMonsterState.Trace);
            }
        }
    }

    public abstract class CanSlowState : BaseMonsterState
    {
        const float ANIM_SLOW_TIME = 0.5f;
        const float ANIM_SLOW_SPEED = 0.5f;

        protected bool _isHiitedByPlayerNormalAttack = false;
        protected float _animReturnOriginalSpeedTimer = ANIM_SLOW_TIME;

        protected CanSlowState(NormalMonsterController controller) : base(controller) {  }
        protected void DecreaseAnimSpeed()
        {
            _entity.Animator.speed = ANIM_SLOW_SPEED;
        }

        protected void RollBackAnimSpeed()
        {
            _entity.Animator.speed = 1f;
        }
        public override void MakeSlow()
        {
            _isHiitedByPlayerNormalAttack = true;
            _animReturnOriginalSpeedTimer = ANIM_SLOW_TIME;
            DecreaseAnimSpeed();
        }

        public override void Enter()
        {
            _isHiitedByPlayerNormalAttack = false;
            RollBackAnimSpeed();
        }

        public override void Excute()
        {
            base.Excute();
        }
        public override void Exit()
        {
            _isHiitedByPlayerNormalAttack = false;
            _animReturnOriginalSpeedTimer = ANIM_SLOW_TIME;
            RollBackAnimSpeed();
        }

        protected void ProcessAnimRollbackTimeCountDownIfHitted()
        {
            if (_isHiitedByPlayerNormalAttack)
            {
                _animReturnOriginalSpeedTimer -= Time.deltaTime;
                if (_animReturnOriginalSpeedTimer < 0f)
                {
                    _animReturnOriginalSpeedTimer = ANIM_SLOW_TIME;
                    _isHiitedByPlayerNormalAttack = false;
                    RollBackAnimSpeed();
                }
            }
        }
    }

    public sealed class Trace : CanSlowState
    {
        public Trace(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() {}
        public override void Enter()  
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.Trace);  
        }
        public override void FixedExcute()
        {
            if (Mathf.Abs(_entity.RigidBody.velocity.x) > Mathf.Abs(_entity.Stat.MoveSpeed * Time.fixedDeltaTime))
            {
                // PlayerNormalAttack에 맞고 AddForce가 호출 되었다는 뜻.
                // 그럴때는 일단 AddForce의 힘이 적용 된 다음에 이동하도록 했음.
                return;
            }
            Vector2 oriVelo = _entity.RigidBody.velocity;
            if (_entity.Animator.speed < 0.9f)
            {
                SetVelocityAcorddingLookDir(new Vector2(_entity.Stat.MoveSpeed * 0.5f * Time.fixedDeltaTime, oriVelo.y));
            }
            else
            {
                SetVelocityAcorddingLookDir(new Vector2(_entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelo.y));
            }
        }

        public override void Excute()
        {
            base.Excute();
            ProcessAnimRollbackTimeCountDownIfHitted();
            if (_entity.IsPlayerInAttackZone)
            {
                ChangeAttackStateAcordingAttackType();
            }
        }
        public override void Exit()
        {
            base.Exit();
            _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y);
        }


        private void SetVelocityAcorddingLookDir(Vector2 velo)
        {
            if (_entity.ELookDir == ECharacterLookDir.Left)
            {
                _entity.RigidBody.velocity = new Vector2(-velo.x, velo.y);
            }
            else
            {
                _entity.RigidBody.velocity = velo;
            }
        }


    }

    public sealed class MelleAttack : CanSlowState
    {
        public MelleAttack(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() 
        {
            _entity.ChangeState(ENormalMonsterState.Idle);
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.MelleAttack);
        }
        public override void Excute()
        {
            ProcessAnimRollbackTimeCountDownIfHitted();
        }
    }

    public sealed class LaunchAttack : CanSlowState
    {
        public LaunchAttack(NormalMonsterController controller) : base(controller) { }

        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(ENormalMonsterState.Idle);
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.LaunchAttack);
        }
        public override void Excute()
        {
            ProcessAnimRollbackTimeCountDownIfHitted();
        }
        public override void Exit()
        {
            base.Exit();
        }
    }


    public abstract class BaseHittedState : BaseMonsterState
    {
        public BaseHittedState(NormalMonsterController controller) : base(controller) { }
        public override void Excute()  { base.Excute(); }

        protected void SetVelocityZero()
        {
            Vector2 velo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(0f, velo.y);
        }

    }

    public sealed class HittedKnockbackByBlockSuccess : BaseHittedState
    {
        public HittedKnockbackByBlockSuccess(NormalMonsterController controller) : base(controller)  { }
        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(ENormalMonsterState.Idle);
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.HitByPlayerBlockSucces);
        }
    }

    public sealed class HittedParalysis : BaseHittedState
    {
        public HittedParalysis(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() { }
        public override void Enter()
        {
            PlayAnimation(ENormalMonsterState.HitByPlayerSkillParallysis);
            SetVelocityZero();
        }
    }
    public sealed class Die : BaseMonsterState
    {
        static public UnityAction<NormalMonsterController> DieEventAnimFullyPlayedHandler;
        static public UnityAction<NormalMonsterController> DieEventEnterStateHandler;
        public Die(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() 
        {
            IDeadBodyReamainable deadBodyOrNull = _entity as IDeadBodyReamainable;
            if (deadBodyOrNull != null)
                deadBodyOrNull.SpawnDeadBody();
            
            if (DieEventAnimFullyPlayedHandler != null)
                DieEventAnimFullyPlayedHandler.Invoke(_entity);
        }
        public override void Enter() 
        {
            PlayAnimation(ENormalMonsterState.Die);
            if (DieEventEnterStateHandler != null)
                DieEventEnterStateHandler.Invoke(_entity);
        }
        public override void Excute()  { }
    }

    #region BOSS_MONSTER

    public abstract class BaseBossMonsterState : State<ColossalBossMonsterController>
    {
        protected const string WAKE_ANIM_KEY = "Wake";
        protected const string HIT_ANIM_KEY = "Idle";
        protected const string RUN_ANIM_KEY = "Run";
        protected const string FIST_MELLE_ATTACK_ANIM_KEY = "FistAttack";
        protected const string SPIN_MELLE_ATTACK_ANIM_KEY = "SpinAttack";
        protected const string RANGE_MELLE_ATTACK_ANIM_KEY = "BurstAttack";
        protected const string BURFED_RANGE_MELLE_ATTACK_ANIM_KEY = "BurfedBurstAttack";
        protected const string BURF_ANIM_KEY = "Burf";
        protected const string DIE_ANIM_KEY = "Die";

        public BaseBossMonsterState(ColossalBossMonsterController entity) : base(entity) {}

        public override void Excute()
        {
        }
        protected void SetLookDirection()
        {
            _entity.SetLookDir();
        }
        public abstract void OnAnimFullyPlayed();
        protected void PlayAnimation(EColossalBossState eState)
        {
            switch (eState)
            {
                case EColossalBossState.Wake:
                    _entity.Animator.Play(WAKE_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.Run:
                    _entity.Animator.Play(RUN_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.FistMelleAttack:
                    _entity.Animator.Play(FIST_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.SpinMelleAttack:
                    _entity.Animator.Play(SPIN_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.BurstMelleAttack:
                    _entity.Animator.Play(RANGE_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.BurfedBurstMelleAttack:
                    _entity.Animator.Play(BURFED_RANGE_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.Burf:
                    _entity.Animator.Play(BURF_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.Hit:
                    _entity.Animator.Play(HIT_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.Die:
                    _entity.Animator.Play(DIE_ANIM_KEY, -1, 0f);
                    return;
                default:
                    Debug.DebugBreak();
                    break;
            }
        }
    }


    public sealed class BossColossalWake : BaseBossMonsterState
    {
        public BossColossalWake(ColossalBossMonsterController entity) : base(entity) { }

        public override void OnAnimFullyPlayed()
        {
            _entity.HealthBar.gameObject.SetActive(true);
            _entity.SetActiveBodyLights(true);
            _entity.ChangeState(EColossalBossState.Run);
        }
    }


    public sealed class BossColossalRun : BaseBossMonsterState
    {
        public BossColossalRun(ColossalBossMonsterController entity) : base(entity)   { }

        public override void OnAnimFullyPlayed() {  }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.Run);
        }
        public override void Excute()
        {
            switch (_entity.EColossalPhase)
            {
                case EColossalBossPhase.FirstPhase:
                    if (_entity.IsPlayerInBurstAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.BurstMelleAttack);
                        return;
                    }
                    if (_entity.IsPlayerInSpinAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.SpinMelleAttack);
                        return;
                    }
                    if (_entity.IsPlayerInFistAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.FistMelleAttack);
                        return;
                    }
                    break;
                case EColossalBossPhase.SecondUnder50PercentHpPhase:
                    if (_entity.IsPlayerInBurstAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.BurfedBurstMelleAttack);
                        return;
                    }
                    if (_entity.IsPlayerInSpinAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.SpinMelleAttack);
                        return;
                    }
                    if (_entity.IsPlayerInFistAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.FistMelleAttack);
                        return;
                    }
                    break;
            }

            SetLookDirection();
        }

        public override void FixedExcute()
        {
            Vector2 oriVelo = _entity.RigidBody.velocity;
            if (_entity.ELookDir == ECharacterLookDir.Left)
            {
                _entity.RigidBody.velocity = new Vector2(-_entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelo.y);
            }
            else
            {
                _entity.RigidBody.velocity = new Vector2(_entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelo.y);
            }
        }

        public override void Exit()
        {
            _entity.RigidBody.velocity = Vector2.zero;
        }
    }

    public abstract class BossAttackState : BaseBossMonsterState
    {
        protected BossAttackState(ColossalBossMonsterController entity) : base(entity)  { }
        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(EColossalBossState.Run);
        }
    }

    public sealed class BossColossalSpinAttack : BossAttackState
    {
        public BossColossalSpinAttack(ColossalBossMonsterController entity) : base(entity) {}

        public override void Enter()
        {
            PlayAnimation(EColossalBossState.SpinMelleAttack);
        }
    }

    public sealed class BossColossalFistAttack : BossAttackState
    {
        public BossColossalFistAttack(ColossalBossMonsterController entity) : base(entity) { }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.FistMelleAttack);
        }
    }

    public sealed class BossColossalBurstAttack : BossAttackState
    {
        public BossColossalBurstAttack(ColossalBossMonsterController entity) : base(entity) { }

        public override void Enter()
        {
            PlayAnimation(EColossalBossState.BurstMelleAttack);
        }
    }

    public sealed class BossColossalBurfedBurstAttack : BossAttackState
    {
        public BossColossalBurfedBurstAttack(ColossalBossMonsterController entity) : base(entity) { }

        public override void Enter()
        {
            PlayAnimation(EColossalBossState.BurfedBurstMelleAttack);
        }
    }

    public sealed class BossColossalBurf : BaseBossMonsterState
    {
        public BossColossalBurf(ColossalBossMonsterController entity) : base(entity)  {}

        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(EColossalBossState.Run);
        }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.Burf);
        }
    }

    public sealed class BossColossalHit : BaseBossMonsterState
    {
        public BossColossalHit(ColossalBossMonsterController entity) : base(entity) { }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.Hit);
        }
        public override void OnAnimFullyPlayed()
        { }
    }

    public sealed class BossColossalDie : BaseBossMonsterState
    {
        public BossColossalDie(ColossalBossMonsterController entity) : base(entity) { }

        public override void OnAnimFullyPlayed()
        {
            // TODO : 그냥 시체 스프라이트 렌더만 할 수 있게 해야함.
        }
        public override void Enter()
        {
            _entity.HealthBar.StartZeroScaleTW();
            PlayAnimation(EColossalBossState.Die);
        }
    }


    #endregion
}


