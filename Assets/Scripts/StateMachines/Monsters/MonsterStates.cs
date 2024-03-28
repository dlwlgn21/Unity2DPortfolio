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
        protected float mDistanceFromPlayer;
        protected Vector2 mDirToPlayer;
        protected static string IDLE_ANIM_KEY = "Idle";
        protected static string RUN_ANIM_KEY = "Run";
        protected static string ATTACK_ANIM_KEY = "Attack";
        protected static string HIT_ANIM_KEY = "Hitted";
        protected static string DIE_ANIM_KEY = "Die";
        protected bool IsAnimEnd(BaseMonsterController entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

        protected void ChangeStateIfAnimEnd(BaseMonsterController entity, EMonsterState eState)
        {
            if (IsAnimEnd(entity))
                entity.ChangeState(eState);
        }
        protected void CalculateDistanceFromPlayer(BaseMonsterController entity)
        {
            mDirToPlayer = entity.PlayerTransform.position - entity.transform.position;
            mDistanceFromPlayer = mDirToPlayer.magnitude;
        }
    }

    public class Spawn : BaseMonsterState
    {
        public override void Enter(BaseMonsterController entity)
        {
            entity.Animator.Play(IDLE_ANIM_KEY);
        }
        public override void Excute(BaseMonsterController entity)
        {
            CalculateDistanceFromPlayer(entity);
            Debug.DrawRay(entity.transform.position + Vector3.up * 0.5f, mDirToPlayer.normalized * entity.AwarenessRangeToTrace, Color.blue);
            if (mDistanceFromPlayer <= entity.AwarenessRangeToTrace)
                entity.ChangeState(EMonsterState.TRACE);
        }
    }

    public class Trace : BaseMonsterState
    {
        public override void Enter(BaseMonsterController entity)
        {
            entity.Animator.Play(RUN_ANIM_KEY);
        }
        public override void Excute(BaseMonsterController entity)
        {
            CalculateDistanceFromPlayer(entity);
            if (mDistanceFromPlayer <= entity.AwarenessRangeToAttack)
                entity.ChangeState(EMonsterState.ATTACK);
            Debug.DrawRay(entity.transform.position + Vector3.up * 0.5f, mDirToPlayer.normalized * entity.AwarenessRangeToTrace, Color.blue);
            Debug.DrawRay(entity.transform.position, mDirToPlayer.normalized * entity.AwarenessRangeToAttack, Color.red);
            Vector2 pos = entity.transform.position;
            pos += mDirToPlayer.normalized * entity.Stat.MoveSpeed * Time.deltaTime;
            entity.transform.position = pos;
        }

    }

    public abstract class BaseAttack : BaseMonsterState
    {
        protected BaseMonsterController mEntity;
        protected int mLayerMask = 1 << (int)define.EColliderLayer.PLAYER;
        public override void Enter(BaseMonsterController entity)
        {
            mEntity = entity;
            entity.Animator.Play(ATTACK_ANIM_KEY);
        }
        public override void Excute(BaseMonsterController entity)
        {
            ChangeStateIfAnimEnd(entity, EMonsterState.TRACE);
        }
        protected void CheckOverlapCircle()
        {
            Collider2D collider = Physics2D.OverlapCircle(mEntity.NormalAttackPoint.position, mEntity.NormalAttackRange, mLayerMask);
            if (collider != null)
            {
                PlayerController pc = collider.GetComponent<PlayerController>();
                if (pc != null)
                    pc.OnHitted(mEntity.Stat.Attack);
            }
        }
    }



    public class Hitted : BaseMonsterState
    {
        BaseMonsterController mEntity;

        public void OnHittedAnimFullyPlayed()
        {
            if (isChangeStateIfDie(mEntity)) {}
            else { mEntity.ChangeState(EMonsterState.TRACE);  }
        }
        public override void Enter(BaseMonsterController entity)
        {
            mEntity = entity;
            entity.Animator.Play(HIT_ANIM_KEY);
            PlayerController pc = entity.PlayerTransform.gameObject.GetComponent<PlayerController>();
            Debug.Assert(pc != null);
            switch (pc.meCurrentState)
            {
                case EPlayerState.NORMAL_ATTACK_1:
                    entity.Stat.OnHitted(pc.Stat.Attack);
                    break;
                case EPlayerState.NORMAL_ATTACK_2:
                    entity.Stat.OnHitted(pc.Stat.Attack * 2);
                    break;
                case EPlayerState.NORMAL_ATTACK_3:
                    entity.Stat.OnHitted(pc.Stat.Attack * 3);
                    break;
                default:
                    break;
            }

            isChangeStateIfDie(entity);
        }
        public override void Excute(BaseMonsterController entity)
        {
            ChangeStateIfAnimEnd(entity, EMonsterState.TRACE);
        }
        bool isChangeStateIfDie(BaseMonsterController entity)
        {
            if (entity.Stat.HP <= 0)
            {
                entity.ChangeState(EMonsterState.DIE);
                return true;
            }
            return false;
        }
    }

    public class Die : BaseMonsterState
    {
        BaseMonsterController mEntity;

        public void OnDieAnimFullyPlayed()
        {
            mEntity.gameObject.SetActive(false);

        }
        public override void Enter(BaseMonsterController entity)
        {
            mEntity = entity;
            entity.Animator.Play(DIE_ANIM_KEY);
        }
        public override void Excute(BaseMonsterController entity)
        {
        }
    }


    #region MONSTER_ATTACK_STATES
    public class CagedShockerAttack : BaseAttack
    {
        public void OnNoramlAttack1ValidSlashed() { CheckOverlapCircle(); }
        public void OnNoramlAttack2ValidSlashed() { CheckOverlapCircle(); }
    }

    public class BlasterAttack : BaseAttack
    {
        public void OnBlaterValidAttack() { CheckOverlapCircle(); }
    }

    public class WardenAttack : BaseAttack
    {
        public void OnWardenValidAttack() { CheckOverlapCircle(); }
    }

    public class HSlicerAttack : BaseAttack
    {
        public void OnHSlicerValidAttack1() { CheckOverlapCircle(); }
        public void OnHSlicerValidAttack2() { CheckOverlapCircle(); }

    }

    #endregion
}


