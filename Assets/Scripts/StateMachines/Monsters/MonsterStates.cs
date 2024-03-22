using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace monster_states
{
    public abstract class BaseMonsterState : State<MonsterController>
    {
        protected float mDistanceFromPlayer;
        protected Vector2 mDirToPlayer;
        protected bool IsAnimEnd(MonsterController entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

        protected void CalculateDistanceFromPlayer(MonsterController entity)
        {
            mDirToPlayer = entity.PlayerTransform.position - entity.transform.position;
            mDistanceFromPlayer = mDirToPlayer.magnitude;
        }
    }

    public class Spawn : BaseMonsterState
    {
        public override void Excute(MonsterController entity)
        {
            CalculateDistanceFromPlayer(entity);
            Debug.DrawRay(entity.transform.position + Vector3.up * 0.5f, mDirToPlayer.normalized * entity.AwarenessRangeToTrace, Color.blue);
            if (mDistanceFromPlayer <= entity.AwarenessRangeToTrace)
                entity.ChangeState(EMonsterState.TRACE);
        }
    }

    public class Trace : BaseMonsterState
    {

        public override void Excute(MonsterController entity)
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

    public class Attack : BaseMonsterState
    {
        public override void Excute(MonsterController entity)
        {
            if (IsAnimEnd(entity))
                entity.ChangeState(EMonsterState.TRACE);
        }
    }

    public class Hitted : BaseMonsterState
    {
        public override void Excute(MonsterController entity)
        {

        }
    }

    public class Die : BaseMonsterState
    {
        public override void Excute(MonsterController entity)
        {

        }
    }
}


