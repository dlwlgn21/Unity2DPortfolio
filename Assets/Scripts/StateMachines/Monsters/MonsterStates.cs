using define;
using DG.Tweening;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace monster_states
{
    public abstract class BaseMonsterState : State<BaseMonsterController>
    {
        protected float _distanceFromPlayer;
        protected Vector2 _dirToPlayer;
        protected readonly static string IDLE_ANIM_KEY = "Idle";
        protected readonly static string RUN_ANIM_KEY = "Run";
        protected readonly static string ATTACK_ANIM_KEY = "Attack";
        protected readonly static string HIT_ANIM_KEY = "Hitted";
        protected readonly static string HIT_PARALYSIS_KEY = "HittedParalysis";
        protected readonly static string DIE_ANIM_KEY = "Die";
        public BaseMonsterState(BaseMonsterController controller) : base(controller) {}
        public override void Excute() {  _entity.SetLookDir(); }

        public virtual void OnHittedByPlayerNormalAttack(ECharacterLookDir ePlayerLookDir, int damage, EPlayerNoramlAttackType eType)
        {
            AdjustKnockbackAcoddingLookDir(eType);
            if (_entity.Stat.HP <= 0)
            {
                _entity.ChangeState(EMonsterState.DIE);
            }
        }

        protected bool IsAnimEnd()
        {
            if (_entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                return true;
            }
            return false;
        }
        protected void PlayAnimation(EMonsterState eState)
        {
            switch (eState)
            {
                case EMonsterState.IDLE:
                    _entity.Animator.Play(IDLE_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.TRACE:
                    _entity.Animator.Play(RUN_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.ATTACK:
                    _entity.Animator.Play(ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
                case EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                    _entity.Animator.Play(HIT_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                    _entity.Animator.Play(HIT_PARALYSIS_KEY, -1, 0f);
                    return;
                case EMonsterState.DIE:
                    _entity.Animator.Play(DIE_ANIM_KEY, -1, 0f);
                    return;
            }
        }
        protected void CalculateDistanceFromPlayer()
        {
            _dirToPlayer = _entity.PlayerTransform.position - _entity.transform.position;
            _distanceFromPlayer = _dirToPlayer.magnitude;
        }


        private void AdjustKnockbackAcoddingLookDir(EPlayerNoramlAttackType eType)
        {
            if (_entity.ELookDir == ECharacterLookDir.LEFT)
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
    }

    public class Idle : BaseMonsterState
    {
        public Idle(BaseMonsterController controller) : base(controller) { }
        public override void Enter() { PlayAnimation(EMonsterState.IDLE); }
        public override void Excute()
        {
            // Monster�� �߶����� ������ ���º�ȯ ���� ����.
            if (_entity.RigidBody.velocity.y < 0f)
            {
                return;
            }
            base.Excute();
            CalculateDistanceFromPlayer();
            Debug.DrawRay(_entity.transform.position + Vector3.up * 0.5f, _dirToPlayer.normalized * _entity.AwarenessRangeToTrace, Color.blue);
            if (_distanceFromPlayer <= _entity.AwarenessRangeToTrace)
            {
                _entity.ChangeState(EMonsterState.TRACE);
            }
        }
    }

    public abstract class CanSlowState : BaseMonsterState
    {
        public readonly static float ANIM_SLOW_TIME = 0.5f;
        public readonly static float ANIM_SLOW_SPEED = 0.5f;

        protected bool _isHiitedByPlayerNormalAttack = false;
        protected float _animReturnOriginalSpeedTimer = ANIM_SLOW_TIME;

        protected CanSlowState(BaseMonsterController controller) : base(controller) {  }
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
        }

        public override void Excute()
        {
            base.Excute();
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
        public override void Exit()
        {
            _isHiitedByPlayerNormalAttack = false;
            _animReturnOriginalSpeedTimer = ANIM_SLOW_TIME;
            RollBackAnimSpeed();
        }
    }

    public class Trace : CanSlowState
    {
        public Trace(BaseMonsterController controller) : base(controller) { }

        public override void Enter()  
        {
            base.Enter();
            PlayAnimation(EMonsterState.TRACE);  
        }
        public override void FixedExcute()
        {
            if (Mathf.Abs(_entity.RigidBody.velocity.x) > Mathf.Abs(_entity.Stat.MoveSpeed * Time.fixedDeltaTime))
            {
                // PlayerNormalAttack�� �°� AddForce�� ȣ�� �Ǿ��ٴ� ��.
                // �׷����� �ϴ� AddForce�� ���� ���� �� ������ �̵��ϵ��� ����.
                return;
            }
            Vector2 oriVelo = _entity.RigidBody.velocity;
            if (_entity.Animator.speed < 0.9f)
            {
                _entity.RigidBody.velocity = new Vector2(_dirToPlayer.normalized.x * (_entity.Stat.MoveSpeed * 0.5f) * Time.fixedDeltaTime, oriVelo.y);
            }
            else
            {
                _entity.RigidBody.velocity = new Vector2(_dirToPlayer.normalized.x * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelo.y);
            }
        }

        public override void Excute()
        {
            base.Excute();
            CalculateDistanceFromPlayer();
            if (_distanceFromPlayer < _entity.AwarenessRangeToAttack)
            {
                _entity.ChangeState(EMonsterState.ATTACK);
                return;
            }
            Debug.DrawRay(_entity.transform.position + Vector3.up * 0.5f, _dirToPlayer.normalized * _entity.AwarenessRangeToTrace, Color.blue);
            Debug.DrawRay(_entity.transform.position, _dirToPlayer.normalized * _entity.AwarenessRangeToAttack, Color.red);
        }
        public override void Exit()
        {
            base.Exit();
            _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y);
        }
    }

    public class Attack : CanSlowState
    {
        public static UnityAction MonsterAttackStartEventHandler;
        public static UnityAction MonsterAttackEndEventHandler;
        public Attack(BaseMonsterController controller) : base(controller) { }

        public void OnAttackAnimFullyPlayed() 
        {
            _entity.ChangeState(EMonsterState.IDLE); 
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.ATTACK);
            MonsterAttackStartEventHandler?.Invoke();
        }
        public override void Excute()
        {
            base.Excute();

        }
        public override void Exit()
        {
            base.Exit();
            MonsterAttackEndEventHandler?.Invoke();
        }
    }


    public abstract class BaseHittedState : BaseMonsterState
    {
        public BaseHittedState(BaseMonsterController controller) : base(controller) { }
        public override void Excute()  { base.Excute(); }
        public abstract void OnHittedAnimFullyPlayed();
        protected void SetVelocityZero()
        {
            Vector2 velo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(0f, velo.y);
        }

    }

    public abstract class CanKnockback : BaseHittedState
    {
        protected float _knockbackForce;

        public CanKnockback(BaseMonsterController controller) : base(controller) { }
        public override void OnHittedAnimFullyPlayed()
        {
            _entity.ChangeState(EMonsterState.IDLE);
        }

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
        public HittedKnockbackByBlockSuccess(BaseMonsterController controller) : base(controller) 
        {
            _knockbackForce = PlayerController.BLOCK_SUCCESS_KNOCKBACK_X_FORCE;
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS);
        }
    }
    public class HittedKnockbackByBomb : CanKnockback
    {
        public HittedKnockbackByBomb(BaseMonsterController controller) : base(controller) 
        {
            _knockbackForce = PlayerController.KNOCKBACK_BOMB_FORCE;
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB);
        }
    }
    public class HittedParalysis : BaseHittedState
    {
        public HittedParalysis(BaseMonsterController controller) : base(controller) { }
        public override void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.IDLE); }
        public override void Enter()
        {
            PlayAnimation(EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS);
            SetVelocityZero();
        }


    }
    public class Die : BaseMonsterState
    {
        private const float SCALE_TW_DURATION = 0.2f; 
        public Die(BaseMonsterController controller) : base(controller) { }
        public void OnDieAnimFullyPlayed()
        {
            Managers.MonsterPool.Return(_entity.MonsterType, _entity.gameObject);
        }
        public override void Enter() 
        { 
            PlayAnimation(EMonsterState.DIE);
            _entity.RigidBody.Sleep();
            _entity.HealthBar.OnMonsterDie();
        }
        public override void Excute() { }


    }
}


