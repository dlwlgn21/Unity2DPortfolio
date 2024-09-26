using define;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace player_states
{

    public abstract class BasePlayerState : State<PlayerController>
    {
        protected float _horizontalMove;
        protected float _groundCheckDistance = 0.2f;
        public static LayerMask sGroundLayerMask = (1 << (int)define.EColliderLayer.PLATFORM) | (1 << (int)define.EColliderLayer.LEDGE_CLIMB);
        private readonly static Vector3 S_LEFT_ROTATION_VECTOR = new Vector3(0f, 180f, 0f);
        private readonly static Vector3 S_RIGHT_ROTATION_VECTOR = new Vector3(0f, 0f, 0f);
        public BasePlayerState(PlayerController controller) : base(controller) { }
        public abstract void OnAnimFullyPlayed();
        
        public override void Excute() { }
        public virtual void ProcessKeyboardInput() { }
        public void FlipSpriteAccodingPlayerInput()
        {

            // TODO : 이방식 그냥 horizontalMoveInput 으로 바꿔도 되는지 생각해보자.
            if (Input.GetKey(KeyCode.LeftArrow) && _entity.ELookDir == ECharacterLookDir.RIGHT)
            {
                _entity.ELookDir = ECharacterLookDir.LEFT;
                _entity.transform.localRotation = Quaternion.Euler(S_LEFT_ROTATION_VECTOR);
                _entity.CamFollowObject.CallTurn();

            }
            else if (Input.GetKey(KeyCode.RightArrow) && _entity.ELookDir == ECharacterLookDir.LEFT)
            {
                _entity.ELookDir = ECharacterLookDir.RIGHT;
                _entity.transform.localRotation = Quaternion.Euler(S_RIGHT_ROTATION_VECTOR);
                _entity.CamFollowObject.CallTurn();
            }
        }
        protected void SetHorizontalMove()  { _horizontalMove = Input.GetAxisRaw("Horizontal"); }
        protected void SetVelocityToZero() { _entity.RigidBody.velocity = Vector2.zero; }


        protected bool IsStandGround()
        {
            // TODO : 이곳 나중에 코드 정리해야 함.
            Bounds bound = _entity.CapsuleCollider.bounds;
            float dist = bound.extents.y + 0.1f;
            float xDiff = 0.15f;
            Vector3 rightDiff = new Vector3(xDiff, 0f, 0f);
            Vector3 leftDiff = new Vector3(-xDiff, 0f, 0f);
            var rightHit = Physics2D.Raycast(bound.center + rightDiff, Vector2.down, dist, sGroundLayerMask);
            var leftHit = Physics2D.Raycast(bound.center + leftDiff, Vector2.down, dist, sGroundLayerMask);
            if (leftHit.collider == null && rightHit.collider == null)
            {
                Debug.DrawRay(bound.center + rightDiff, Vector2.down * dist, Color.green);
                Debug.DrawRay(bound.center + leftDiff, Vector2.down * dist, Color.green);
                return false;
            }
            Debug.DrawRay(bound.center + rightDiff, Vector2.down * dist, Color.red);
            Debug.DrawRay(bound.center + leftDiff, Vector2.down * dist, Color.red);
            return true;
        }
        protected void PlayAnimation(EPlayerState eState)
        {
            switch (eState) 
            {
                case EPlayerState.IDLE:
                    _entity.Animator.Play("Idle", -1, 0f);
                    return;
                case EPlayerState.RUN:
                    _entity.Animator.Play("Run", -1, 0f);
                    return;
                case EPlayerState.ROLL:
                    _entity.Animator.Play("Roll", -1, 0f);
                    return;
                case EPlayerState.JUMP:
                    _entity.Animator.Play("Jump", -1, 0f);
                    return;
                case EPlayerState.CLIMB:
                    _entity.Animator.Play("Climb", -1, 0f);
                    return;
                case EPlayerState.FALL:
                    _entity.Animator.Play("Fall", -1, 0f);
                    return;
                case EPlayerState.LAND:
                    _entity.Animator.Play("Land", -1, 0f);
                    return;
                case EPlayerState.NORMAL_ATTACK_1:
                    _entity.Animator.Play("NormalAttack1", -1, 0f);
                    return;
                case EPlayerState.NORMAL_ATTACK_2:
                    _entity.Animator.Play("NormalAttack2", -1, 0f);
                    return;
                case EPlayerState.NORMAL_ATTACK_3:
                    _entity.Animator.Play("NormalAttack3", -1, 0f);
                    return;
                case EPlayerState.CAST_LAUNCH:
                    _entity.Animator.Play("Launch", -1, 0f);
                    return;
                case EPlayerState.CAST_SPAWN:
                    _entity.Animator.Play("SpawnReaper", -1, 0f);
                    return;
                case EPlayerState.HITTED_MELLE_ATTACK:
                    _entity.Animator.Play("Hitted", -1, 0f);
                    return;
                case EPlayerState.HITTED_STATUS_PARALLYSIS:
                    _entity.Animator.Play("HittedStatusParallysis", -1, 0f);
                    return;
                case EPlayerState.HITTED_PROJECTILE_KONCKBACK:
                    _entity.Animator.Play("HittedProjectileKnockback", -1, 0f);
                    return;
                case EPlayerState.BLOCKING:
                    _entity.Animator.Play("Blocking", -1, 0f);
                    return;
                case EPlayerState.BLOCK_SUCESS:
                    _entity.Animator.Play("BlockSuccess", -1, 0f);
                    return;
                case EPlayerState.DIE:
                    _entity.Animator.Play("Die", -1, 0f);
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
        public override void OnAnimFullyPlayed() { }
        public override void ProcessKeyboardInput()
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                _entity.ChangeState(EPlayerState.RUN);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyAttack))
            {
                _entity.ChangeState(EPlayerState.NORMAL_ATTACK_1);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                _entity.ChangeState(EPlayerState.BLOCKING);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _entity.ChangeState(EPlayerState.JUMP);
                return;
            }
        }
        public override void Enter()       
        { PlayAnimation(EPlayerState.IDLE); }

        public override void FixedExcute() 
        { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y); }
        public override void Excute()
        {
            SetHorizontalMove();
            if (!IsStandGround())
            {
                _entity.ChangeState(EPlayerState.FALL);
                return;
            }
            FlipSpriteAccodingPlayerInput();
            ProcessKeyboardInput();
        }


    }
    public class Run : BasePlayerState
    {
        public Run(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() { }

        public override void ProcessKeyboardInput()
        {
            // ChangeState
            if (!Input.anyKey)
            {
                _entity.ChangeState(EPlayerState.IDLE);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyBlock))
            {
                _entity.ChangeState(EPlayerState.BLOCKING);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyJump))
            {
                _entity.ChangeState(EPlayerState.JUMP);
                return;
            }
            if (Input.GetKeyDown(PlayerController.KeyAttack))
            {
                _entity.ChangeState(EPlayerState.NORMAL_ATTACK_1);
                return;
            }
        }

        public override void Enter() 
        { PlayAnimation(EPlayerState.RUN); }

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
                _entity.ChangeState(EPlayerState.FALL);
                return;
            }
            FlipSpriteAccodingPlayerInput();
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


    public class Jump : InAir
    {
        // TODO : Static UnityAction으로 바꿔야 함.
        public Action TwiceJumpEventHandler;

        private bool _isInAir;
        private bool _isTwiceJump;
        private bool _isJumpKeyDownTwice;

        public Jump(PlayerController controller) : base(controller) { }

        ~Jump() { TwiceJumpEventHandler = null; }

        public override void ProcessKeyboardInput()
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
            PlayAnimation(EPlayerState.JUMP);
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
                    _entity.ChangeState(EPlayerState.FALL);
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
            FlipSpriteAccodingPlayerInput();
        }
    }

    public abstract class BaseFall : InAir
    {
        protected Transform _ledgeHeadRayPoint;
        protected Transform _ledgeBodyRayPoint;
        protected LayerMask _ledgeLayerMask = 1 << (int)define.EColliderLayer.LEDGE_CLIMB;
        protected ECharacterLookDir _eCharacterLookDir;
        protected float _extraHeight = 0.2f;
        protected const float RAY_DISTANCE = 0.4f;
        protected BaseFall(PlayerController controller) : base(controller) { }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.FALL);
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
                _entity.ChangeState(EPlayerState.LAND);
                return;
            }

            if (IsGrabLedge())
            {
                _entity.ChangeState(EPlayerState.CLIMB);
            }
            SetHorizontalMove();
            FlipSpriteAccodingPlayerInput();
        }

        public bool IsGrabLedge()
        {
            RaycastHit2D headRayHit = new RaycastHit2D();
            RaycastHit2D bodyRayHit = new RaycastHit2D();
            if (_eCharacterLookDir == ECharacterLookDir.RIGHT)
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

        private void RaycastToLedgeLayer(ref RaycastHit2D headHit, ref RaycastHit2D bodyHit, Vector2 dir)
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

    public class FallCanTwiceJump : BaseFall
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
        public override void ProcessKeyboardInput()
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
                _entity.ChangeState(EPlayerState.FALL_TO_TWICE_JUMP);
                return;
            }
            ProcessKeyboardInput();
            if (IsStandGround())
            {
                _entity.ChangeState(EPlayerState.LAND);
                return;
            }

            if (IsGrabLedge())
            {
                _entity.ChangeState(EPlayerState.CLIMB);
            }
            SetHorizontalMove();
            FlipSpriteAccodingPlayerInput();
        }

        public override void Exit()
        {
            _isAlreadyTwiceJump = false;
        }
    }

    public class FallToTwiceJump : BasePlayerState
    {

        public FallToTwiceJump(PlayerController controller) : base(controller) {}
        public override void OnAnimFullyPlayed() { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.JUMP);
        }

        public override void FixedExcute()
        {
            if (_entity.RigidBody.velocity.y < 0f)
            {
                _entity.ChangeState(EPlayerState.TWICE_JUMP_TO_FALL);
            }
        }
    }

    public class TwiceJumpToFall : BaseFall
    {
        public TwiceJumpToFall(PlayerController controller) : base(controller) {}
    }

    public class Climb : BasePlayerState
    {
        public Climb(PlayerController controller) : base(controller) { }

        ECharacterLookDir _eCharacterLookDir;
        private const float X_OFFSET = 0.7f;
        private const float Y_OFFSET = 1.4f;
        private const float ANIM_DURATION_HALF_TIME = 0.3f;

        public override void OnAnimFullyPlayed() { _entity.ChangeState(EPlayerState.IDLE); }

        public override void Enter()
        {
            _eCharacterLookDir = _entity.ELookDir;
            PlayAnimation(EPlayerState.CLIMB);
            Vector3 pos = _entity.transform.position;
            _entity.transform.DOLocalMove(new Vector3(pos.x, pos.y + Y_OFFSET, pos.z), ANIM_DURATION_HALF_TIME).OnComplete(OnYMoveTWEnd);
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
            if (_eCharacterLookDir == ECharacterLookDir.RIGHT)
                _entity.transform.DOLocalMove(new Vector3(pos.x + X_OFFSET, pos.y, pos.z), ANIM_DURATION_HALF_TIME);
            else
                _entity.transform.DOLocalMove(new Vector3(pos.x - X_OFFSET, pos.y, pos.z), ANIM_DURATION_HALF_TIME);
        }
    }

    public class Land : BasePlayerState
    {
        public Land(PlayerController controller) : base(controller) { }

        public override void OnAnimFullyPlayed() 
        {
            if (Input.anyKey)
            {
                _entity.ChangeState(EPlayerState.RUN);
            }
            else
            {
                _entity.ChangeState(EPlayerState.IDLE);
            }
        }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.LAND);
        }
    }
    public class Roll : BasePlayerState
    {
        private readonly Vector2 ROLL_FORCE = new Vector2(7.5f, 2f);
        public Roll(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed() { _entity.ChangeState(EPlayerState.RUN); }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.ROLL);
            SetVelocityToZero();
            if (_entity.ELookDir == ECharacterLookDir.RIGHT)
            {
                _entity.RigidBody.AddForce(ROLL_FORCE, ForceMode2D.Impulse);
            }
            else
            {
                _entity.RigidBody.AddForce(new(-ROLL_FORCE.x, ROLL_FORCE.y), ForceMode2D.Impulse);
            }
        }
    }
    public abstract class NormalAttackState : BasePlayerState
    {
        // AttackLightController가 구독함.
        static public UnityAction NormalAttackExitEventHandler;
        protected ECharacterLookDir _eLookDir;
        protected bool _isGoToNextAttack;
        protected Transform _attackPoint;
        protected int _layerMask = 1 << ((int)define.EColliderLayer.MONSTERS_BODY);
        protected EPlayerNoramlAttackType _eAttackType;
        public NormalAttackState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            _eLookDir = _entity.ELookDir;
            _isGoToNextAttack = false;
            SetVelocityToZero();
        }

        public override void Exit()
        {
            if (!_isGoToNextAttack)
            {
                NormalAttackExitEventHandler?.Invoke();
            }
        }

        public override void ProcessKeyboardInput()
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

    public class NormalAttack1 : NormalAttackState
    {
        public NormalAttack1(PlayerController controller) : base(controller) 
        {
            _eAttackType = EPlayerNoramlAttackType.ATTACK_1;
        }
        public override void OnAnimFullyPlayed()
        {
            if (_isGoToNextAttack)
            {
                _entity.ChangeState(EPlayerState.NORMAL_ATTACK_2);
            }
            else
            {
                _entity.ChangeState(EPlayerState.IDLE);
            }
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_1);

            if (_entity.ELookDir == ECharacterLookDir.LEFT && Input.GetKey(KeyCode.LeftArrow))
            {
                _entity.RigidBody.AddForce(new(-PlayerController.NORMAL_ATTACK_1_DASH_FORCE.x, PlayerController.NORMAL_ATTACK_1_DASH_FORCE.y), ForceMode2D.Impulse);
                return;
            }
            if (_entity.ELookDir == ECharacterLookDir.RIGHT && Input.GetKey(KeyCode.RightArrow))
            {
                _entity.RigidBody.AddForce(PlayerController.NORMAL_ATTACK_1_DASH_FORCE, ForceMode2D.Impulse);
                return;            
            }
        }
        public override void Excute() { ProcessKeyboardInput(); }


    }

    public class NormalAttack2 : NormalAttackState
    {
        public NormalAttack2(PlayerController controller) : base(controller) 
        {
            _eAttackType = EPlayerNoramlAttackType.ATTACK_2;
        }
        public override void OnAnimFullyPlayed()
        {
            if (_isGoToNextAttack)
            {
                _entity.ChangeState(EPlayerState.NORMAL_ATTACK_3);
            }
            else
            {
                _entity.ChangeState(EPlayerState.IDLE);
            }
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_2);
        }
        public override void Excute() 
        { ProcessKeyboardInput(); }


    }

    public class NormalAttack3 : NormalAttackState
    {
        public NormalAttack3(PlayerController controller) : base(controller) 
        { _eAttackType = EPlayerNoramlAttackType.ATTACK_3; }

        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.IDLE); }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_3);
        }

        public override void Exit()
        {
            NormalAttackExitEventHandler?.Invoke();
        }
    }

    public class CastLaunch : BasePlayerState
    { 
        public CastLaunch(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        {
            // Because of Poped 1 frame.
            PlayAnimation(EPlayerState.IDLE);
            _entity.ChangeState(EPlayerState.IDLE);
        }
        public override void Enter()  
        { 
            PlayAnimation(EPlayerState.CAST_LAUNCH);
            SetVelocityToZero();
        }
    }

    public class CastSpawn : BasePlayerState
    {
        public CastSpawn(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.IDLE); }

        public override void Enter() 
        { 
            PlayAnimation(EPlayerState.CAST_SPAWN);
            SetVelocityToZero();
        }
    }

    public class Blocking : BasePlayerState
    {
        public Blocking(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.IDLE); }
        public override void Enter()        
        { PlayAnimation(EPlayerState.BLOCKING); }

        public override void FixedExcute()  
        { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y); }
    }

    public class BlockSuccess : BasePlayerState
    {
        public BlockSuccess(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.IDLE); }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.BLOCK_SUCESS);
        }
    }

    public abstract class BaseHitted : BasePlayerState
    {
        protected BaseHitted(PlayerController controller) : base(controller)  { }
        public override void OnAnimFullyPlayed()
        { _entity.ChangeState(EPlayerState.IDLE); }
        public void AdjustKnockbackForce(Vector2 force)
        {
            _entity.RigidBody.velocity = Vector2.zero;
            _entity.RigidBody.AddForce(force, ForceMode2D.Impulse);
        }
    }


    public class HittedMelleAttack : BaseHitted
    {
        public HittedMelleAttack(PlayerController controller) : base(controller) { }

        public override void Enter()
        { PlayAnimation(EPlayerState.HITTED_MELLE_ATTACK); }
    }
    public class HittedParallysis : BaseHitted
    {
        public HittedParallysis(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            SetVelocityToZero();
            PlayAnimation(EPlayerState.HITTED_STATUS_PARALLYSIS); 
        }
    }
    public class HittedProjectileKnockback : BaseHitted
    {
        public HittedProjectileKnockback(PlayerController controller) : base(controller) { }

        public override void Enter()
        { PlayAnimation(EPlayerState.HITTED_PROJECTILE_KONCKBACK); }

    }

    public class Die : BasePlayerState
    {
        public Die(PlayerController controller) : base(controller) { }
        public override void OnAnimFullyPlayed()
        { 
            Managers.PlayerRespawn.SpawnPlayer(_entity);
            Managers.FullScreenEffect.StartFullScreenEffect(EFullScreenEffectType.SCENE_TRANSITION);
        }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.DIE);
        }
    }
}