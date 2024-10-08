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
        protected readonly static string IDLE_ANIM_KEY = "Idle";
        protected readonly static string RUN_ANIM_KEY = "Run";
        protected readonly static string MELLE_ATTACK_ANIM_KEY = "MelleAttack";
        protected readonly static string LAUNCH_ATTACK_ANIM_KEY = "LaunchAttack";
        protected readonly static string HIT_ANIM_KEY = "Hitted";
        protected readonly static string HIT_PARALYSIS_KEY = "HittedParalysis";
        protected readonly static string DIE_ANIM_KEY = "Die";
        public BaseMonsterState(NormalMonsterController controller) : base(controller) {}
        public override void Excute() {  _entity.SetLookDir(); }

        public virtual void OnHittedByPlayerNormalAttack(ECharacterLookDir ePlayerLookDir, int damage, EPlayerNoramlAttackType eType)
        {
            AdjustKnockbackAcoddingLookDir(eType);
            if (_entity.Stat.HP <= 0)
            {
                _entity.ChangeState(ENormalMonsterState.DIE);
            }
        }

        public abstract void OnAnimFullyPlayed();
        protected void PlayAnimation(ENormalMonsterState eState)
        {
            switch (eState)
            {
                case ENormalMonsterState.IDLE:
                    _entity.Animator.Play(IDLE_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.TRACE:
                    _entity.Animator.Play(RUN_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.MELLE_ATTACK:
                    _entity.Animator.Play(MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.LAUNCH_ATTACK:
                    _entity.Animator.Play(LAUNCH_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
                case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                    _entity.Animator.Play(HIT_ANIM_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                    _entity.Animator.Play(HIT_PARALYSIS_KEY, -1, 0f);
                    return;
                case ENormalMonsterState.DIE:
                    _entity.Animator.Play(DIE_ANIM_KEY, -1, 0f);
                    return;
            }
            Debug.Assert(false);
        }
        protected void CalculateDistanceFromPlayer()
        {
            _dirToPlayer = _entity.PlayerTransform.position - _entity.transform.position;
            _distanceFromPlayer = _dirToPlayer.magnitude;
        }


        private void AdjustKnockbackAcoddingLookDir(EPlayerNoramlAttackType eType)
        {
            if (_entity.ELookDir == ECharacterLookDir.Left)
            {
                switch (eType)
                {
                    case EPlayerNoramlAttackType.ATTACK_1:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE, ForceMode2D.Impulse);
                        break;
                    case EPlayerNoramlAttackType.ATTACK_2:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE * PlayerController.NORMAL_ATTACK_2_FORCE_COEFF, ForceMode2D.Impulse);
                        break;
                    case EPlayerNoramlAttackType.ATTACK_3:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE * PlayerController.NORMAL_ATTACK_3_FORCE_COEFF, ForceMode2D.Impulse);
                        break;
                    case EPlayerNoramlAttackType.BACK_ATTACK:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_LEFT_KNOCKBACK_FORCE * PlayerController.BACK_ATTACK_FORCE_COEFF, ForceMode2D.Impulse);
                        break;
                }
            }
            else
            {
                switch (eType)
                {
                    case EPlayerNoramlAttackType.ATTACK_1:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_LEFT_KNOCKBACK_FORCE, ForceMode2D.Impulse);
                        break;
                    case EPlayerNoramlAttackType.ATTACK_2:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_LEFT_KNOCKBACK_FORCE * PlayerController.NORMAL_ATTACK_2_FORCE_COEFF, ForceMode2D.Impulse);
                        break;
                    case EPlayerNoramlAttackType.ATTACK_3:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_LEFT_KNOCKBACK_FORCE * PlayerController.NORMAL_ATTACK_3_FORCE_COEFF, ForceMode2D.Impulse);
                        break;
                    case EPlayerNoramlAttackType.BACK_ATTACK:
                        _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_RIGHT_KNOCKBACK_FORCE * PlayerController.BACK_ATTACK_FORCE_COEFF, ForceMode2D.Impulse);
                        break;
                }
            }
        }

        protected void ChangeAttackStateAcordingAttackType()
        {
            switch (_entity.EMonsterAttackType)
            {
                case ENormalMonsterAttackType.MELLE_ATTACK:
                    _entity.ChangeState(ENormalMonsterState.MELLE_ATTACK);
                    break;
                case ENormalMonsterAttackType.LAUNCH_ATTACK:
                    _entity.ChangeState(ENormalMonsterState.LAUNCH_ATTACK);
                    break;
                case ENormalMonsterAttackType.BOTH_ATTACK:
                    break;
            }
        }
    }

    public class Idle : BaseMonsterState
    {
        public Idle(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { }
        public override void Enter() { PlayAnimation(ENormalMonsterState.IDLE); }
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
                _entity.ChangeState(ENormalMonsterState.TRACE);
            }
        }


    }

    public abstract class CanSlowState : BaseMonsterState
    {
        public readonly static float ANIM_SLOW_TIME = 0.5f;
        public readonly static float ANIM_SLOW_SPEED = 0.5f;

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
        public override void OnHittedByPlayerNormalAttack(ECharacterLookDir ePlayerLookDir, int damage, EPlayerNoramlAttackType eType)
        {
            base.OnHittedByPlayerNormalAttack(ePlayerLookDir, damage, eType);
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

    public class Trace : CanSlowState
    {
        public Trace(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() {}
        public override void Enter()  
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.TRACE);  
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

    public class MelleAttack : CanSlowState
    {
        public MelleAttack(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() 
        {
            _entity.ChangeState(ENormalMonsterState.IDLE);
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.MELLE_ATTACK);
        }
        public override void Excute()
        {
            ProcessAnimRollbackTimeCountDownIfHitted();
        }
    }

    public class LaunchAttack : CanSlowState
    {
        public LaunchAttack(NormalMonsterController controller) : base(controller) { }

        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(ENormalMonsterState.IDLE);
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.LAUNCH_ATTACK);
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
        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(ENormalMonsterState.IDLE);
        }
        protected void SetVelocityZero()
        {
            Vector2 velo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(0f, velo.y);
        }

    }

    public abstract class CanKnockback : BaseHittedState
    {
        protected float _knockbackForce;

        public CanKnockback(NormalMonsterController controller) : base(controller) { }


        public override void Enter()
        {
            SetVelocityZero();
            KnockbackMonster();
        }

        protected void KnockbackMonster()
        {
            CalculateDistanceFromPlayer();
            if (_dirToPlayer.x < 0f)
            {
                _entity.RigidBody.AddForce(Vector2.right * _knockbackForce, ForceMode2D.Impulse);
            }
            else
            {
                _entity.RigidBody.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse);
            }
        }

    }

    public class HittedKnockbackByBlockSuccess : CanKnockback
    {
        public HittedKnockbackByBlockSuccess(NormalMonsterController controller) : base(controller) 
        {
            _knockbackForce = PlayerController.BLOCK_SUCCESS_KNOCKBACK_X_FORCE;
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS);
        }
    }
    public class HittedKnockbackByBomb : CanKnockback
    {
        public HittedKnockbackByBomb(NormalMonsterController controller) : base(controller) 
        {
            _knockbackForce = PlayerController.KNOCKBACK_BOMB_FORCE;
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(ENormalMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB);
        }
    }
    public class HittedParalysis : BaseHittedState
    {
        public HittedParalysis(NormalMonsterController controller) : base(controller) { }
        public override void Enter()
        {
            PlayAnimation(ENormalMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS);
            SetVelocityZero();
        }


    }
    public class Die : BaseMonsterState
    {
        static public UnityAction<NormalMonsterController> MonsterDieEventHandelr;
        public Die(NormalMonsterController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        {
            MonsterDieEventHandelr?.Invoke(_entity);
        }
        public override void Enter() 
        { 
            PlayAnimation(ENormalMonsterState.DIE);
            _entity.OnDie();
        }
        public override void Excute() { }
    }



    #region BOSS_MONSTER

    public abstract class BaseBossMonsterState : State<ColossalBossMonsterController>
    {
        protected readonly static string WAKE_ANIM_KEY = "Wake";
        protected readonly static string IDLE_ANIM_KEY = "Idle";
        protected readonly static string RUN_ANIM_KEY = "Run";
        protected readonly static string FIST_MELLE_ATTACK_ANIM_KEY = "FistAttack";
        protected readonly static string SPIN_MELLE_ATTACK_ANIM_KEY = "SpinAttack";
        protected readonly static string RANGE_MELLE_ATTACK_ANIM_KEY = "BurstAttack";
        protected readonly static string BURFED_RANGE_MELLE_ATTACK_ANIM_KEY = "BurfedBurstAttack";
        protected readonly static string BURF_ANIM_KEY = "Burf";
        protected readonly static string DIE_ANIM_KEY = "Die";

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
                case EColossalBossState.WAKE:
                    _entity.Animator.Play(WAKE_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.RUN:
                    _entity.Animator.Play(RUN_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.FIST_MELLE_ATTACK:
                    _entity.Animator.Play(FIST_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.SPIN_MELLE_ATTACK:
                    _entity.Animator.Play(SPIN_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.BURST_MEELE_ATTACK:
                    _entity.Animator.Play(RANGE_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.BURFED_BURST_ATTACK:
                    _entity.Animator.Play(BURFED_RANGE_MELLE_ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.BURF:
                    _entity.Animator.Play(BURF_ANIM_KEY, -1, 0f);
                    return;
                case EColossalBossState.DIE:
                    _entity.Animator.Play(DIE_ANIM_KEY, -1, 0f);
                    return;
            }
        }
    }


    public class BossColossalWake : BaseBossMonsterState
    {
        public BossColossalWake(ColossalBossMonsterController entity) : base(entity) { }

        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(EColossalBossState.RUN);
        }
    }


    public class BossColossalRun : BaseBossMonsterState
    {
        public BossColossalRun(ColossalBossMonsterController entity) : base(entity)   { }

        public override void OnAnimFullyPlayed() {  }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.RUN);
        }
        public override void Excute()
        {
            switch (_entity.EColossalPhase)
            {
                case EColossalBossPhase.FIRST_PHASE:
                    if (_entity.IsPlayerInBurstAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.BURST_MEELE_ATTACK);
                        return;
                    }
                    if (_entity.IsPlayerInSpinAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.SPIN_MELLE_ATTACK);
                        return;
                    }
                    if (_entity.IsPlayerInFistAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.FIST_MELLE_ATTACK);
                        return;
                    }
                    break;
                case EColossalBossPhase.SECOND_UNDER_50_PERCENT_HP_PHASE:
                    if (_entity.IsPlayerInBurstAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.BURFED_BURST_ATTACK);
                        return;
                    }
                    if (_entity.IsPlayerInSpinAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.SPIN_MELLE_ATTACK);
                        return;
                    }
                    if (_entity.IsPlayerInFistAttackZone)
                    {
                        _entity.ChangeState(EColossalBossState.FIST_MELLE_ATTACK);
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
            _entity.ChangeState(EColossalBossState.RUN);
        }
    }

    public class BossColossalSpinAttack : BossAttackState
    {
        public BossColossalSpinAttack(ColossalBossMonsterController entity) : base(entity) {}

        public override void Enter()
        {
            PlayAnimation(EColossalBossState.SPIN_MELLE_ATTACK);
        }
    }

    public class BossColossalFistAttack : BossAttackState
    {
        public BossColossalFistAttack(ColossalBossMonsterController entity) : base(entity) { }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.FIST_MELLE_ATTACK);
        }
    }

    public class BossColossalBurstAttack : BossAttackState
    {
        public BossColossalBurstAttack(ColossalBossMonsterController entity) : base(entity) { }

        public override void Enter()
        {
            PlayAnimation(EColossalBossState.BURST_MEELE_ATTACK);
        }
    }

    public class BossColossalBurfedBurstAttack : BossAttackState
    {
        public BossColossalBurfedBurstAttack(ColossalBossMonsterController entity) : base(entity) { }

        public override void Enter()
        {
            PlayAnimation(EColossalBossState.BURFED_BURST_ATTACK);
        }
    }

    public class BossColossalBurf : BaseBossMonsterState
    {
        public BossColossalBurf(ColossalBossMonsterController entity) : base(entity)  {}

        public override void OnAnimFullyPlayed()
        {
            _entity.ChangeState(EColossalBossState.RUN);
        }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.BURF);
        }
    }

    public class BossColossalDie : BaseBossMonsterState
    {
        public BossColossalDie(ColossalBossMonsterController entity) : base(entity) { }

        public override void OnAnimFullyPlayed()
        {
            // TODO : 그냥 시체 스프라이트 렌더만 할 수 있게 해야함.
        }
        public override void Enter()
        {
            PlayAnimation(EColossalBossState.DIE);
        }
    }


    #endregion
}


