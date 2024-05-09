using define;
using UnityEngine;


namespace player_states
{

    public abstract class BasePlayerState : State<PlayerController>
    {
        protected float _horizontalMove;
        protected float _groundCheckDistance = 0.2f;
        public static LayerMask sGroundLayerMask = (1 << (int)define.EColliderLayer.GROUND) | (1 << (int)define.EColliderLayer.PLATFORM);
        public BasePlayerState(PlayerController controller) : base(controller) {  }
        public override void Excute() { _horizontalMove = Input.GetAxisRaw("Horizontal"); }
        public virtual void ProcessKeyboardInput() {}

        public void FlipSpriteAccodingPlayerInput()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _entity.NormalAttackPoint.localPosition = _entity.CachedAttackPointLocalLeftPos;
                _entity.ELookDir = ECharacterLookDir.LEFT;
                _entity.SpriteRenderer.flipX = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _entity.NormalAttackPoint.localPosition = _entity.CachedAttackPointLocalRightPos;
                _entity.ELookDir = ECharacterLookDir.RIGHT;
                _entity.SpriteRenderer.flipX = false;
            }
        }
        protected bool IsAnimEnd()
        {
            if (_entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            return false;
        }

        protected bool IsStandGround()
        {
            Bounds bound = _entity.BoxCollider.bounds;
            var hit = Physics2D.BoxCast(bound.center, bound.size, 0f, Vector2.down, _groundCheckDistance, sGroundLayerMask);
            BoxCast2DDebugDraw(bound.center, bound.size, _groundCheckDistance, hit);
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
                    _entity.Animator.Play("Idle");
                    return;
                case EPlayerState.RUN:
                    _entity.Animator.Play("Run");
                    return;
                case EPlayerState.ROLL:
                    _entity.Animator.Play("Roll");
                    return;
                case EPlayerState.JUMP:
                    _entity.Animator.Play("Jump");
                    return;
                case EPlayerState.CLIMB:
                    _entity.Animator.Play("Climb");
                    return;
                case EPlayerState.FALL:
                    _entity.Animator.Play("Fall");
                    return;
                case EPlayerState.LAND:
                    _entity.Animator.Play("Land");
                    return;
                case EPlayerState.NORMAL_ATTACK_1:
                    _entity.Animator.Play("NormalAttack1");
                    return;
                case EPlayerState.NORMAL_ATTACK_2:
                    _entity.Animator.Play("NormalAttack2");
                    return;
                case EPlayerState.NORMAL_ATTACK_3:
                    _entity.Animator.Play("NormalAttack3");
                    return;
                case EPlayerState.HITTED:
                    _entity.Animator.Play("Hitted");
                    return;
                case EPlayerState.BLOCKING:
                    _entity.Animator.Play("Blocking");
                    return;
                case EPlayerState.BLOCK_SUCESS:
                    _entity.Animator.Play("BlockSuccess");
                    return;
                case EPlayerState.DIE:
                    _entity.Animator.Play("Die");
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
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                _entity.ChangeState(EPlayerState.RUN);
            }
            else if (Input.GetKeyDown(PlayerController.KeyRoll))
            {
                _entity.ChangeState(EPlayerState.ROLL);
            }
            else if (Input.GetKeyDown(PlayerController.KeyAttack))
            {
                _entity.ChangeState(EPlayerState.NORMAL_ATTACK_1);
            }
            else if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                _entity.ChangeState(EPlayerState.BLOCKING);
            }
            else if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _entity.ChangeState(EPlayerState.JUMP);
            }
        }

        public override void Enter()       { PlayAnimation(EPlayerState.IDLE); }

        public override void FixedExcute() { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y); }
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
                _entity.ChangeState(EPlayerState.IDLE);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyRoll))
            {
                _entity.ChangeState(EPlayerState.ROLL);
                return;
            }
            else if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                _entity.ChangeState(EPlayerState.BLOCKING);
                return;
            }
            else if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _entity.ChangeState(EPlayerState.JUMP);
                return;
            }
        }

        public override void Enter() { PlayAnimation(EPlayerState.RUN); }

        public override void FixedExcute()
        {
            Vector2 oriVelocity = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
        }

        public override void Excute()
        {
            base.Excute();
            if (!IsStandGround())
                _entity.ChangeState(EPlayerState.FALL);
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
            _entity.JumpParticle.Play();

        }
        public override void FixedExcute()
        {
            if (mIsTwiceJump)
            {
                _entity.Animator.Play("Jump", -1, 0f);
                Vector2 oriVelocity = _entity.RigidBody.velocity;
                _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
                _entity.RigidBody.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
                mIsTwiceJump = false;
            }

            if (!mIsInAir)
            {
                Vector2 oriVelocity = _entity.RigidBody.velocity;
                _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
                _entity.RigidBody.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                mIsInAir = true;
            }
            else
            {
                if (_entity.RigidBody.velocity.y <= 0f)
                    _entity.ChangeState(EPlayerState.FALL);
            }
        }
        public override void Excute()
        {
            base.Excute();
            ProcessKeyboardInput();
            FlipSpriteAccodingPlayerInput();
        }
    }

    public class Climb : BasePlayerState
    {
        public Climb(PlayerController controller) : base(controller) { }

        ECharacterLookDir _eCharacterLookDir;
        float _xOffset = 0.7f;
        float _yOffset = 1.4f;

        public void OnClimbAnimFullyPlayed()
        {
            Vector3 pos = _entity.transform.position;
            if (_eCharacterLookDir == ECharacterLookDir.RIGHT)
                _entity.transform.position = new Vector3(pos.x + _xOffset, pos.y + _yOffset, pos.z);
            else
                _entity.transform.position = new Vector3(pos.x - _xOffset, pos.y + _yOffset, pos.z);
            _entity.ChangeState(EPlayerState.IDLE);
        }
        public override void Enter()
        {
            _entity.SpriteRenderer.material = _entity.PlayerClimbMaterial;
            _eCharacterLookDir = _entity.ELookDir;
            PlayAnimation(EPlayerState.CLIMB);
        }
        public override void FixedExcute()
        {
            _entity.RigidBody.gravityScale = 0f;
            _entity.RigidBody.velocity = Vector2.zero;
        }
        public override void Exit()
        {
            _entity.RigidBody.gravityScale = 1f;
            _entity.SpriteRenderer.material = _entity.PlayerMaterial;
        }
    }


    public class Fall : BasePlayerState
    {
        public Fall(PlayerController controller) : base(controller) { }

        Transform _ledgeCheckPoint;
        LayerMask _platformLayerMask = 1 << (int)define.EColliderLayer.PLATFORM;
        ECharacterLookDir _eCharacterLookDir;
        float _extraHeight = 0.2f;

        public override void Enter()
        {
            PlayAnimation(EPlayerState.FALL);
            if (_ledgeCheckPoint == null)
                _ledgeCheckPoint = _entity.LedgeCheckPoint;
            _eCharacterLookDir = _entity.ELookDir;
        }
        public override void FixedExcute()
        {
            Vector2 oriVelocity = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
        }
        public override void Excute()
        {
            base.Excute();
            Bounds bound = _entity.BoxCollider.bounds;
            var hit = Physics2D.BoxCast(bound.center, bound.size, 0f, Vector2.down, _extraHeight, sGroundLayerMask);
            BoxCast2DDebugDraw(bound.center, bound.size, _extraHeight, hit);
            FlipSpriteAccodingPlayerInput();
            if (hit.collider != null)
            {
                _entity.ChangeState(EPlayerState.LAND);
                return;
            }

            if (IsGrabLedge())
                _entity.ChangeState(EPlayerState.CLIMB);
        }

        public bool IsGrabLedge()
        {
            float dist = 0.75f;
            RaycastHit2D hit;
            if (_eCharacterLookDir == ECharacterLookDir.RIGHT)
            {
                hit = Physics2D.Raycast(_ledgeCheckPoint.position, Vector2.right, dist, _platformLayerMask);
                Debug.DrawRay(_ledgeCheckPoint.position, Vector2.right * dist, UnityEngine.Color.red);
            }
            else
            {
                hit = Physics2D.Raycast(_ledgeCheckPoint.position, Vector2.left, dist, _platformLayerMask);
                Debug.DrawRay(_ledgeCheckPoint.position, Vector2.left * dist, UnityEngine.Color.red);
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
            _entity.FootDustParticle.Play();
        }
        public override void Excute()
        {
            if (IsAnimEnd())
            {
                if (Input.anyKey)
                    _entity.ChangeState(EPlayerState.RUN);
                else
                    _entity.ChangeState(EPlayerState.IDLE);
            }
        }
    }
    public class Roll : BasePlayerState
    {
        public Roll(PlayerController controller) : base(controller) { }

        ECharacterLookDir _eLookDir;
        int _layerMask = (1 << (int)EColliderLayer.MONSTERS) | (1 << (int)EColliderLayer.GROUND) | (1 << (int)EColliderLayer.PLATFORM) | (1 << (int)EColliderLayer.ENV) | (1 << (int)EColliderLayer.EVENT_BOX);
        public void OnRollAnimFullyPlayed(PlayerController _entity) { _entity.ChangeState(EPlayerState.RUN); }
        public override void Enter()
        {
            _eLookDir = _entity.ELookDir;
            PlayAnimation(EPlayerState.ROLL);
            Physics2D.IgnoreLayerCollision((int)EColliderLayer.MONSTERS, (int)EColliderLayer.PLAYER);
        }
        public override void FixedExcute()
        {
            Vector2 oriVelo = _entity.RigidBody.velocity;
            float speed = _entity.Stat.MoveSpeed * 1.5f;
            if (_eLookDir == ECharacterLookDir.RIGHT)
                _entity.RigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime, oriVelo.y);
            else
                _entity.RigidBody.velocity = new Vector2(speed * -Time.fixedDeltaTime, oriVelo.y);
        }

        public override void Exit() { Physics2D.SetLayerCollisionMask((int)EColliderLayer.PLAYER, _layerMask); }
    }
    public abstract class NormalAttackState : BasePlayerState
    {
        public NormalAttackState(PlayerController controller) : base(controller) { }
        protected ECharacterLookDir _eLookDir;
        protected bool _isGoToNextAttack;
        protected Transform _attackPoint;
        protected int _layerMask = 1 << ((int)define.EColliderLayer.MONSTERS);
        public void DamageHittedMonsters()
        {
            Collider2D[] monsters = Physics2D.OverlapCircleAll(_attackPoint.position, 1f, _layerMask);
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
            _attackPoint = _entity.NormalAttackPoint;
            _eLookDir = _entity.ELookDir;
            _isGoToNextAttack = false;
        }

        public override void ProcessKeyboardInput()
        {
            float currAnimTime = _entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currAnimTime >= 0.3f && currAnimTime <= 0.9f)
            {
                if (Input.GetKey(PlayerController.KeyAttack))
                    _isGoToNextAttack = true;
            }
        }
        protected void CheckGoToNextAttack(EPlayerState eNextAttack)
        {
            if (_entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (_isGoToNextAttack)
                    _entity.ChangeState(eNextAttack);
                else
                    _entity.ChangeState(EPlayerState.RUN);
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
        public override void Excute() { CheckGoToNextAttack(EPlayerState.NORMAL_ATTACK_2); }
    }

    public class NormalAttack2 : NormalAttackState
    {
        public NormalAttack2(PlayerController controller) : base(controller) { }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_2);
        }
        public override void Excute() { CheckGoToNextAttack(EPlayerState.NORMAL_ATTACK_3); }
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
                _entity.ChangeState(EPlayerState.RUN);
        }
    }

    public class Blocking : BasePlayerState
    {
        public Blocking(PlayerController controller) : base(controller) { }

        public override void Enter()        { PlayAnimation(EPlayerState.BLOCKING); }

        public override void FixedExcute()  { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y); }

        public override void Excute()
        {
            if (IsAnimEnd())
                _entity.ChangeState(EPlayerState.IDLE);
        }
    }

    public class BlockSuccess : BasePlayerState
    {
        bool _isKnockbackFlag;
        float _knockbackForce = 3f;
        public BlockSuccess(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.BLOCK_SUCESS);
            _entity.StatusText.ShowPopup("Block!");
            _isKnockbackFlag = false;
        }

        public override void FixedExcute()
        {
            if (!_isKnockbackFlag)
            {
                if (_entity.ELookDir == define.ECharacterLookDir.LEFT)
                    _entity.RigidBody.AddForce(Vector2.right * _knockbackForce, ForceMode2D.Impulse);
                else
                    _entity.RigidBody.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse);
                _isKnockbackFlag = true;
            }
        }

        public override void Excute()
        {
            if (IsAnimEnd())
                _entity.ChangeState(EPlayerState.IDLE);
        }
    }

    public class Hitted : BasePlayerState
    {
        public Hitted(PlayerController controller) : base(controller) { }

        public void OnHittedAnimFullyPlayed() { _entity.ChangeState(EPlayerState.RUN); }
        public override void Enter()
        {
            if (!_entity.HitEffectAniamtor.gameObject.activeSelf)
                _entity.HitEffectAniamtor.gameObject.SetActive(true);

            PlayAnimation(EPlayerState.HITTED);
            // TODO : 플레이어 HitEffectAnimation 살릴지 말지 결정해야 함.
            //_entity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_3_KEY, -1, 0f);
        }
        public override void Excute()
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
                _entity.gameObject.SetActive(false);
        }
    }
}