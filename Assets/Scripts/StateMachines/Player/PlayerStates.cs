using define;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace player_states
{

    public abstract class BasePlayerState : State<PlayerController>
    {
        protected float _horizontalMove;
        const int GROUND_LAYER_MASK = (1 << (int)define.EColliderLayer.Platform) | (1 << (int)define.EColliderLayer.LedgeClimb);

        #region GroundCheck
        Bounds _bound;
        readonly float GROUND_CHECK_LAY_DIST;
        readonly Vector3 RIGHT_GROUND_CHECK_DIFF = new(0.15f, 0f, 0f);
        readonly Vector3 LEFT_GROUND_CHECK_DIFF = new(-0.15f, 0f, 0f);
        #endregion
        public BasePlayerState(PlayerController controller) : base(controller) 
        {
            GROUND_CHECK_LAY_DIST = _entity.CapsuleCollider.bounds.extents.y + 0.1f; 
        }
        #region Public
        public abstract void OnAnimFullyPlayed();
        public override void Excute() { }
        #endregion

        #region Protected
        protected virtual void ProcessKeyboardInput() { }
        protected void SetHorizontalMove()  { _horizontalMove = Input.GetAxisRaw("Horizontal"); }
        protected void RotateTransformAccodingHorizontalMove()
        {
            //동시 키입력시에 Rotate하지 않음!
            if (_horizontalMove == 0)
                return;
            if (_horizontalMove < 0 && _entity.ELookDir == ECharacterLookDir.Right)
            {
                _entity.ELookDir = ECharacterLookDir.Left;
                _entity.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                Managers.Cam.CamFollow.CallTurn();
            }
            else if (_horizontalMove > 0 && _entity.ELookDir == ECharacterLookDir.Left)
            {
                _entity.ELookDir = ECharacterLookDir.Right;
                _entity.transform.localRotation = Quaternion.Euler(Vector3.zero);
                Managers.Cam.CamFollow.CallTurn();
            }
        }
        protected void SetVelocityToZero() { _entity.RigidBody.velocity = Vector2.zero; }
        protected bool IsStandGround()
        {
            _bound = _entity.CapsuleCollider.bounds;
            var rightHit = Physics2D.Raycast(_bound.center + RIGHT_GROUND_CHECK_DIFF, Vector2.down, GROUND_CHECK_LAY_DIST, GROUND_LAYER_MASK);
            var leftHit = Physics2D.Raycast(_bound.center + LEFT_GROUND_CHECK_DIFF, Vector2.down, GROUND_CHECK_LAY_DIST, GROUND_LAYER_MASK);
            if (leftHit.collider == null && rightHit.collider == null)
            {
                DrawGroundCheckDebugRay(Color.green);
                return false;
            }
            DrawGroundCheckDebugRay(Color.red);
            return true;
        }
        protected void PlayAnimation(EPlayerState eState)
        {
            switch (eState) 
            {
                case EPlayerState.Idle:
                    _entity.Animator.Play("Idle", -1, 0f);
                    return;
                case EPlayerState.Run:
                    _entity.Animator.Play("Run", -1, 0f);
                    return;
                case EPlayerState.Roll:
                    _entity.Animator.Play("Roll", -1, 0f);
                    return;
                case EPlayerState.Jump:
                    _entity.Animator.Play("Jump", -1, 0f);
                    return;
                case EPlayerState.Climb:
                    _entity.Animator.Play("Climb", -1, 0f);
                    return;
                case EPlayerState.Fall:
                    _entity.Animator.Play("Fall", -1, 0f);
                    return;
                case EPlayerState.Land:
                    _entity.Animator.Play("Land", -1, 0f);
                    return;
                case EPlayerState.NormalAttack_1:
                    _entity.Animator.Play("NormalAttack1", -1, 0f);
                    return;
                case EPlayerState.NormalAttack_2:
                    _entity.Animator.Play("NormalAttack2", -1, 0f);
                    return;
                case EPlayerState.NormalAttack_3:
                    _entity.Animator.Play("NormalAttack3", -1, 0f);
                    return;
                case EPlayerState.SkillCast:
                    _entity.Animator.Play("SkillCast", -1, 0f);
                    return;
                case EPlayerState.SkillSpawn:
                    _entity.Animator.Play("SkillSpawn", -1, 0f);
                    return;
                case EPlayerState.HitByMelleAttack:
                    _entity.Animator.Play("Hitted", -1, 0f);
                    return;
                case EPlayerState.HitByStatusParallysis:
                    _entity.Animator.Play("HittedStatusParallysis", -1, 0f);
                    return;
                case EPlayerState.HitByProjectileKnockback:
                    _entity.Animator.Play("HittedProjectileKnockback", -1, 0f);
                    return;
                case EPlayerState.Block:
                    _entity.Animator.Play("Blocking", -1, 0f);
                    return;
                case EPlayerState.BlockSucces:
                    _entity.Animator.Play("BlockSuccess", -1, 0f);
                    return;
                case EPlayerState.Die:
                    _entity.Animator.Play("Die", -1, 0f);
                    return;
            }
            Debug.Assert(false);
            return;
        }
        #endregion

        void DrawGroundCheckDebugRay(Color color)
        {
            Debug.DrawRay(_bound.center + RIGHT_GROUND_CHECK_DIFF, Vector2.down * GROUND_CHECK_LAY_DIST, color);
            Debug.DrawRay(_bound.center + LEFT_GROUND_CHECK_DIFF, Vector2.down * GROUND_CHECK_LAY_DIST, color);
        }
    }

    public sealed class Idle : BasePlayerState
    {
        public Idle(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() { }
        protected override void ProcessKeyboardInput()
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                _entity.ChangeState(EPlayerState.Run);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyAttack))
            {
                _entity.ChangeState(EPlayerState.NormalAttack_1);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                _entity.ChangeState(EPlayerState.Block);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _entity.ChangeState(EPlayerState.Jump);
                return;
            }
        }
        public override void Enter()       
        {
            _entity.HeadLight.SetActive(true);
            PlayAnimation(EPlayerState.Idle); 
        }

        public override void FixedExcute() 
        { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y); }
        public override void Excute()
        {
            SetHorizontalMove();
            if (!IsStandGround())
            {
                _entity.ChangeState(EPlayerState.Fall);
                return;
            }
            RotateTransformAccodingHorizontalMove();
            ProcessKeyboardInput();
        }


    }
    public sealed class Run : BasePlayerState
    {
        public Run(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() { }

        protected override void ProcessKeyboardInput()
        {
            // ChangeState
            if (!Input.anyKey)
            {
                _entity.ChangeState(EPlayerState.Idle);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                _entity.ChangeState(EPlayerState.Block);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _entity.ChangeState(EPlayerState.Jump);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyAttack))
            {
                _entity.ChangeState(EPlayerState.NormalAttack_1);
                return;
            }
        }

        public override void Enter() 
        {
            _entity.HeadLight.SetActive(true);
            PlayAnimation(EPlayerState.Run); 
        }

        public override void FixedExcute()
        {
            Vector2 oriVelocity = _entity.RigidBody.velocity;
            if (_entity.IsSlowState)
            {
                _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime * 0.5f, oriVelocity.y);
            }
            else
            {
                _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
            }
        }

        public override void Excute()
        {
            SetHorizontalMove();
            if (!IsStandGround())
            {
                // 5.13에 return 추가함.
                _entity.ChangeState(EPlayerState.Fall);
                return;
            }
            RotateTransformAccodingHorizontalMove();
            ProcessKeyboardInput();
        }

    }

    public abstract class InAir : BasePlayerState
    {
        protected static readonly float FIRST_JUMP_FORCE = 9f;
        protected static readonly float SECOND_JUMP_FORCE = 5f;
        protected static readonly float FALL_TO_TWICE_JUMP_FORCE = 12f;
        public static UnityAction<Vector2> JumpEventHandler;
        public InAir(PlayerController controller) : base(controller)  {  }
        public override void OnAnimFullyPlayed() { }

        protected void ChangeVelocityAcordingHorizontalInput()
        {
            Vector2 oriVelocity = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
        }
        protected void DoJump(float upForce)
        {
            ChangeVelocityAcordingHorizontalInput();
            _entity.RigidBody.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
            JumpEventHandler?.Invoke(_entity.transform.position);
        }
    }


    public sealed class Jump : InAir
    {
        // TODO : Static UnityAction으로 바꿔야 함.
        public Action TwiceJumpEventHandler;

        private bool _isInAir;
        private bool _isTwiceJump;
        private bool _isJumpKeyDownTwice;

        public Jump(PlayerController controller) : base(controller) { }

        ~Jump() { TwiceJumpEventHandler = null; }

        protected override void ProcessKeyboardInput()
        {
            if (_isJumpKeyDownTwice)
                return;
            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _isTwiceJump = true;
                _isJumpKeyDownTwice = true;
            }
        }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.Jump);
            _isInAir = false;
            _isTwiceJump = false;
            _isJumpKeyDownTwice = false;
        }
        public override void FixedExcute()
        {
            ProcessHorizontalInputAndFlip();
            if (_isTwiceJump)
            {
                _entity.Animator.Play("Jump", -1, 0f);
                DoJump(SECOND_JUMP_FORCE);
                _isTwiceJump = false;
                // 6.10일 FallState에서도 이단점프 가능하게 하기 위해서 추가한 부분.
                if (TwiceJumpEventHandler != null)
                {
                    TwiceJumpEventHandler.Invoke();
                }
            }
            if (!_isInAir)
            {
                DoJump(FIRST_JUMP_FORCE);
                _isInAir = true;
            }
            else
            {
                ChangeVelocityAcordingHorizontalInput();
                if (_entity.RigidBody.velocity.y <= 0f)
                {
                    _entity.ChangeState(EPlayerState.Fall);
                }
            }
        }
        public override void Excute()
        {
            SetHorizontalMove();
            ProcessHorizontalInputAndFlip();
        }

        private void ProcessHorizontalInputAndFlip()
        {
            ProcessKeyboardInput();
            RotateTransformAccodingHorizontalMove();
        }
    }

    public abstract class BaseFall : InAir
    {
        protected Transform _ledgeHeadRayPoint;
        protected Transform _ledgeBodyRayPoint;
        protected LayerMask _ledgeLayerMask = 1 << (int)define.EColliderLayer.LedgeClimb;
        protected ECharacterLookDir _eCharacterLookDir;
        protected float _extraHeight = 0.2f;
        protected const float RAY_DISTANCE = 0.4f;
        protected BaseFall(PlayerController controller) : base(controller) { }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.Fall);
            if (_ledgeHeadRayPoint == null)
            {
                _ledgeHeadRayPoint = _entity.LedgeHeadRayPoint;
                _ledgeBodyRayPoint = _entity.LedgeBodyRayPoint;
            }
            _eCharacterLookDir = _entity.ELookDir;
        }
        public override void FixedExcute()
        {
            ChangeVelocityAcordingHorizontalInput();
        }
        public override void Excute()
        {
            if (IsStandGround())
            {
                _entity.ChangeState(EPlayerState.Land);
                return;
            }

            if (IsGrabLedge())
            {
                _entity.ChangeState(EPlayerState.Climb);
            }
            SetHorizontalMove();
            RotateTransformAccodingHorizontalMove();
        }

        protected bool IsGrabLedge()
        {
            RaycastHit2D headRayHit = new();
            RaycastHit2D bodyRayHit = new();
            if (_eCharacterLookDir == ECharacterLookDir.Right)
            {
                RaycastToLedgeLayer(ref headRayHit, ref bodyRayHit, Vector2.right);
            }
            else
            {
                RaycastToLedgeLayer(ref headRayHit, ref bodyRayHit, Vector2.left);
            }
            if (headRayHit.collider != null && bodyRayHit.collider != null)
            {
                return true;
            }
            return false;
        }

        void RaycastToLedgeLayer(ref RaycastHit2D headHit, ref RaycastHit2D bodyHit, Vector2 dir)
        {
            headHit = Physics2D.Raycast(_ledgeHeadRayPoint.position, dir, RAY_DISTANCE, _ledgeLayerMask);
            bodyHit = Physics2D.Raycast(_ledgeBodyRayPoint.position, dir, RAY_DISTANCE, _ledgeLayerMask);
            if (headHit.collider != null && bodyHit.collider != null)
            {
                Debug.DrawRay(_ledgeHeadRayPoint.position, dir * RAY_DISTANCE, UnityEngine.Color.red);
                Debug.DrawRay(_ledgeBodyRayPoint.position, dir * RAY_DISTANCE, UnityEngine.Color.red);
            }
            else
            {
                Debug.DrawRay(_ledgeHeadRayPoint.position, dir * RAY_DISTANCE, UnityEngine.Color.green);
                Debug.DrawRay(_ledgeBodyRayPoint.position, dir * RAY_DISTANCE, UnityEngine.Color.green);
            }
        }

    }

    public sealed class FallCanTwiceJump : BaseFall
    {
        private bool _isAlreadyTwiceJump = false;
        private bool _isHaveToTwiceJump = false;
        public FallCanTwiceJump(PlayerController controller) : base(controller)  { }
        public void SubscribeTwiceJumpEventHandler(Jump jumpState)
        {
            jumpState.TwiceJumpEventHandler += OnPlayerTwiceJumpInJumpState;
        }
        public void OnPlayerTwiceJumpInJumpState()
        {
            _isAlreadyTwiceJump = true;
        }

        public override void Enter()
        {
            base.Enter();
            _isHaveToTwiceJump = false;
        }
        protected override void ProcessKeyboardInput()
        {
            if (_isAlreadyTwiceJump && !_isHaveToTwiceJump)
            {
                return;
            }

            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _isHaveToTwiceJump = true;
                DoJump(FALL_TO_TWICE_JUMP_FORCE);
            }
        }

        public override void Excute()
        {
            //SetHorizontalMove();
            //FlipSpriteAccodingPlayerInput();
            if (_isHaveToTwiceJump)
            {
                _entity.ChangeState(EPlayerState.FallToTwiceJump);
                return;
            }
            ProcessKeyboardInput();
            if (IsStandGround())
            {
                _entity.ChangeState(EPlayerState.Land);
                return;
            }

            if (IsGrabLedge())
            {
                _entity.ChangeState(EPlayerState.Climb);
            }
            SetHorizontalMove();
            RotateTransformAccodingHorizontalMove();
        }

        public override void Exit()
        {
            _isAlreadyTwiceJump = false;
        }
    }

    public sealed class FallToTwiceJump : BasePlayerState
    {

        public FallToTwiceJump(PlayerController controller) : base(controller) {}
        public override void OnAnimFullyPlayed() { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.Jump);
        }

        public override void FixedExcute()
        {
            if (_entity.RigidBody.velocity.y < 0f)
            {
                _entity.ChangeState(EPlayerState.TwiceJumpToFall);
            }
        }
    }

    public sealed class TwiceJumpToFall : BaseFall
    {
        public TwiceJumpToFall(PlayerController controller) : base(controller) {}
    }

    public sealed class Climb : BasePlayerState
    {
        public Climb(PlayerController controller) : base(controller) { }

        ECharacterLookDir _eCharacterLookDir;
        private const float X_OFFSET = 0.7f;
        private const float Y_OFFSET = 1.4f;
        private const float ANIM_DURATION_HALF_TIME_IN_SEC = 0.3f;

        public override void OnAnimFullyPlayed() { _entity.ChangeState(EPlayerState.Idle); }

        public override void Enter()
        {
            _eCharacterLookDir = _entity.ELookDir;
            PlayAnimation(EPlayerState.Climb);
            Vector3 pos = _entity.transform.position;
            _entity.transform.DOLocalMove(new Vector3(pos.x, pos.y + Y_OFFSET, pos.z), ANIM_DURATION_HALF_TIME_IN_SEC).OnComplete(OnYMoveTWEnd);
        }
        public override void FixedExcute()
        {
            _entity.RigidBody.gravityScale = 0f;
            _entity.RigidBody.velocity = Vector2.zero;
        }
        public override void Exit()
        {
            _entity.RigidBody.gravityScale = 1f;
        }

        public void OnYMoveTWEnd()
        {
            Vector3 pos = _entity.transform.position;
            if (_eCharacterLookDir == ECharacterLookDir.Right)
                _entity.transform.DOLocalMove(new Vector3(pos.x + X_OFFSET, pos.y, pos.z), ANIM_DURATION_HALF_TIME_IN_SEC);
            else
                _entity.transform.DOLocalMove(new Vector3(pos.x - X_OFFSET, pos.y, pos.z), ANIM_DURATION_HALF_TIME_IN_SEC);
        }
    }

    public sealed class Land : BasePlayerState
    {
        public Land(PlayerController controller) : base(controller) { }

        public override void OnAnimFullyPlayed() 
        {
            if (Input.anyKey)
            {
                _entity.ChangeState(EPlayerState.Run);
            }
            else
            {
                _entity.ChangeState(EPlayerState.Idle);
            }
        }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.Land);
        }
    }
    public sealed class Roll : BasePlayerState
    {
        private readonly Vector2 ROLL_FORCE = new(7.5f, 2f);
        public Roll(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() { _entity.ChangeState(EPlayerState.Idle); }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.Roll);
            SetVelocityToZero();
            if (_entity.ELookDir == ECharacterLookDir.Right)
            {
                _entity.RigidBody.AddForce(ROLL_FORCE, ForceMode2D.Impulse);
            }
            else
            {
                _entity.RigidBody.AddForce(new(-ROLL_FORCE.x, ROLL_FORCE.y), ForceMode2D.Impulse);
            }
            _entity.HeadLight.SetActive(false);
        }

        public override void Exit()
        {
            _entity.HeadLight.SetActive(true);
        }
    }
    public abstract class NormalAttackState : BasePlayerState
    {
        // AttackLightController가 구독함.
        static public UnityAction NormalAttackExitEventHandler;
        protected ECharacterLookDir _eLookDir;
        protected bool _isGoToNextAttack;
        protected Transform _attackPoint;
        protected int _layerMask = 1 << ((int)define.EColliderLayer.MonsterBody);
        protected EPlayerNoramlAttackType _eAttackType;
        public NormalAttackState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            _eLookDir = _entity.ELookDir;
            _isGoToNextAttack = false;
            SetVelocityToZero();
            _entity.HeadLight.SetActive(false);
        }

        public override void Exit()
        {
            if (!_isGoToNextAttack)
            {
                NormalAttackExitEventHandler?.Invoke();
                _entity.HeadLight.SetActive(true);
            }
        }

        protected override void ProcessKeyboardInput()
        {
            float currAnimTime = _entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (currAnimTime >= 0.3f && currAnimTime <= 1f)
            {
                if (Input.GetKey(PlayerController.KeyAttack))
                {
                    _isGoToNextAttack = true;
                }
            }
        }
    }

    public sealed class NormalAttack1 : NormalAttackState
    {
        public NormalAttack1(PlayerController controller) : base(controller) 
        {
            _eAttackType = EPlayerNoramlAttackType.Attack_1;
        }
        public override void OnAnimFullyPlayed()
        {
            if (_isGoToNextAttack)
            {
                _entity.ChangeState(EPlayerState.NormalAttack_2);
            }
            else
            {
                _entity.ChangeState(EPlayerState.Idle);
            }
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NormalAttack_1);

            if (_entity.ELookDir == ECharacterLookDir.Left && Input.GetKey(KeyCode.LeftArrow))
            {
                _entity.RigidBody.AddForce(new(-PlayerController.NORMAL_ATTACK_1_DASH_FORCE.x, PlayerController.NORMAL_ATTACK_1_DASH_FORCE.y), ForceMode2D.Impulse);
                return;
            }
            if (_entity.ELookDir == ECharacterLookDir.Right && Input.GetKey(KeyCode.RightArrow))
            {
                _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_1_DASH_FORCE, ForceMode2D.Impulse);
                return;            
            }
        }
        public override void Excute() { ProcessKeyboardInput(); }


    }

    public sealed class NormalAttack2 : NormalAttackState
    {
        public NormalAttack2(PlayerController controller) : base(controller) 
        {
            _eAttackType = EPlayerNoramlAttackType.Attack_2;
        }
        public override void OnAnimFullyPlayed()
        {
            if (_isGoToNextAttack)
            {
                _entity.ChangeState(EPlayerState.NormalAttack_3);
            }
            else
            {
                _entity.ChangeState(EPlayerState.Idle);
            }
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NormalAttack_2);
        }
        public override void Excute() 
        { ProcessKeyboardInput(); }


    }

    public sealed class NormalAttack3 : NormalAttackState
    {
        public NormalAttack3(PlayerController controller) : base(controller) 
        { _eAttackType = EPlayerNoramlAttackType.Attack_3; }

        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.Idle); }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NormalAttack_3);
        }

        public override void Exit()
        {
            NormalAttackExitEventHandler?.Invoke();
        }
    }

    public sealed class CastLaunch : BasePlayerState
    { 
        public CastLaunch(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        {
            // Because of Poped 1 frame.
            PlayAnimation(EPlayerState.Idle);
            _entity.ChangeState(EPlayerState.Idle);
        }
        public override void Enter()  
        {
            _entity.HeadLight.SetActive(false);
            PlayAnimation(EPlayerState.SkillCast);
            SetVelocityToZero();
        }

        public override void Exit()
        {
            _entity.HeadLight.SetActive(true);
        }
    }

    public sealed class CastSpawn : BasePlayerState
    {
        public CastSpawn(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.Idle); }

        public override void Enter() 
        {
            _entity.HeadLight.SetActive(false);
            PlayAnimation(EPlayerState.SkillSpawn);
            SetVelocityToZero();
        }

        public override void Exit()
        {
            _entity.HeadLight.SetActive(true);
        }
    }

    public sealed class Blocking : BasePlayerState
    {
        public Blocking(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.Idle); }
        public override void Enter()        
        {
            _entity.HeadLight.SetActive(false);
            PlayAnimation(EPlayerState.Block); 
        }

        public override void FixedExcute()  
        { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y); }

        public override void Exit()
        {
            _entity.HeadLight.SetActive(true);
        }
    }

    public sealed class BlockSuccess : BasePlayerState
    {
        public BlockSuccess(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.Idle); }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.BlockSucces);
        }
    }

    public abstract class BaseHitted : BasePlayerState
    {
        protected BaseHitted(PlayerController controller) : base(controller)  { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.Idle); }
        public void AdjustKnockbackForce(Vector2 force)
        {
            _entity.RigidBody.velocity = Vector2.zero;
            _entity.RigidBody.AddForce(force, ForceMode2D.Impulse);
        }
    }


    public sealed class HittedMelleAttack : BaseHitted
    {
        public HittedMelleAttack(PlayerController controller) : base(controller) { }

        public override void Enter()
        { PlayAnimation(EPlayerState.HitByMelleAttack); }
    }
    public sealed class HittedParallysis : BaseHitted
    {
        public HittedParallysis(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            SetVelocityToZero();
            PlayAnimation(EPlayerState.HitByStatusParallysis); 
        }
    }
    public sealed class HittedProjectileKnockback : BaseHitted
    {
        public HittedProjectileKnockback(PlayerController controller) : base(controller) { }

        public override void Enter()
        { PlayAnimation(EPlayerState.HitByProjectileKnockback); }

    }

    public sealed class Die : BasePlayerState
    {
        public Die(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { 
            Managers.PlayerRespawn.SpawnPlayer(true);
            Managers.FullScreenEffect.StartFullScreenEffect(EFullScreenEffectType.SCENE_TRANSITION);
        }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.Die);
        }
    }
}