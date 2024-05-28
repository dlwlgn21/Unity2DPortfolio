using CameraShake;
using define;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
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
        protected static string HIT_PARALYSIS_KEY = "HittedParalysis";
        protected static string DIE_ANIM_KEY = "Die";
        protected bool IsAnimEnd()
        {
            if (_entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                return true;
            }
            return false;
        }

        public override void Excute()
        {
            _entity.SetLookDir();
        }

        protected void PlayAnimation(EMonsterState eState)
        {
            switch (eState)
            {
                case EMonsterState.SPAWN:
                    _entity.Animator.Play(IDLE_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.TRACE:
                    _entity.Animator.Play(RUN_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.ATTACK:
                    _entity.Animator.Play(ATTACK_ANIM_KEY, -1, 0f);
                    return;
                case EMonsterState.HITTED:
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
    }

    public class Spawn : BaseMonsterState
    {
        public Spawn(BaseMonsterController controller) : base(controller) { }

        public override void Enter() { PlayAnimation(EMonsterState.SPAWN); }
        public override void Excute()
        {
            base.Excute();
            CalculateDistanceFromPlayer();
            Debug.DrawRay(_entity.transform.position + Vector3.up * 0.5f, _dirToPlayer.normalized * _entity.AwarenessRangeToTrace, Color.blue);
            if (_distanceFromPlayer <= _entity.AwarenessRangeToTrace)
            {
                _entity.ChangeState(EMonsterState.TRACE);
            }
        }
    }

    public class Trace : BaseMonsterState
    {
        public Trace(BaseMonsterController controller) : base(controller) { }

        public override void Enter()  { PlayAnimation(EMonsterState.TRACE);  }
        public override void FixedExcute()
        {
            Vector2 oriVelo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(_dirToPlayer.normalized.x * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelo.y);
        }

        public override void Excute()
        {
            base.Excute();
            CalculateDistanceFromPlayer();
            if (_distanceFromPlayer <= _entity.AwarenessRangeToAttack)
            {
                _entity.ChangeState(EMonsterState.ATTACK);
                return;
            }
            Debug.DrawRay(_entity.transform.position + Vector3.up * 0.5f, _dirToPlayer.normalized * _entity.AwarenessRangeToTrace, Color.blue);
            Debug.DrawRay(_entity.transform.position, _dirToPlayer.normalized * _entity.AwarenessRangeToAttack, Color.red);
        }
    }

    public abstract class BaseAttack : BaseMonsterState
    {
        public BaseAttack(BaseMonsterController controller) : base(controller) { }

        protected int _playerLayerMask = 1 << (int)define.EColliderLayer.PLAYER;
        public override void Enter()
        {
            PlayAnimation(EMonsterState.ATTACK);
            _entity.StatusText.ShowPopup("공격!");
            _entity.AttackLight.SetActive(true);
            _entity.RotateAttackLightAccodingCharacterLookDir();
            _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y);
        }
        //public override void FixedExcute()  { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y);  }
        public override void Excute() 
        {
            base.Excute();
            ChangeStateIfAnimEnd(EMonsterState.SPAWN);  
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
            _entity.AttackLight.SetActive(false);
        }
    }

    public abstract class BaseHittedState : BaseMonsterState
    {
        public BaseHittedState(BaseMonsterController controller) : base(controller) { }
        
        public override void Enter()
        {
            PlayAnimation(EMonsterState.HITTED);
            Vector2 velo = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(0f, velo.y);
        }

        public override void Excute()  { base.Excute(); }
        public abstract void OnHittedAnimFullyPlayed();

    }


    public class Hitted : BaseHittedState
    {
        private PlayerController _pc = null;
        public Hitted(BaseMonsterController controller) : base(controller) { }

        public override void OnHittedAnimFullyPlayed()
        {
            if (isChangeStateIfDie()) {}
            else { _entity.ChangeState(EMonsterState.SPAWN);  }
        }
        public override void Enter()
        {
            base.Enter();
            _entity.StatusText.ShowPopup("경직!");

            if (_pc == null)
            {
                _pc = _entity.PlayerTransform.gameObject.GetComponent<PlayerController>();
            }
            Debug.Assert(_pc != null);

            Managers.HitParticle.Play(_entity.transform.position);
            if (!_entity.HitEffectAniamtor.gameObject.activeSelf)
                _entity.HitEffectAniamtor.gameObject.SetActive(true);

            Managers.CamShake.CamShake(ECamShakeType.MONSTER_HITTED_BY_PLAYER_NORMAL_ATTACK);

            switch (_pc.ECurrentState)
            {
                case EPlayerState.NORMAL_ATTACK_1:
                    ProcessHitted(_pc, _pc.Stat.Attack);
                    _entity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_1_KEY, -1, 0f);

                    break;
                case EPlayerState.NORMAL_ATTACK_2:
                    ProcessHitted(_pc, (int)(_pc.Stat.Attack * 1.5f));
                    _entity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_2_KEY, -1, 0f);
                    break;
                case EPlayerState.NORMAL_ATTACK_3:
                    ProcessHitted(_pc, _pc.Stat.Attack * 2);
                    _entity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_3_KEY, -1, 0f);
                    break;
                default:
                    break;
            }
            isChangeStateIfDie();
        }

        private bool isChangeStateIfDie()
        {
            if (_entity.Stat.HP <= 0)
            {
                _entity.ChangeState(EMonsterState.DIE);
                return true;
            }
            return false;
        }

        private void ProcessHitted(PlayerController pc, int damage)
        {
            int beforeDamgeHp = 0;
            int afterDamageHp = 0;
            if (pc.ELookDir == _entity.ELookDir)
            {
                // TODO : 이 하드코딩된 매직넘버, 즉 백어택 데미지 계수 수정하자.
                int backAttackDamage = damage * 3;
                _entity.Stat.OnHitted(backAttackDamage, out beforeDamgeHp, out afterDamageHp);
                pc.StatusText.ShowPopup("백어택!");
            }
            else
            {
                _entity.Stat.OnHitted(damage, out beforeDamgeHp, out afterDamageHp);
            }
            _entity.DamageText.ShowPopup(beforeDamgeHp - afterDamageHp);
            _entity.HealthBar.DecraseHP(beforeDamgeHp, afterDamageHp);
        }
    }

    public class HittedKnockback : BaseHittedState
    {
        protected float _knockbackForce = 5f;
        private bool _isAddForceThisFrame = false;

        public HittedKnockback(BaseMonsterController controller) : base(controller) { }
        public override void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.SPAWN);  }
        public override void Enter()
        {
            base.Enter();
            _entity.StatusText.ShowPopup("넉백!");
            _isAddForceThisFrame = false;
        }

        public override void FixedExcute()
        {
            CalculateDistanceFromPlayer();
            if (!_isAddForceThisFrame)
            {
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

    public class HittedKnockbackBomb : HittedKnockback
    {
        public HittedKnockbackBomb(BaseMonsterController controller) : base(controller) 
        {
            _knockbackForce = 12f;
        }
        public override void Enter()
        {
            base.Enter();
            Managers.CamShake.CamShake(ECamShakeType.MONSTER_HITTED_BY_KNOCKBACK_BOMB);
        }
        public override void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.SPAWN); }
    }
    public class HittedParalysis : BaseHittedState
    {
        public HittedParalysis(BaseMonsterController controller) : base(controller) { }
        public override void OnHittedAnimFullyPlayed() { _entity.ChangeState(EMonsterState.SPAWN); }
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
        private const float SCALE_TW_DURATION = 0.5f; 
        public Die(BaseMonsterController controller) : base(controller) { }

        public override void Enter() 
        { 
            PlayAnimation(EMonsterState.DIE);
            _entity.HealthBar.transform.DOScale(0f, SCALE_TW_DURATION).SetEase(Ease.OutElastic);
        }
        public override void Excute()
        {
            base.Excute();
            if (IsAnimEnd())
            {
                Managers.MonsterPool.Return(_entity.MonsterType, _entity.gameObject);
            }
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


