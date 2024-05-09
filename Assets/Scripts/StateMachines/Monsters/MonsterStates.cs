using CameraShake;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEditor.Experimental.Licensing;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace monster_states
{
    public abstract class BaseMonsterState : State<BaseMonsterController>
    {
        public BaseMonsterState(BaseMonsterController controller) : base(controller) {}
        protected float _distanceFromPlayer;
        protected Vector2 _dirToPlayer;
        protected static string IDLE_ANIM_KEY = "Idle";
        protected static string RUN_ANIM_KEY = "Run";
        protected static string ATTACK_ANIM_KEY = "Attack";
        protected static string HIT_ANIM_KEY = "Hitted";
        protected static string DIE_ANIM_KEY = "Die";
        protected bool IsAnimEnd()
        {
            if (_entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

        protected void PlayAnimation(EMonsterState eState)
        {
            switch (eState)
            {
                case EMonsterState.SPAWN:
                    _entity.Animator.Play(IDLE_ANIM_KEY);
                    return;
                case EMonsterState.TRACE:
                    _entity.Animator.Play(RUN_ANIM_KEY);
                    return;
                case EMonsterState.ATTACK:
                    _entity.Animator.Play(ATTACK_ANIM_KEY);
                    return;
                case EMonsterState.HITTED:
                    _entity.Animator.Play(HIT_ANIM_KEY);
                    return;
                case EMonsterState.DIE:
                    _entity.Animator.Play(DIE_ANIM_KEY);
                    return;
            }

        }
        protected void ChangeStateIfAnimEnd(EMonsterState eState)
        {
            if (IsAnimEnd())
                _entity.ChangeState(eState);
        }
        protected void CalculateDistanceFromPlayer()
        {
            _dirToPlayer = _entity.PlayerTransform.position - _entity.transform.position;
            _distanceFromPlayer = _dirToPlayer.magnitude;
        }
    }

    public class Spawn : BaseMonsterState
    {
        public Spawn(BaseMonsterController controller) : base(controller) { }

        public override void Enter() { PlayAnimation(EMonsterState.SPAWN); }
        public override void Excute()
        {
            CalculateDistanceFromPlayer();
            Debug.DrawRay(_entity.transform.position + Vector3.up * 0.5f, _dirToPlayer.normalized * _entity.AwarenessRangeToTrace, Color.blue);
            if (_distanceFromPlayer <= _entity.AwarenessRangeToTrace)
                _entity.ChangeState(EMonsterState.TRACE);
        }
    }

    public class Trace : BaseMonsterState
    {
        public Trace(BaseMonsterController controller) : base(controller) { }

        public override void Enter() { PlayAnimation(EMonsterState.TRACE); }
        public override void FixedExcute()
        {
            Vector2 oriVelo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(_dirToPlayer.normalized.x * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelo.y);
        }

        public override void Excute()
        {
            CalculateDistanceFromPlayer();
            if (_distanceFromPlayer <= _entity.AwarenessRangeToAttack)
                _entity.ChangeState(EMonsterState.ATTACK);
            Debug.DrawRay(_entity.transform.position + Vector3.up * 0.5f, _dirToPlayer.normalized * _entity.AwarenessRangeToTrace, Color.blue);
            Debug.DrawRay(_entity.transform.position, _dirToPlayer.normalized * _entity.AwarenessRangeToAttack, Color.red);

        }
    }

    public abstract class BaseAttack : BaseMonsterState
    {
        public BaseAttack(BaseMonsterController controller) : base(controller) { }

        protected int mLayerMask = 1 << (int)define.EColliderLayer.PLAYER;
        public override void Enter()
        {
            PlayAnimation(EMonsterState.ATTACK);
            _entity.StatusText.ShowPopup("Attack!");
        }
        public override void FixedExcute()  { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y);  }
        public override void Excute()       { ChangeStateIfAnimEnd(EMonsterState.TRACE);  }
        protected void CheckOverlapCircle()
        {
            Collider2D collider = Physics2D.OverlapCircle(_entity.NormalAttackPoint.position, _entity.NormalAttackRange, mLayerMask);
            if (collider != null)
            {
                PlayerController pc = collider.GetComponent<PlayerController>();
                if (pc != null)
                    pc.OnHitted(_entity.Stat.Attack, _entity);
            }
        }
    }

    public abstract class BaseHittedState : BaseMonsterState
    {
        protected bool _isKnockbackFlag;
        protected float _knockbackForce;
        public BaseHittedState(BaseMonsterController controller, float knockbackForce) : base(controller) { _knockbackForce = knockbackForce; }
        
        public override void Enter()
        {
            _isKnockbackFlag = false;
        }
        public override void FixedExcute()
        {
            CalculateDistanceFromPlayer();
            if (!_isKnockbackFlag)
            {
                if (_dirToPlayer.x < 0f)
                    _entity.RigidBody.AddForce(Vector2.right * _knockbackForce, ForceMode2D.Impulse);
                else
                    _entity.RigidBody.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse);
                _isKnockbackFlag = true;
            }
        }
        public override void Excute() { ChangeStateIfAnimEnd(EMonsterState.SPAWN); }
    }


    public class Hitted : BaseHittedState
    {
        public Hitted(BaseMonsterController controller) : base(controller, knockbackForce:4f) { }

        public void OnHittedAnimFullyPlayed()
        {
            if (isChangeStateIfDie()) {}
            else { _entity.ChangeState(EMonsterState.TRACE);  }
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.HITTED);
            PlayerController pc = _entity.PlayerTransform.gameObject.GetComponent<PlayerController>();
            Debug.Assert(pc != null);
            Managers.HitParticle.Play(_entity.transform.position);
            if (!_entity.HitEffectAniamtor.gameObject.activeSelf)
                _entity.HitEffectAniamtor.gameObject.SetActive(true);
            switch (pc.ECurrentState)
            {
                case EPlayerState.NORMAL_ATTACK_1:
                    ProcessHitted(pc, pc.Stat.Attack, BaseCharacterController.HIT_EFFECT_1_KEY, define.EHitCameraShake.WEAK_SHAKE_2D);
                    break;
                case EPlayerState.NORMAL_ATTACK_2:
                    ProcessHitted(pc, (int)(pc.Stat.Attack * 1.5f), BaseCharacterController.HIT_EFFECT_2_KEY, define.EHitCameraShake.WEAK_SHAKE_2D);
                    break;
                case EPlayerState.NORMAL_ATTACK_3:
                    ProcessHitted(pc, pc.Stat.Attack * 2, BaseCharacterController.HIT_EFFECT_3_KEY, define.EHitCameraShake.STRONG_SHAKE_2D);
                    break;
                default:
                    break;
            }
            isChangeStateIfDie();
        }

        bool isChangeStateIfDie()
        {
            if (_entity.Stat.HP <= 0)
            {
                _entity.ChangeState(EMonsterState.DIE);
                return true;
            }
            return false;
        }

        void ProcessHitted(PlayerController pc, int damage, string hitEffectKey, define.EHitCameraShake eCamShakeType)
        {
            if (pc.ELookDir == _entity.ELookDir)
            {
                int backAttackDamage = damage * 3;
                _entity.Stat.OnHitted(backAttackDamage);
                _entity.DamageText.ShowPopup(backAttackDamage);
                pc.StatusText.ShowPopup("Back Attack!");
            }
            else
            {
                _entity.Stat.OnHitted(damage);
                _entity.DamageText.ShowPopup(damage);
            }
            _entity.HealthBar.SetHpRatio((float)_entity.Stat.HP / _entity.Stat.MaxHP);
            _entity.HitEffectAniamtor.Play(hitEffectKey, -1, 0f);
            pc.ShakeCamera(eCamShakeType);
        }
    }

    public class HittedKnockback : BaseHittedState
    {
        public HittedKnockback(BaseMonsterController controller) : base(controller, knockbackForce: 5f) { }
        public void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.TRACE);  }
        public override void Enter()
        {
            base.Enter();
            _entity.StatusText.ShowPopup("Knockback");
        }
    }
    public class Die : BaseMonsterState
    {
        // TODO : 몬스터 Die 구현해야함.
        public Die(BaseMonsterController controller) : base(controller) { }

        public override void Enter()
        {
            PlayAnimation(EMonsterState.DIE);
        }
        public override void Excute()
        {
            if (IsAnimEnd())
                Managers.MonsterPool.Return(_entity.MonsterType, _entity.gameObject);
        }
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


