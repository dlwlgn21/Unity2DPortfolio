using define;
using UnityEngine;


namespace player_states
{

    public abstract class BasePlayerState : State<PlayerController>
    {
        protected float mHorizontalMove;
        protected float mGroundCheckDistance = 0.2f;
        public static LayerMask sGroundLayerMask = (1 << (int)define.EColliderLayer.GROUND) | (1 << (int)define.EColliderLayer.PLATFORM);

        public override void Excute(PlayerController entity)
        {
            mHorizontalMove = Input.GetAxisRaw("Horizontal");
        }
        public virtual void ProcessKeyboardInput(PlayerController entity) {}

        public void FlipSpriteAccodingPlayerInput(PlayerController entity)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                entity.NormalAttackPoint.localPosition = entity.CachedAttackPointLocalLeftPos;
                entity.ELookDir = ECharacterLookDir.LEFT;
                entity.SpriteRenderer.flipX = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                entity.NormalAttackPoint.localPosition = entity.CachedAttackPointLocalRightPos;
                entity.ELookDir = ECharacterLookDir.RIGHT;
                entity.SpriteRenderer.flipX = false;
            }
        }
        protected bool IsAnimEnd(PlayerController entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

        protected bool IsStandGround(PlayerController entity)
        {
            Bounds bound = entity.BoxCollider.bounds;
            var hit = Physics2D.BoxCast(bound.center, bound.size, 0f, Vector2.down, mGroundCheckDistance, sGroundLayerMask);
            BoxCast2DDebugDraw(bound.center, bound.size, mGroundCheckDistance, hit);
            if (hit.collider == null)
            {
                return false;
            }
            return true;
        }

        static public void BoxCast2DDebugDraw(Vector2 origin, Vector2 size, float distasnce, RaycastHit2D hit)
        {
            Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
            float w = size.x * 0.5f;
            float h = size.y * 0.5f;
            p1 = new Vector2(-w, h);
            p2 = new Vector2(w, h);
            p3 = new Vector2(w, -h);
            p4 = new Vector2(-w, -h);

            Quaternion q = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
            p1 = q * p1;
            p2 = q * p2;
            p3 = q * p3;
            p4 = q * p4;

            p1 += origin;
            p2 += origin;
            p3 += origin;
            p4 += origin;

            Vector2 realDistance = Vector2.down * distasnce;
            p5 = p1 + realDistance;
            p6 = p2 + realDistance;
            p7 = p3 + realDistance;
            p8 = p4 + realDistance;


            //Drawing the cast
            UnityEngine.Color castColor = hit ? UnityEngine.Color.red : UnityEngine.Color.white;
            Debug.DrawLine(p1, p2, castColor);
            Debug.DrawLine(p2, p3, castColor);
            Debug.DrawLine(p3, p4, castColor);
            Debug.DrawLine(p4, p1, castColor);

            Debug.DrawLine(p5, p6, castColor);
            Debug.DrawLine(p6, p7, castColor);
            Debug.DrawLine(p7, p8, castColor);
            Debug.DrawLine(p8, p5, castColor);

            Debug.DrawLine(p1, p5, castColor);
            Debug.DrawLine(p2, p6, castColor);
            Debug.DrawLine(p3, p7, castColor);
            Debug.DrawLine(p4, p8, castColor);
            if (hit)
            {
                Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, UnityEngine.Color.yellow);
            }
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
            else if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                entity.ChangeState(EPlayerState.JUMP);
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
            base.Excute(entity);
            FlipSpriteAccodingPlayerInput(entity);
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
            else if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                entity.ChangeState(EPlayerState.JUMP);
                return;
            }
        }

        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Run");
        }

        public override void FixedExcute(PlayerController entity)
        {
            Vector2 oriVelocity = entity.RigidBody.velocity;
            entity.RigidBody.velocity = new Vector2(mHorizontalMove * entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
        }

        public override void Excute(PlayerController entity)
        {
            base.Excute(entity);
            if (!IsStandGround(entity))
                entity.ChangeState(EPlayerState.FALL);
            FlipSpriteAccodingPlayerInput(entity);
            ProcessKeyboardInput(entity);
        }

    }

    public class Jump : BasePlayerState
    {

        bool mIsInAir;
        bool mIsTwiceJump;
        bool mIsJumpKeyDownTwice;
        public override void ProcessKeyboardInput(PlayerController entity)
        {
            if (mIsJumpKeyDownTwice)
                return;
            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                mIsTwiceJump = true;
                mIsJumpKeyDownTwice = true;
            }
        }
        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Jump");
            mIsInAir = false;
            mIsTwiceJump = false;
            mIsJumpKeyDownTwice = false;
            entity.JumpParticle.Play();

        }
        public override void FixedExcute(PlayerController entity)
        {
            if (mIsTwiceJump)
            {
                entity.Animator.Play("Jump", -1, 0f);
                entity.RigidBody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
                mIsTwiceJump = false;
            }

            if (!mIsInAir)
            {
                entity.RigidBody.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                mIsInAir = true;
            }
            else
            {
                if (entity.RigidBody.velocity.y <= 0f)
                    entity.ChangeState(EPlayerState.FALL);
            }
        }
        public override void Excute(PlayerController entity)
        {
            base.Excute(entity);
            ProcessKeyboardInput(entity);
        }
    }

    public class Climb : BasePlayerState
    {
        ECharacterLookDir mCharacterLookDir;
        float mXOffset = 0.7f;
        float mYOffset = 1.4f;

        public void OnClimbAnimFullyPlayed(PlayerController entity)
        {
            Vector3 pos = entity.transform.position;
            if (mCharacterLookDir == ECharacterLookDir.RIGHT)
                entity.transform.position = new Vector3(pos.x + mXOffset, pos.y + mYOffset, pos.z);
            else
                entity.transform.position = new Vector3(pos.x - mXOffset, pos.y + mYOffset, pos.z);
            entity.ChangeState(EPlayerState.IDLE);
        }

        public override void Enter(PlayerController entity)
        {
            entity.SpriteRenderer.material = entity.PlayerClimbMaterial;
            mCharacterLookDir = entity.ELookDir;
            entity.Animator.Play("Climb");
        }
        public override void FixedExcute(PlayerController entity)
        {
            entity.RigidBody.gravityScale = 0f;
            entity.RigidBody.velocity = Vector2.zero;
        }
        public override void Exit(PlayerController entity)
        {
            entity.RigidBody.gravityScale = 1f;
            entity.SpriteRenderer.material = entity.PlayerMaterial;
        }
    }


    public class Fall : BasePlayerState
    {
        Transform mLedgeCheckPoint;
        LayerMask mPlatformLayerMask = 1 << (int)define.EColliderLayer.PLATFORM;
        ECharacterLookDir mCharacterLookDir;
        float mExtraHeight = 0.2f;

        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Fall");
            if (mLedgeCheckPoint == null)
                mLedgeCheckPoint = entity.LedgeCheckPoint;
            mCharacterLookDir = entity.ELookDir;
        }



        public override void Excute(PlayerController entity)
        {
            Bounds bound = entity.BoxCollider.bounds;
            var hit = Physics2D.BoxCast(bound.center, bound.size, 0f, Vector2.down, mExtraHeight, sGroundLayerMask);
            BoxCast2DDebugDraw(bound.center, bound.size, mExtraHeight, hit);

            if (hit.collider != null)
            {
                entity.ChangeState(EPlayerState.LAND);
                return;
            }

            if (IsGrabLedge())
                entity.ChangeState(EPlayerState.CLIMB);
        }

        public bool IsGrabLedge()
        {
            float dist = 0.75f;
            RaycastHit2D hit;
            if (mCharacterLookDir == ECharacterLookDir.RIGHT)
            {
                hit = Physics2D.Raycast(mLedgeCheckPoint.position, Vector2.right, dist, mPlatformLayerMask);
                Debug.DrawRay(mLedgeCheckPoint.position, Vector2.right * dist, UnityEngine.Color.red);
            }
            else
            {
                hit = Physics2D.Raycast(mLedgeCheckPoint.position, Vector2.left, dist, mPlatformLayerMask);
                Debug.DrawRay(mLedgeCheckPoint.position, Vector2.left * dist, UnityEngine.Color.red);
            }
            if (hit.collider != null)
                return true;
            return false;
        }
    }

    public class Land : BasePlayerState
    {
        public override void Enter(PlayerController entity)
        {
            entity.Animator.Play("Land");
            entity.FootDustParticle.Play();
        }
        public override void Excute(PlayerController entity)
        {
            if (IsAnimEnd(entity))
            {
                if (Input.anyKey)
                    entity.ChangeState(EPlayerState.RUN);
                else
                    entity.ChangeState(EPlayerState.IDLE);
            }
        }
    }

    public class Roll : BasePlayerState
    {
        ECharacterLookDir meLookDir;
        int mLayerMask = (1 << (int)EColliderLayer.MONSTERS) | (1 << (int)EColliderLayer.GROUND) | (1 << (int)EColliderLayer.PLATFORM);
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