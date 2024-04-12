using define;
using UnityEngine;


namespace player_states
{

    public abstract class BasePlayerState : State<PlayerController>
    {
        protected float mHorizontalMove;
        protected float mGroundCheckDistance = 0.2f;
        public static LayerMask sGroundLayerMask = (1 << (int)define.EColliderLayer.GROUND) | (1 << (int)define.EColliderLayer.PLATFORM);
        public BasePlayerState(PlayerController controller) : base(controller) {  }
        public override void Excute()
        {
            mHorizontalMove = Input.GetAxisRaw("Horizontal");
        }
        public virtual void ProcessKeyboardInput() {}

        public void FlipSpriteAccodingPlayerInput()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                mEntity.NormalAttackPoint.localPosition = mEntity.CachedAttackPointLocalLeftPos;
                mEntity.ELookDir = ECharacterLookDir.LEFT;
                mEntity.SpriteRenderer.flipX = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                mEntity.NormalAttackPoint.localPosition = mEntity.CachedAttackPointLocalRightPos;
                mEntity.ELookDir = ECharacterLookDir.RIGHT;
                mEntity.SpriteRenderer.flipX = false;
            }
        }
        protected bool IsAnimEnd()
        {
            if (mEntity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

        protected bool IsStandGround()
        {
            Bounds bound = mEntity.BoxCollider.bounds;
            var hit = Physics2D.BoxCast(bound.center, bound.size, 0f, Vector2.down, mGroundCheckDistance, sGroundLayerMask);
            BoxCast2DDebugDraw(bound.center, bound.size, mGroundCheckDistance, hit);
            if (hit.collider == null)
            {
                return false;
            }
            return true;
        }
        protected void PlayAnimation(EPlayerState eState)
        {
            switch (eState) 
            {
                case EPlayerState.IDLE:
                    mEntity.Animator.Play("Idle");
                    return;
                case EPlayerState.RUN:
                    mEntity.Animator.Play("Run");
                    return;
                case EPlayerState.ROLL:
                    mEntity.Animator.Play("Roll");
                    return;
                case EPlayerState.JUMP:
                    mEntity.Animator.Play("Jump");
                    return;
                case EPlayerState.CLIMB:
                    mEntity.Animator.Play("Climb");
                    return;
                case EPlayerState.FALL:
                    mEntity.Animator.Play("Fall");
                    return;
                case EPlayerState.LAND:
                    mEntity.Animator.Play("Land");
                    return;
                case EPlayerState.NORMAL_ATTACK_1:
                    mEntity.Animator.Play("NormalAttack1");
                    return;
                case EPlayerState.NORMAL_ATTACK_2:
                    mEntity.Animator.Play("NormalAttack2");
                    return;
                case EPlayerState.NORMAL_ATTACK_3:
                    mEntity.Animator.Play("NormalAttack3");
                    return;
                case EPlayerState.HITTED:
                    mEntity.Animator.Play("Hitted");
                    return;
                case EPlayerState.BLOCKING:
                    mEntity.Animator.Play("Blocking");
                    return;
                case EPlayerState.BLOCK_SUCESS:
                    mEntity.Animator.Play("BlockSuccess");
                    return;
                case EPlayerState.DIE:
                    mEntity.Animator.Play("Die");
                    return;
            }
            Debug.Assert(false);
            return;
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
        public Idle(PlayerController controller) : base(controller) { }
        public override void ProcessKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                mEntity.ChangeState(EPlayerState.RUN);
            }
            else if (Input.GetKeyDown(PlayerController.KeyRoll))
            {
                mEntity.ChangeState(EPlayerState.ROLL);
            }
            else if (Input.GetKeyDown(PlayerController.KeyAttack))
            {
                mEntity.ChangeState(EPlayerState.NORMAL_ATTACK_1);
            }
            else if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                mEntity.ChangeState(EPlayerState.BLOCKING);
            }
            else if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                mEntity.ChangeState(EPlayerState.JUMP);
            }
        }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.IDLE);
        }

        public override void FixedExcute()
        {
            mEntity.RigidBody.velocity = new Vector2(0f, mEntity.RigidBody.velocity.y);
        }
        public override void Excute()
        {
            base.Excute();
            FlipSpriteAccodingPlayerInput();
            ProcessKeyboardInput();
        }
    }
    public class Run : BasePlayerState
    {
        public Run(PlayerController controller) : base(controller) { }

        public override void ProcessKeyboardInput()
        {
            // ChangeState
            if (!Input.anyKey)
            {
                mEntity.ChangeState(EPlayerState.IDLE);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyRoll))
            {
                mEntity.ChangeState(EPlayerState.ROLL);
                return;
            }
            else if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                mEntity.ChangeState(EPlayerState.BLOCKING);
                return;
            }
            else if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                mEntity.ChangeState(EPlayerState.JUMP);
                return;
            }
        }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.RUN);
        }

        public override void FixedExcute()
        {
            Vector2 oriVelocity = mEntity.RigidBody.velocity;
            mEntity.RigidBody.velocity = new Vector2(mHorizontalMove * mEntity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
        }

        public override void Excute()
        {
            base.Excute();
            if (!IsStandGround())
                mEntity.ChangeState(EPlayerState.FALL);
            FlipSpriteAccodingPlayerInput();
            ProcessKeyboardInput();
        }

    }

    public class Jump : BasePlayerState
    {
        public Jump(PlayerController controller) : base(controller) { }

        bool mIsInAir;
        bool mIsTwiceJump;
        bool mIsJumpKeyDownTwice;
        public override void ProcessKeyboardInput()
        {
            if (mIsJumpKeyDownTwice)
                return;
            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                mIsTwiceJump = true;
                mIsJumpKeyDownTwice = true;
            }
        }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.JUMP);
            mIsInAir = false;
            mIsTwiceJump = false;
            mIsJumpKeyDownTwice = false;
            mEntity.JumpParticle.Play();

        }
        public override void FixedExcute()
        {
            if (mIsTwiceJump)
            {
                mEntity.Animator.Play("Jump", -1, 0f);
                mEntity.RigidBody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
                mIsTwiceJump = false;
            }

            if (!mIsInAir)
            {
                mEntity.RigidBody.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                mIsInAir = true;
            }
            else
            {
                if (mEntity.RigidBody.velocity.y <= 0f)
                    mEntity.ChangeState(EPlayerState.FALL);
            }
        }
        public override void Excute()
        {
            base.Excute();
            ProcessKeyboardInput();
        }
    }

    public class Climb : BasePlayerState
    {
        public Climb(PlayerController controller) : base(controller) { }

        ECharacterLookDir mCharacterLookDir;
        float mXOffset = 0.7f;
        float mYOffset = 1.4f;

        public void OnClimbAnimFullyPlayed()
        {
            Vector3 pos = mEntity.transform.position;
            if (mCharacterLookDir == ECharacterLookDir.RIGHT)
                mEntity.transform.position = new Vector3(pos.x + mXOffset, pos.y + mYOffset, pos.z);
            else
                mEntity.transform.position = new Vector3(pos.x - mXOffset, pos.y + mYOffset, pos.z);
            mEntity.ChangeState(EPlayerState.IDLE);
        }

        public override void Enter()
        {
            mEntity.SpriteRenderer.material = mEntity.PlayerClimbMaterial;
            mCharacterLookDir = mEntity.ELookDir;
            PlayAnimation(EPlayerState.CLIMB);
        }
        public override void FixedExcute()
        {
            mEntity.RigidBody.gravityScale = 0f;
            mEntity.RigidBody.velocity = Vector2.zero;
        }
        public override void Exit()
        {
            mEntity.RigidBody.gravityScale = 1f;
            mEntity.SpriteRenderer.material = mEntity.PlayerMaterial;
        }
    }


    public class Fall : BasePlayerState
    {
        public Fall(PlayerController controller) : base(controller) { }

        Transform mLedgeCheckPoint;
        LayerMask mPlatformLayerMask = 1 << (int)define.EColliderLayer.PLATFORM;
        ECharacterLookDir mCharacterLookDir;
        float mExtraHeight = 0.2f;

        public override void Enter()
        {
            PlayAnimation(EPlayerState.FALL);
            if (mLedgeCheckPoint == null)
                mLedgeCheckPoint = mEntity.LedgeCheckPoint;
            mCharacterLookDir = mEntity.ELookDir;
        }



        public override void Excute()
        {
            Bounds bound = mEntity.BoxCollider.bounds;
            var hit = Physics2D.BoxCast(bound.center, bound.size, 0f, Vector2.down, mExtraHeight, sGroundLayerMask);
            BoxCast2DDebugDraw(bound.center, bound.size, mExtraHeight, hit);

            if (hit.collider != null)
            {
                mEntity.ChangeState(EPlayerState.LAND);
                return;
            }

            if (IsGrabLedge())
                mEntity.ChangeState(EPlayerState.CLIMB);
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
        public Land(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.LAND);
            mEntity.FootDustParticle.Play();
        }
        public override void Excute()
        {
            if (IsAnimEnd())
            {
                if (Input.anyKey)
                    mEntity.ChangeState(EPlayerState.RUN);
                else
                    mEntity.ChangeState(EPlayerState.IDLE);
            }
        }
    }

    public class Roll : BasePlayerState
    {
        public Roll(PlayerController controller) : base(controller) { }

        ECharacterLookDir meLookDir;
        int mLayerMask = (1 << (int)EColliderLayer.MONSTERS) | (1 << (int)EColliderLayer.GROUND) | (1 << (int)EColliderLayer.PLATFORM);
        public void OnRollAnimFullyPlayed(PlayerController mEntity) { mEntity.ChangeState(EPlayerState.RUN); }
        public override void Enter()
        {

            meLookDir = mEntity.ELookDir;
            PlayAnimation(EPlayerState.ROLL);
            Physics2D.IgnoreLayerCollision((int)EColliderLayer.MONSTERS, (int)EColliderLayer.PLAYER);
        }
        public override void FixedExcute()
        {
            Vector2 oriVelo = mEntity.RigidBody.velocity;
            float speed = mEntity.Stat.MoveSpeed * 1.5f;
            if (meLookDir == ECharacterLookDir.RIGHT)
                mEntity.RigidBody.velocity = new Vector2(speed * Time.deltaTime, oriVelo.y);
            else
                mEntity.RigidBody.velocity = new Vector2(speed * -Time.deltaTime, oriVelo.y);
        }

        public override void Exit()
        {
            Physics2D.SetLayerCollisionMask((int)EColliderLayer.PLAYER, mLayerMask);
        }
    }
    public abstract class NormalAttackState : BasePlayerState
    {
        public NormalAttackState(PlayerController controller) : base(controller) { }
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

        public override void Enter()
        {
            mAttackPoint = mEntity.NormalAttackPoint;
            meLookDir = mEntity.ELookDir;
            mIsGoToNextAttack = false;
        }

        public override void ProcessKeyboardInput()
        {
            float currAnimTime = mEntity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currAnimTime >= 0.3f && currAnimTime <= 0.9f)
            {
                if (Input.GetKey(PlayerController.KeyAttack))
                    mIsGoToNextAttack = true;
            }
        }
        protected void CheckGoToNextAttack(EPlayerState eNextAttack)
        {
            if (mEntity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (mIsGoToNextAttack)
                    mEntity.ChangeState(eNextAttack);
                else
                    mEntity.ChangeState(EPlayerState.RUN);
                return;
            }
            ProcessKeyboardInput();
        }

    }

    public class NormalAttack1 : NormalAttackState
    {
        public NormalAttack1(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_1);
        }
        public override void Excute()
        {
            CheckGoToNextAttack(EPlayerState.NORMAL_ATTACK_2);
        }
    }

    public class NormalAttack2 : NormalAttackState
    {
        public NormalAttack2(PlayerController controller) : base(controller) { }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_2);
        }
        public override void Excute()
        {
            CheckGoToNextAttack(EPlayerState.NORMAL_ATTACK_3);
        }
    }

    public class NormalAttack3 : NormalAttackState
    {
        public NormalAttack3(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_3);
        }
        public override void Excute()
        {
            if (IsAnimEnd())
                mEntity.ChangeState(EPlayerState.RUN);
        }
    }

    public class Blocking : BasePlayerState
    {
        public Blocking(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.BLOCKING);
        }

        public override void FixedExcute()
        {
            mEntity.RigidBody.velocity = new Vector2(0f, mEntity.RigidBody.velocity.y);
        }

        public override void Excute()
        {
            if (IsAnimEnd())
                mEntity.ChangeState(EPlayerState.IDLE);
        }
    }

    public class BlockSuccess : BasePlayerState
    {
        public BlockSuccess(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.BLOCK_SUCESS);
            mEntity.ShowStatusPopup("Block!");
        }
        public override void Excute()
        {
            if (IsAnimEnd())
                mEntity.ChangeState(EPlayerState.IDLE);
        }
    }

    public class Hitted : BasePlayerState
    {
        public Hitted(PlayerController controller) : base(controller) { }

        public void OnHittedAnimFullyPlayed() { mEntity.ChangeState(EPlayerState.RUN); }
        public override void Enter()
        {
            if (!mEntity.HitEffectAniamtor.gameObject.activeSelf)
                mEntity.HitEffectAniamtor.gameObject.SetActive(true);
            PlayAnimation(EPlayerState.HITTED);
            // TODO : 플레이어 HitEffectAnimation 살릴지 말지 결정해야 함.
            //mEntity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_3_KEY, -1, 0f);
        }
        public override void Excute()
        {

        }
        public override void Exit()
        {
        }
    }

    public class Die : BasePlayerState
    {
        public Die(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.DIE);
        }
        public override void Excute()
        {
            if (IsAnimEnd())
                mEntity.gameObject.SetActive(false);
        }
        public override void Exit()
        {
        }
    }
}