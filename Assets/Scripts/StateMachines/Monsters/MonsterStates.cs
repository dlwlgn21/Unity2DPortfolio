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
        protected float mDistanceFromPlayer;
        protected Vector2 mDirToPlayer;
        protected static string IDLE_ANIM_KEY = "Idle";
        protected static string RUN_ANIM_KEY = "Run";
        protected static string ATTACK_ANIM_KEY = "Attack";
        protected static string HIT_ANIM_KEY = "Hitted";
        protected static string DIE_ANIM_KEY = "Die";
        protected bool IsAnimEnd()
        {
            if (mEntity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

        protected void PlayAnimation(EMonsterState eState)
        {
            switch (eState)
            {
                case EMonsterState.SPAWN:
                    mEntity.Animator.Play(IDLE_ANIM_KEY);
                    return;
                case EMonsterState.TRACE:
                    mEntity.Animator.Play(RUN_ANIM_KEY);
                    return;
                case EMonsterState.ATTACK:
                    mEntity.Animator.Play(ATTACK_ANIM_KEY);
                    return;
                case EMonsterState.HITTED:
                    mEntity.Animator.Play(HIT_ANIM_KEY);
                    return;
                case EMonsterState.DIE:
                    mEntity.Animator.Play(DIE_ANIM_KEY);
                    return;
            }

        }
        protected void ChangeStateIfAnimEnd(EMonsterState eState)
        {
            if (IsAnimEnd())
                mEntity.ChangeState(eState);
        }
        protected void CalculateDistanceFromPlayer()
        {
            mDirToPlayer = mEntity.PlayerTransform.position - mEntity.transform.position;
            mDistanceFromPlayer = mDirToPlayer.magnitude;
        }
    }

    public class Spawn : BaseMonsterState
    {
        public Spawn(BaseMonsterController controller) : base(controller) { }

        public override void Enter() { PlayAnimation(EMonsterState.SPAWN); }
        public override void Excute()
        {
            CalculateDistanceFromPlayer();
            Debug.DrawRay(mEntity.transform.position + Vector3.up * 0.5f, mDirToPlayer.normalized * mEntity.AwarenessRangeToTrace, Color.blue);
            if (mDistanceFromPlayer <= mEntity.AwarenessRangeToTrace)
                mEntity.ChangeState(EMonsterState.TRACE);
        }
    }

    public class Trace : BaseMonsterState
    {
        public Trace(BaseMonsterController controller) : base(controller) { }

        public override void Enter() { PlayAnimation(EMonsterState.TRACE); }
        public override void FixedExcute()
        {
            Vector2 oriVelo = mEntity.RigidBody.velocity;
            mEntity.RigidBody.velocity = new Vector2(mDirToPlayer.normalized.x * mEntity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelo.y);
        }

        public override void Excute()
        {
            CalculateDistanceFromPlayer();
            if (mDistanceFromPlayer <= mEntity.AwarenessRangeToAttack)
                mEntity.ChangeState(EMonsterState.ATTACK);
            Debug.DrawRay(mEntity.transform.position + Vector3.up * 0.5f, mDirToPlayer.normalized * mEntity.AwarenessRangeToTrace, Color.blue);
            Debug.DrawRay(mEntity.transform.position, mDirToPlayer.normalized * mEntity.AwarenessRangeToAttack, Color.red);

        }
    }

    public abstract class BaseAttack : BaseMonsterState
    {
        public BaseAttack(BaseMonsterController controller) : base(controller) { }

        protected int mLayerMask = 1 << (int)define.EColliderLayer.PLAYER;
        public override void Enter()
        {
            PlayAnimation(EMonsterState.ATTACK);
            mEntity.StatusText.ShowPopup("Attack!");
        }
        public override void FixedExcute()  { mEntity.RigidBody.velocity = new Vector2(0f, mEntity.RigidBody.velocity.y);  }
        public override void Excute()       { ChangeStateIfAnimEnd(EMonsterState.TRACE);  }
        protected void CheckOverlapCircle()
        {
            Collider2D collider = Physics2D.OverlapCircle(mEntity.NormalAttackPoint.position, mEntity.NormalAttackRange, mLayerMask);
            if (collider != null)
            {
                PlayerController pc = collider.GetComponent<PlayerController>();
                if (pc != null)
                    pc.OnHitted(mEntity.Stat.Attack, mEntity);
            }
        }
    }

    public abstract class BaseHittedState : BaseMonsterState
    {
        protected bool mIsKnockbackFlag;
        protected float mKnockbackForce;
        public BaseHittedState(BaseMonsterController controller, float knockbackForce) : base(controller) { mKnockbackForce = knockbackForce; }
        
        public override void Enter()
        {
            mIsKnockbackFlag = false;
        }
        public override void FixedExcute()
        {
            if (!mIsKnockbackFlag)
            {
                if (mEntity.ELookDir == define.ECharacterLookDir.LEFT)
                    mEntity.RigidBody.AddForce(Vector2.right * mKnockbackForce, ForceMode2D.Impulse);
                else
                    mEntity.RigidBody.AddForce(Vector2.left * mKnockbackForce, ForceMode2D.Impulse);
                mIsKnockbackFlag = true;
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
            else { mEntity.ChangeState(EMonsterState.TRACE);  }
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EMonsterState.HITTED);
            PlayerController pc = mEntity.PlayerTransform.gameObject.GetComponent<PlayerController>();
            Debug.Assert(pc != null);
            Managers.HitParticle.Play(mEntity.transform.position);
            if (!mEntity.HitEffectAniamtor.gameObject.activeSelf)
                mEntity.HitEffectAniamtor.gameObject.SetActive(true);
            switch (pc.meCurrentState)
            {
                case EPlayerState.NORMAL_ATTACK_1:
                    ProcessHitted(pc, pc.Stat.Attack, BaseCharacterController.HIT_EFFECT_1_KEY, define.EHitCameraShake.WEAK_SHAKE_2D);
                    break;
                case EPlayerState.NORMAL_ATTACK_2:
                    ProcessHitted(pc, pc.Stat.Attack * 2, BaseCharacterController.HIT_EFFECT_2_KEY, define.EHitCameraShake.WEAK_SHAKE_2D);
                    break;
                case EPlayerState.NORMAL_ATTACK_3:
                    ProcessHitted(pc, pc.Stat.Attack * 3, BaseCharacterController.HIT_EFFECT_3_KEY, define.EHitCameraShake.STRONG_SHAKE_2D);
                    break;
                default:
                    break;
            }
            isChangeStateIfDie();
        }

        bool isChangeStateIfDie()
        {
            if (mEntity.Stat.HP <= 0)
            {
                mEntity.ChangeState(EMonsterState.DIE);
                return true;
            }
            return false;
        }

        void ProcessHitted(PlayerController pc, int damage, string hitEffectKey, define.EHitCameraShake eCamShakeType)
        {
            mEntity.Stat.OnHitted(damage);
            mEntity.HealthBar.SetHpRatio((float)mEntity.Stat.HP / mEntity.Stat.MaxHP);
            mEntity.HitEffectAniamtor.Play(hitEffectKey, -1, 0f);
            pc.ShakeCamera(eCamShakeType);
            mEntity.DamageText.ShowPopup(damage);
        }
    }

    public class HittedKnockback : BaseHittedState
    {
        public HittedKnockback(BaseMonsterController controller) : base(controller, knockbackForce: 5f) { }
        public void OnHittedAnimFullyPlayed() { mEntity.ChangeState(EMonsterState.TRACE);  }
        public override void Enter()
        {
            base.Enter();
            mEntity.StatusText.ShowPopup("Knockback");
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
                mEntity.gameObject.SetActive(false);
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


