using define;
using UnityEngine;

namespace player_states
{

    public abstract class BasePlayerState : State<PlayerController>
    {
        public virtual void ProcessKeyboardInput(PlayerController entity) {}
        protected bool IsAnimEnd(PlayerController entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }
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
            else if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                entity.ChangeState(EPlayerState.BLOCKING);
            }
        }

        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Idle");
        }

        public override void FixedExcute(PlayerController entity)
        {
            entity.RigidBody.velocity = new Vector2(0f, entity.RigidBody.velocity.y);
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
        float mHorizontalMove;
        public override void ProcessKeyboardInput(PlayerController entity)
        {
            mHorizontalMove = Input.GetAxisRaw("Horizontal");
            // ChangeState
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
            else if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                entity.ChangeState(EPlayerState.BLOCKING);
                return;
            }


            //// Moving
            //Vector2 pos = entity.transform.position;
            //if (Input.GetKey(PlayerController.KeyRight))
            //{
            //    entity.SpriteRenderer.flipX = false;

            //    pos.x += entity.Stat.MoveSpeed * Time.deltaTime;
            //}
            //if (Input.GetKey(PlayerController.KeyLeft))
            //{
            //    entity.SpriteRenderer.flipX = true;
            //    pos.x += entity.Stat.MoveSpeed * -Time.deltaTime;
            //}
            //entity.transform.position = pos;
        }

        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Run");
        }

        public override void FixedExcute(PlayerController entity)
        {
            Vector2 oriVelocity = entity.RigidBody.velocity;
            entity.RigidBody.velocity = new Vector2(mHorizontalMove * entity.Stat.MoveSpeed * Time.deltaTime, oriVelocity.y);
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
        ECharacterLookDir meLookDir;
        int mLayerMask = (1 << (int)EColliderLayer.MONSTERS) | (1 << (int)EColliderLayer.CAVE_TILES);
        public void OnRollAnimFullyPlayed(PlayerController entity) { entity.ChangeState(EPlayerState.RUN); }
        public override void Enter(PlayerController entity)
        {
            meLookDir = entity.ELookDir;
            entity.Animator.Play("Roll");
            Physics2D.IgnoreLayerCollision((int)EColliderLayer.MONSTERS, (int)EColliderLayer.PLAYER);
        }
        public override void FixedExcute(PlayerController entity)
        {
            Vector2 oriVelo = entity.RigidBody.velocity;
            float speed = entity.Stat.MoveSpeed * 1.5f;
            if (meLookDir == ECharacterLookDir.RIGHT)
                entity.RigidBody.velocity = new Vector2(speed * Time.deltaTime, oriVelo.y);
            else
                entity.RigidBody.velocity = new Vector2(speed * -Time.deltaTime, oriVelo.y);
        }

        public override void Excute(PlayerController entity)
        {
            //Vector2 pos = entity.transform.position;
            //if (meLookDir == define.ECharacterLookDir.RIGHT)
            //    pos.x += mRollMovingDist * Time.deltaTime;
            //else
            //    pos.x += mRollMovingDist * -Time.deltaTime;
            //entity.transform.position = pos;
        }
        public override void Exit(PlayerController entity)
        {
            Physics2D.SetLayerCollisionMask((int)EColliderLayer.PLAYER, mLayerMask);
        }
    }
    public abstract class NormalAttackState : BasePlayerState
    {
        protected ECharacterLookDir meLookDir;
        protected bool mIsGoToNextAttack;
        protected Transform mAttackPoint;
        protected int mLayerMask = 1 << ((int)define.EColliderLayer.MONSTERS);
        public void DamageHittedMonsters()
        {
            Collider2D[] monsters = Physics2D.OverlapCircleAll(mAttackPoint.position, 1f, mLayerMask);
            if (monsters == null)
                return;

            foreach (Collider2D mon in monsters)
            {
                BaseMonsterController controller = mon.gameObject.GetComponent<BaseMonsterController>();
                Debug.Assert(controller != null);
                controller.HittedByPlayer();
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

    }

    public class NormalAttack1 : NormalAttackState
    {
        public override void Enter(PlayerController entity)
        {
            base.Enter(entity);
            entity.Animator.Play("NormalAttack1");
        }
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
        public override void Enter(PlayerController entity)
        {
            base.Enter(entity);
            entity.Animator.Play("NormalAttack2");
        }
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
        public override void Enter(PlayerController entity)
        {
            base.Enter(entity);
            entity.Animator.Play("NormalAttack3");
        }
        public override void Excute(PlayerController entity)
        {
            if (IsAnimEnd(entity))
                entity.ChangeState(EPlayerState.RUN);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }

    public class Blocking : BasePlayerState
    {
        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Blocking");
        }

        public override void FixedExcute(PlayerController entity)
        {
            entity.RigidBody.velocity = new Vector2(0f, entity.RigidBody.velocity.y);
        }

        public override void Excute(PlayerController entity)
        {
            if (IsAnimEnd(entity))
                entity.ChangeState(EPlayerState.IDLE);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }

    public class BlockSuccess : BasePlayerState
    {
        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("BlockSuccess");
            entity.ShowStatusPopup("Block!");
        }
        public override void Excute(PlayerController entity)
        {
            if (IsAnimEnd(entity))
                entity.ChangeState(EPlayerState.IDLE);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }

    public class Hitted : BasePlayerState
    {
        public void OnHittedAnimFullyPlayed(PlayerController entitiy) { entitiy.ChangeState(EPlayerState.RUN); }
        public override void Enter(PlayerController entity)
        {
            if (!entity.HitEffectAniamtor.gameObject.activeSelf)
                entity.HitEffectAniamtor.gameObject.SetActive(true);
            entity.Animator.Play("Hitted");
            // TODO : 플레이어 HitEffectAnimation 살릴지 말지 결정해야 함.
            //entity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_3_KEY, -1, 0f);
        }
        public override void Excute(PlayerController entity)
        {

        }
        public override void Exit(PlayerController entity)
        {
        }
    }

    public class Die : BasePlayerState
    {
        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Die");
        }
        public override void Excute(PlayerController entity)
        {
            if (IsAnimEnd(entity))
                entity.gameObject.SetActive(false);
        }
        public override void Exit(PlayerController entity)
        {
        }
    }
}