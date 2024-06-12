using define;
using DG.Tweening;
using System;
using UnityEngine;

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
        
        public override void Excute() { ProcessHorizontalInput(); }
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

        protected void ProcessHorizontalInput()  { _horizontalMove = Input.GetAxisRaw("Horizontal"); }
        protected void SetVelocityToZero() { _entity.RigidBody.velocity = Vector2.zero; }
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
                case EPlayerState.HITTED:
                    _entity.Animator.Play("Hitted", -1, 0f);
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

    public abstract class BaseCanSkillState : BasePlayerState
    {
        public BaseCanSkillState(PlayerController controller) : base(controller) {}

        public override void Excute() 
        { 
            base.Excute();
        }

        protected bool IsChangeStateToRoll()
        {
            if (Input.GetKeyDown(PlayerController.KeyRoll))
            {
                if (_entity.IsPossibleRoll)
                {
                    _entity.ChangeState(EPlayerState.ROLL);
                    _entity.RollCoolTimerImg.StartCoolTime(_entity.RollCollTime);
                    _entity.IsPossibleRoll = false;
                    return true;
                }
            }
            return false;
        }

        protected bool IsChangeStateToLaunchBomb()
        {
            if (Input.GetKeyDown(PlayerController.KeyLaunchBomb))
            {
                if (_entity.IsPossibleLaunchBomb)
                {
                    _entity.ChangeState(EPlayerState.CAST_LAUNCH);
                    _entity.BombCoolTimerImg.StartCoolTime(_entity.BombCollTime);
                    _entity.IsPossibleLaunchBomb = false;
                    return true;
                }
            }
            return false;
        }
        protected bool IsChangeStateToSpawnReaper()
        {
            if (Input.GetKeyDown(PlayerController.KeySpawnReaper))
            {
                if (_entity.IsPossibleSpawnReaper)
                {
                    _entity.ChangeState(EPlayerState.CAST_SPAWN);
                    _entity.SpawnReaperCoolTimerImg.StartCoolTime(_entity.SpawnReaperCollTime);
                    _entity.IsPossibleSpawnReaper = false;
                    return true;
                }
            }
            return false;
        }
    }


    public class Idle : BaseCanSkillState
    {
        public Idle(PlayerController controller) : base(controller) { }
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

        public override void Enter()       { PlayAnimation(EPlayerState.IDLE); }

        public override void FixedExcute() { _entity.RigidBody.velocity = new Vector2(0f, _entity.RigidBody.velocity.y); }
        public override void Excute()
        {
            base.Excute();
            FlipSpriteAccodingPlayerInput();
            if (IsChangeStateToRoll())
            {
                return;
            }
            if (IsChangeStateToLaunchBomb())
            {
                return;
            }
            if (IsChangeStateToSpawnReaper())
            {
                return;
            }
            ProcessKeyboardInput();
        }
    }
    public class Run : BaseCanSkillState
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
            {
                // 5.13에 return 추가함.
                _entity.ChangeState(EPlayerState.FALL);
                return;
            }
            FlipSpriteAccodingPlayerInput();
            if (IsChangeStateToRoll())
            {
                return;
            }
            if (IsChangeStateToLaunchBomb())
            {
                return;
            }
            if (IsChangeStateToSpawnReaper())
            {
                return;
            }
            ProcessKeyboardInput();
        }

    }

    public abstract class InAir : BasePlayerState
    {
        public static float sFirstJumpForce = 9f;
        public static float sSecondJumpForce = 5f;
        public static float sFallToTwiceJumpForce = 12f;
        public InAir(PlayerController controller) : base(controller)  {  }

        protected void ChangeVelocity()
        {
            Vector2 oriVelocity = _entity.RigidBody.velocity;
            _entity.RigidBody.velocity = new Vector2(_horizontalMove * _entity.Stat.MoveSpeed * Time.fixedDeltaTime, oriVelocity.y);
        }
        protected void DoJump(float upForce)
        {
            ChangeVelocity();
            _entity.RigidBody.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
            _entity.PlayMovementEffectAnimation(EPlayerMovementEffect.JUMP);
        }
    }


    public class Jump : InAir
    {
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
                DoJump(sSecondJumpForce);
                _isTwiceJump = false;
                // 6.10일 FallState에서도 이단점프 가능하게 하기 위해서 추가한 부분.
                if (TwiceJumpEventHandler != null)
                {
                    TwiceJumpEventHandler.Invoke();
                }
            }
            if (!_isInAir)
            {
                DoJump(sFirstJumpForce);
                _isInAir = true;
            }
            else
            {
                ChangeVelocity();
                if (_entity.RigidBody.velocity.y <= 0f)
                {
                    _entity.ChangeState(EPlayerState.FALL);
                }
            }
        }
        public override void Excute()
        {
            base.Excute();
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
        protected Transform _ledgeCheckPoint;
        protected LayerMask _ledgeLayerMask = 1 << (int)define.EColliderLayer.LEDGE_CLIMB;
        protected ECharacterLookDir _eCharacterLookDir;
        protected float _extraHeight = 0.2f;
        protected BaseFall(PlayerController controller) : base(controller) { }
        public override void Enter()
        {
            PlayAnimation(EPlayerState.FALL);
            if (_ledgeCheckPoint == null)
            {
                _ledgeCheckPoint = _entity.LedgeCheckPoint;
            }
            _eCharacterLookDir = _entity.ELookDir;
        }
        public override void FixedExcute()
        {
            ChangeVelocity();
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
            {
                _entity.ChangeState(EPlayerState.CLIMB);
            }
        }

        public bool IsGrabLedge()
        {
            float dist = 0.75f;
            RaycastHit2D hit;
            if (_eCharacterLookDir == ECharacterLookDir.RIGHT)
            {
                hit = Physics2D.Raycast(_ledgeCheckPoint.position, Vector2.right, dist, _ledgeLayerMask);
                Debug.DrawRay(_ledgeCheckPoint.position, Vector2.right * dist, UnityEngine.Color.red);
            }
            else
            {
                hit = Physics2D.Raycast(_ledgeCheckPoint.position, Vector2.left, dist, _ledgeLayerMask);
                Debug.DrawRay(_ledgeCheckPoint.position, Vector2.left * dist, UnityEngine.Color.red);
            }
            if (hit.collider != null)
            {
                return true;
            }
            return false;
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
                DoJump(sFallToTwiceJumpForce);
            }
        }

        public override void Excute()
        {
            base.Excute();
            Bounds bound = _entity.BoxCollider.bounds;
            var hit = Physics2D.BoxCast(bound.center, bound.size, 0f, Vector2.down, _extraHeight, sGroundLayerMask);
            BoxCast2DDebugDraw(bound.center, bound.size, _extraHeight, hit);
            FlipSpriteAccodingPlayerInput();
            if (_isHaveToTwiceJump)
            {
                _entity.ChangeState(EPlayerState.FALL_TO_TWICE_JUMP);
                return;
            }

            ProcessKeyboardInput();
            if (hit.collider != null)
            {
                _entity.ChangeState(EPlayerState.LAND);
                return;
            }

            if (IsGrabLedge())
            {
                _entity.ChangeState(EPlayerState.CLIMB);
            }
        }

        public override void Exit()
        {
            _isAlreadyTwiceJump = false;
        }
    }

    public class FallToTwiceJump : BasePlayerState
    {
        public FallToTwiceJump(PlayerController controller) : base(controller) {}

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

        public void OnClimbAnimFullyPlayed() { _entity.ChangeState(EPlayerState.IDLE); }
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
        public override void Enter()
        {
            PlayAnimation(EPlayerState.LAND);
            _entity.FootDustParticle.Play();
            _entity.PlayMovementEffectAnimation(EPlayerMovementEffect.LAND);
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
        private const float HORIZONTAL_ROLL_FORCE = 7.5f;
        private const float VERTICAL_ROLL_FORCE = 2f;
        public Roll(PlayerController controller) : base(controller) { }

        ECharacterLookDir _eLookDir;
        int _layerMask = (1 << (int)EColliderLayer.MONSTERS) | (1 << (int)EColliderLayer.PLATFORM) | (1 << (int)EColliderLayer.ENV) | (1 << (int)EColliderLayer.EVENT_BOX) | (1 << (int)EColliderLayer.LEDGE_CLIMB);
        public void OnRollAnimFullyPlayed() { _entity.ChangeState(EPlayerState.RUN); }
        public override void Enter()
        {
            _eLookDir = _entity.ELookDir;
            _entity.FootDustParticle.Play();
            Managers.Sound.Play(DataManager.SFX_PLAYER_ROLLING_PATH);
            PlayAnimation(EPlayerState.ROLL);
            _entity.RigidBody.velocity = Vector2.zero;
            if (_eLookDir == ECharacterLookDir.RIGHT)
            {
                _entity.RigidBody.AddForce(new Vector2(HORIZONTAL_ROLL_FORCE, VERTICAL_ROLL_FORCE), ForceMode2D.Impulse);
            }
            else
            {
                _entity.RigidBody.AddForce(new Vector2(-HORIZONTAL_ROLL_FORCE, VERTICAL_ROLL_FORCE), ForceMode2D.Impulse);
            }
            Physics2D.IgnoreLayerCollision((int)EColliderLayer.MONSTERS, (int)EColliderLayer.PLAYER);
        }
        public override void FixedExcute()
        {
            // 6.5 AddForce 방식으로 로 바꿈.
            //Vector2 oriVelo = _entity.RigidBody.velocity;
            //float speed = _entity.Stat.MoveSpeed * 1.5f;
            //if (_eLookDir == ECharacterLookDir.RIGHT)
            //    _entity.RigidBody.velocity = new Vector2(speed * Time.fixedDeltaTime, oriVelo.y);
            //else
            //    _entity.RigidBody.velocity = new Vector2(speed * -Time.fixedDeltaTime, oriVelo.y);
        }

        public override void Exit() { Physics2D.SetLayerCollisionMask((int)EColliderLayer.PLAYER, _layerMask); }
    }
    public abstract class NormalAttackState : BasePlayerState
    {
        static readonly protected Vector2 S_RIGHT_MOVE_FORCE = new Vector2(5f, 2f); 
        static readonly protected Vector2 S_LEFT_MOVE_FORCE = new Vector2(-S_RIGHT_MOVE_FORCE.x, S_RIGHT_MOVE_FORCE.y); 

        protected ECharacterLookDir _eLookDir;
        protected bool _isGoToNextAttack;
        protected Transform _attackPoint;
        protected int _layerMask = 1 << ((int)define.EColliderLayer.MONSTERS);
        protected EPlayerNoramlAttackType _eAttackType;
        public NormalAttackState(PlayerController controller) : base(controller) { }
        public void DamageHittedMonsters()
        {
            Collider2D[] monsters = Physics2D.OverlapCircleAll(_attackPoint.position, 1f, _layerMask);
            if (monsters == null)
                return;

            foreach (Collider2D mon in monsters)
            {
                BaseMonsterController controller = mon.gameObject.GetComponent<BaseMonsterController>();
                Debug.Assert(controller != null);
                controller.HittedByPlayerNormalAttack(_entity, _eAttackType);
            }
        }
        public abstract void OnAttackAnimFullyPlayed();

        public override void Enter()
        {
            _attackPoint = _entity.NormalAttackPoint;
            _eLookDir = _entity.ELookDir;
            _isGoToNextAttack = false;

            // AttackLight 추가된 파트
            _entity.AttackLightController.TurnOnLight();
            

            //_entity.RotateAttackLightAccodingCharacterLookDir();
            SetVelocityToZero();
        }

        public override void Exit()
        {
            if (!_isGoToNextAttack)
            {
                _entity.AttackLightController.TurnOffLightGradually();
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
        public override void OnAttackAnimFullyPlayed()
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
            Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_1_PATH);

            if (_entity.ELookDir == ECharacterLookDir.LEFT)
            {
                _entity.RigidBody.AddForce(S_LEFT_MOVE_FORCE, ForceMode2D.Impulse);
            }
            else
            {
                _entity.RigidBody.AddForce(S_RIGHT_MOVE_FORCE, ForceMode2D.Impulse);
            }
            _entity.PlayMovementEffectAnimation(EPlayerMovementEffect.NORMAL_ATTACK_1);
        }
        public override void Excute() { ProcessKeyboardInput(); }


    }

    public class NormalAttack2 : NormalAttackState
    {
        public NormalAttack2(PlayerController controller) : base(controller) 
        {
            _eAttackType = EPlayerNoramlAttackType.ATTACK_2;
        }
        public override void OnAttackAnimFullyPlayed()
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
            Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_2_PATH);
        }
        public override void Excute() { ProcessKeyboardInput(); }


    }

    public class NormalAttack3 : NormalAttackState
    {
        public NormalAttack3(PlayerController controller) : base(controller) 
        {
            _eAttackType = EPlayerNoramlAttackType.ATTACK_3;
        }
        public override void OnAttackAnimFullyPlayed()
        {
            _entity.ChangeState(EPlayerState.IDLE);
        }
        public override void Enter()
        {
            base.Enter();
            PlayAnimation(EPlayerState.NORMAL_ATTACK_3);
            Managers.Sound.Play(DataManager.SFX_PLAYER_SWING_3_PATH);
        }
        public override void Excute() { }

        public override void Exit()
        {
            _entity.AttackLightController.TurnOffLightGradually();
        }
    }

    public class CastLaunch : BasePlayerState
    { 
        public CastLaunch(PlayerController controller) : base(controller) { }

        public void OnLaunchAnimFullyPlayed() 
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

        public void OnSpawnAnimFullyPlayed() { _entity.ChangeState(EPlayerState.IDLE); }
        public override void Enter() 
        { 
            PlayAnimation(EPlayerState.CAST_SPAWN);
            SetVelocityToZero();
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
        private const float KNOCKBACK_FORCE = 3f;

        public BlockSuccess(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            PlayAnimation(EPlayerState.BLOCK_SUCESS);
            _entity.StatusText.ShowPopup("Block!");
            Managers.CamShake.CamShake(ECamShakeType.PLAYER_BLOCK_SUCCES);
            _isKnockbackFlag = false;
        }

        public override void FixedExcute()
        {
            if (!_isKnockbackFlag)
            {
                if (_entity.ELookDir == define.ECharacterLookDir.LEFT)
                {
                    _entity.RigidBody.AddForce(Vector2.right * KNOCKBACK_FORCE, ForceMode2D.Impulse);
                }
                else
                {
                    _entity.RigidBody.AddForce(Vector2.left * KNOCKBACK_FORCE, ForceMode2D.Impulse);
                }
                _isKnockbackFlag = true;
            }
        }

        public override void Excute()
        {
            if (IsAnimEnd())
            {
                _entity.ChangeState(EPlayerState.IDLE);
            }
        }
    }

    public class Hitted : BasePlayerState
    {
        private const float KNOCKBACK_FORCE = 2f;
        public Hitted(PlayerController controller) : base(controller) { }

        public void OnHittedAnimFullyPlayed() 
        {
            Debug.Log("Hit Anim End!!");
            _entity.ChangeState(EPlayerState.RUN); 
        }
        public override void Enter()
        {
            if (!_entity.HitEffectAniamtor.gameObject.activeSelf)
                _entity.HitEffectAniamtor.gameObject.SetActive(true);
            int randIdx = UnityEngine.Random.Range(0, 1);
            if (randIdx % 2 == 0)
            {
                Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_1_PATH);
            }
            else
            {
                Managers.Sound.Play(DataManager.SFX_PLAYER_HIT_2_PATH);
            }

            if (_entity.ELookDir == define.ECharacterLookDir.LEFT)
            {
                _entity.RigidBody.AddForce(new Vector2(KNOCKBACK_FORCE, KNOCKBACK_FORCE), ForceMode2D.Impulse);
            }
            else
            {
                _entity.RigidBody.AddForce(new Vector2(-KNOCKBACK_FORCE, KNOCKBACK_FORCE), ForceMode2D.Impulse);
            }


            PlayAnimation(EPlayerState.HITTED);
            Managers.CamShake.CamShake(ECamShakeType.PLAYER_HITTED_BY_MONSTER);
            _entity.PlayEffectForceField();
            // TODO : 플레이어 HitEffectAnimation 살릴지 말지 결정해야 함.
            //_entity.HitEffectAniamtor.Play(BaseCharacterController.HIT_EFFECT_3_KEY, -1, 0f);
            Managers.TimeManager.OnPlayerHittedByMonster();
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