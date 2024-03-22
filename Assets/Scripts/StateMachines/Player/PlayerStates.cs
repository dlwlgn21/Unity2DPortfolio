using define;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.EventSystems.EventTrigger;

namespace player_states
{

    public abstract class BasePlayerState : State<PlayerController>
    {
        public virtual void ProcessKeyboardInput(PlayerController entity) {}
    }

    public class Idle : BasePlayerState
    {
        public override void ProcessKeyboardInput(PlayerController entity)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                entity.ChangeState(EPlayerState.RUN);
            }
            else if (Input.GetKeyDown(PlayerController.KeyRoll))
            {
                entity.ChangeState(EPlayerState.ROLL);
            }
            else if (Input.GetKeyDown(PlayerController.KeyAttack))
            {
                entity.ChangeState(EPlayerState.NORMAL_ATTACK_1);
            }
        }

        public override void Enter(PlayerController entity)
        {
        }
        public override void Excute(PlayerController entity)
        {
            ProcessKeyboardInput(entity);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }
    public class Run : BasePlayerState
    {
        public override void ProcessKeyboardInput(PlayerController entity)
        {
            if (!Input.anyKey)
            {
                entity.ChangeState(EPlayerState.IDLE);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyRoll))
            {
                entity.ChangeState(EPlayerState.ROLL);
                return;
            }

            Vector2 pos = entity.transform.position;
            if (Input.GetKey(PlayerController.KeyRight))
                pos.x += entity.Stat.MoveSpeed * Time.deltaTime;
            if (Input.GetKey(PlayerController.KeyLeft))
                pos.x += entity.Stat.MoveSpeed * -Time.deltaTime;
            entity.transform.position = pos;
        }

        public override void Enter(PlayerController entity)
        {
        }
        public override void Excute(PlayerController entity)
        {
            ProcessKeyboardInput(entity);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }
    public class Roll : BasePlayerState
    {
        float mRollMovingDist = 5.0f;
        ECharacterLookDir meLookDir;
        public override void Enter(PlayerController entity)
        {
            meLookDir = entity.ELookDir;
        }
        public override void Excute(PlayerController entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                entity.ChangeState(EPlayerState.RUN);

            Vector2 pos = entity.transform.position;
            if (meLookDir == define.ECharacterLookDir.RIGHT)
                pos.x += mRollMovingDist * Time.deltaTime;
            else
                pos.x += mRollMovingDist * -Time.deltaTime;
            entity.transform.position = pos;
        }
        public override void Exit(PlayerController entity)
        {
        }
    }
    public abstract class NormalAttackState : BasePlayerState
    {
        protected ECharacterLookDir meLookDir;
        protected bool mIsGoToNextAttack;
        protected Transform mAttackPoint;
        protected int mLayerMask = 1 << ((int)define.EColliderLayer.MONSTERS);
        public void IsHitMonsters(EPlayerNoramlAttackType eAttackType)
        {
            switch (eAttackType)
            {
                case EPlayerNoramlAttackType.ATTACK_1:
                    Debug.Log("Attack 1 Called!");
                    Physics2D.OverlapCircleAll(mAttackPoint.position, 1f, mLayerMask);
                    break;
                case EPlayerNoramlAttackType.ATTACK_2:
                    Debug.Log("Attack 2 Called!");
                    Physics2D.OverlapCircleAll(mAttackPoint.position, 1f, mLayerMask);
                    break;
                case EPlayerNoramlAttackType.ATTACK_3:
                    Debug.Log("Attack 3 Called!");
                    Physics2D.OverlapCircleAll(mAttackPoint.position, 1f, mLayerMask);
                    break;
            }
        }

        public override void Enter(PlayerController entity)
        {
            mAttackPoint = entity.NormalAttackPoint;
            meLookDir = entity.ELookDir;
            mIsGoToNextAttack = false;
        }

        public override void ProcessKeyboardInput(PlayerController entity)
        {
            float currAnimTime = entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currAnimTime >= 0.3f && currAnimTime <= 0.9f)
            {
                if (Input.GetKey(PlayerController.KeyAttack))
                    mIsGoToNextAttack = true;
            }
        }
        protected void CheckGoToNextAttack(PlayerController entity, EPlayerState eNextAttack)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (mIsGoToNextAttack)
                    entity.ChangeState(eNextAttack);
                else
                    entity.ChangeState(EPlayerState.RUN);
                return;
            }
            ProcessKeyboardInput(entity);
        }
        protected bool IsAnimEnd(PlayerController entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

    }

    public class NormalAttack1 : NormalAttackState
    {
        public override void Excute(PlayerController entity)
        {
            CheckGoToNextAttack(entity, EPlayerState.NORMAL_ATTACK_2);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }

    public class NormalAttack2 : NormalAttackState
    {
        public override void Excute(PlayerController entity)
        {
            CheckGoToNextAttack(entity, EPlayerState.NORMAL_ATTACK_3);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }

    public class NormalAttack3 : NormalAttackState
    {

        public override void Excute(PlayerController entity)
        {
            if (IsAnimEnd(entity))
                entity.ChangeState(EPlayerState.RUN);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }
}