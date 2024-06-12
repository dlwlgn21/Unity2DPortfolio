using define;
using DG.Tweening;
using UnityEngine;


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

        public virtual void OnHittedByPlayerNormalAttack(PlayerController pc, EPlayerNoramlAttackType eType)
        {
            // TODO : HitEffect도 따로 빼서 스스로 관리할 수 있게 해야함.
            if (!_entity.HitEffectAniamtor.gameObject.activeSelf)
            {
                _entity.HitEffectAniamtor.gameObject.SetActive(true);
            }
            _entity.StatusText.ShowPopup(BaseMonsterController.HITTED_STATUS_TEXT_STRING);
            Managers.CamShake.CamShake(ECamShakeType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK);
            switch (eType)
            {
                case EPlayerNoramlAttackType.ATTACK_1:
                    {
                        ProcessHitted(pc, pc.Stat.Attack, EPlayerNoramlAttackType.ATTACK_1);
                        break;
                    }
                case EPlayerNoramlAttackType.ATTACK_2:
                    {
                        ProcessHitted(pc, (int)(pc.Stat.Attack * 1.5f), EPlayerNoramlAttackType.ATTACK_2);
                        break;
                    }

                case EPlayerNoramlAttackType.ATTACK_3:
                    {
                        ProcessHitted(pc, pc.Stat.Attack * 2, EPlayerNoramlAttackType.ATTACK_3);
                        StartBigAttackEffect();
                        break;
                    }
                default:
                    break;
            }
            if (_entity.Stat.HP <= 0)
            {
                _entity.ChangeState(EMonsterState.DIE);
            }
        }
        private void ProcessHitted(PlayerController pc, int damage, EPlayerNoramlAttackType eType)
        {
            int beforeDamgeHp = 0;
            int afterDamageHp = 0;
            if (pc.ELookDir == _entity.ELookDir && eType == EPlayerNoramlAttackType.ATTACK_1)
            {
                // TODO : 이 하드코딩된 매직넘버, 즉 백어택 데미지 계수 수정하자.
                int backAttackDamage = damage * 3;
                _entity.Stat.OnHitted(backAttackDamage, out beforeDamgeHp, out afterDamageHp);
                pc.StatusText.ShowPopup("백어택!");
                _entity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_3_KEY, -1, 0f);
                StartBigAttackEffect();
                _entity.BloodEffectController.PlayBloodAnimation(
                    EPlayerNoramlAttackType.BACK_ATTACK, 
                    _entity.transform.position,
                    (_entity.ELookDir == ECharacterLookDir.LEFT) ? ECharacterLookDir.RIGHT : ECharacterLookDir.LEFT
                );
            }
            else
            {
                _entity.Stat.OnHitted(damage, out beforeDamgeHp, out afterDamageHp);
                _entity.BloodEffectController.PlayBloodAnimation(eType, _entity.transform.position, _entity.ELookDir);
            }
            _entity.DamageText.ShowPopup(beforeDamgeHp - afterDamageHp);
            _entity.HealthBar.DecraseHP(beforeDamgeHp, afterDamageHp);
            _entity.DamageFlasher.StartDamageFlash();
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
                case EMonsterState.HITTED_KNOCKBACK:
                case EMonsterState.HITTED_KNOCKBACK_BOMB:
                    _entity.Animator.Play(HIT_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.HITTED_PARALYSIS:
                    _entity.Animator.Play(HIT_PARALYSIS_KEY, -1, 0f);
                    return;
                case EMonsterState.DIE:
                    _entity.Animator.Play(DIE_ANIM_KEY, -1, 0f);
                    return;
            }
        }
        protected void ChangeStateIfAnimEnd(EMonsterState eState)
        {
            if (IsAnimEnd())
            {
                _entity.ChangeState(eState);
            }
        }
        protected void CalculateDistanceFromPlayer()
        {
            _dirToPlayer = _entity.PlayerTransform.position - _entity.transform.position;
            _distanceFromPlayer = _dirToPlayer.magnitude;
        }


        protected void AdjustKnockbackAcoddingLookDir(EPlayerNoramlAttackType eType)
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
        private void StartBigAttackEffect()
        {
            Managers.CamManager.StartCamZoom();
            Managers.TimeManager.OnPlayerNormalAttackSuccess();
            Managers.HitParticle.PlayBigHittedParticle(_entity.transform.position);
        }
    }

    public class Idle : BaseMonsterState
    {
        public Idle(BaseMonsterController controller) : base(controller) { }
        public override void OnHittedByPlayerNormalAttack(PlayerController pc, EPlayerNoramlAttackType eType)
        {
            base.OnHittedByPlayerNormalAttack(pc, eType);
            AdjustKnockbackAcoddingLookDir(eType);
        }
        public override void Enter() { PlayAnimation(EMonsterState.IDLE); }
        public override void Excute()
        {
            // Monster가 추락중일 때에는 상태변환 하지 않음.
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
        public override void OnHittedByPlayerNormalAttack(PlayerController pc, EPlayerNoramlAttackType eType)
        {
            base.OnHittedByPlayerNormalAttack(pc, eType);
            _isHiitedByPlayerNormalAttack = true;
            AdjustKnockbackAcoddingLookDir(eType);
            DecreaseAnimSpeed();
            _animReturnOriginalSpeedTimer = ANIM_SLOW_TIME;
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
                // PlayerNormalAttack에 맞고 AddForce가 호출 되었다는 뜻.
                // 그럴때는 일단 AddForce의 힘이 적용 된 다음에 이동하도록 했음.
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

    public class BaseAttack : CanSlowState
    {
        public BaseAttack(BaseMonsterController controller) : base(controller) { }

        protected int _playerLayerMask = 1 << (int)define.EColliderLayer.PLAYER;

        public void OnAttackAnimFullyPlayed() { _entity.ChangeState(EMonsterState.IDLE); }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.ATTACK);
            _entity.StatusText.ShowPopup("공격!");
            _entity.AttackLightController.TurnOnLight();

        }
        public override void Excute()
        {
            base.Excute();
        }
        protected void CheckOverlapCircle()
        {
            Collider2D collider = Physics2D.OverlapCircle(_entity.NormalAttackPoint.position, _entity.NormalAttackRange, _playerLayerMask);
            if (collider != null)
            {
                PlayerController pc = collider.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.OnHitted(_entity.Stat.Attack, _entity);
                }
            }
        }
        public override void Exit()
        {
            base.Exit();
            _entity.AttackLightController.TurnOffLightGradually();
        }
    }


    public abstract class BaseHittedState : BaseMonsterState
    {
        public BaseHittedState(BaseMonsterController controller) : base(controller) { }
        
        public override void Enter()
        {
            Vector2 velo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(0f, velo.y);
        }

        public override void Excute()  { base.Excute(); }
        public abstract void OnHittedAnimFullyPlayed();

    }

    public class HittedKnockbackByBlockSuccess : BaseHittedState
    {
        protected float _knockbackForce = PlayerController.BLOCK_SUCCESS_KNOCKBACK_X_FORCE;
        private bool _isAddForceThisFrame = false;
        public HittedKnockbackByBlockSuccess(BaseMonsterController controller) : base(controller) { }
        public override void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.IDLE);  }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.HITTED_KNOCKBACK);
            _entity.StatusText.ShowPopup("넉백!");
            _isAddForceThisFrame = false;
        }

        public override void FixedExcute()
        {
            if (!_isAddForceThisFrame)
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
                _isAddForceThisFrame = true;
            }
        }


    }

    public class HittedKnockbackByBomb : HittedKnockbackByBlockSuccess
    {
        public HittedKnockbackByBomb(BaseMonsterController controller) : base(controller) 
        {
            _knockbackForce = PlayerController.KNOCKBACK_BOMB_FORCE;
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.HITTED_KNOCKBACK_BOMB);
            Managers.CamShake.CamShake(ECamShakeType.MONSTER_HITTED_BY_KNOCKBACK_BOMB);
        }
        public override void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.IDLE); }
    }
    public class HittedParalysis : BaseHittedState
    {
        public HittedParalysis(BaseMonsterController controller) : base(controller) { }
        public override void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.IDLE); }
        public override void Enter()
        {
            PlayAnimation(EMonsterState.HITTED_PARALYSIS);
            Vector2 velo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(0f, velo.y);
            _entity.StatusText.ShowPopup("마비!");
            Managers.CamShake.CamShake(ECamShakeType.MONSTER_HITTED_BY_REAPER_ATTACK);
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
            _entity.HealthBar.transform.DOScale(0f, SCALE_TW_DURATION).SetEase(Ease.OutElastic);
        }
        public override void Excute() { }


    }


    #region MONSTER_ATTACK_STATES
    public class CagedShockerAttack : BaseAttack
    {
        public CagedShockerAttack(BaseMonsterController controller) : base(controller) { }

        public void OnNoramlAttack1ValidSlashed() { CheckOverlapCircle(); }
        public void OnNoramlAttack2ValidSlashed() { CheckOverlapCircle(); }
    }

    public class BlasterAttack : BaseAttack
    {
        public BlasterAttack(BaseMonsterController controller) : base(controller) { }

        public void OnBlaterValidAttack() { CheckOverlapCircle(); }
    }

    public class WardenAttack : BaseAttack
    {
        public WardenAttack(BaseMonsterController controller) : base(controller) { }

        public void OnWardenValidAttack() { CheckOverlapCircle(); }
    }

    public class HSlicerAttack : BaseAttack
    {
        public HSlicerAttack(BaseMonsterController controller) : base(controller) { }

        public void OnHSlicerValidAttack1() { CheckOverlapCircle(); }
        public void OnHSlicerValidAttack2() { CheckOverlapCircle(); }

    }

    #endregion
}


