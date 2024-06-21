using define;
using monster_states;
using UnityEngine;
using UnityEngine.Events;
public enum EMonsterState
{ 
    IDLE,
    TRACE,
    ATTACK,
    HITTED_BY_PLAYER_BLOCK_SUCCESS,
    HITTED_BY_PLAYER_SKILL_PARALYSIS,
    HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB,
    DIE,
    COUNT
}

public abstract class BaseMonsterController : BaseCharacterController
{
    public static UnityAction BigAttackEventHandler;
    public static UnityAction HittedByNormalAttackNoArgsEventHandler;
    public static UnityAction<EPlayerNoramlAttackType> HittedByNormalAttackEffectEventHandler;
    public static UnityAction<int, int, int> HittedByNormalAttackWSUIEventHandler;

    public static UnityAction<EMonsterState> MonsterChangeStateEventHandler;

    private readonly static Vector3 sLeftRotationVector = new Vector3(0f, 180f, 0f);
    private readonly static Vector3 sRightRotationVector = new Vector3(0f, 0f, 0f);
    public UIWSMonsterHpBar HealthBar { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public EMonsterNames MonsterType { get; protected set; }
    public MonsterStat Stat { get; protected set; }
    public float AwarenessRangeToTrace { get; private set; }
    public float AwarenessRangeToAttack { get; protected set; }
    public EMonsterState ECurrentState { get; private set; }

    protected StateMachine<BaseMonsterController> _stateMachine;
    protected State<BaseMonsterController>[] _states;
    protected PlayerController _pc;
    public bool IsHittedByPlayerNormalAttack { get; private set; } = false;
    public bool IsDoingInit { get; private set; } = false;
    public override void Init()
    {
        if (HealthBar == null)
        {
            base.Init();
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Assert(PlayerTransform != null);
            _pc = PlayerTransform.gameObject.GetComponent<PlayerController>();
            Stat = gameObject.GetOrAddComponent<MonsterStat>();
            ECurrentState = EMonsterState.IDLE;

            // TODO : 여기 하드코딩 되어 있는 수치들 나중에 다 MonsterData로 빼서 읽어와야 함.
            AwarenessRangeToTrace = 10f;
            HealthBar = Utill.GetComponentInChildrenOrNull<UIWSMonsterHpBar>(gameObject, "UIWSMonsterHpBar");
        }
        InitStat();
        IsHittedByPlayerNormalAttack = false;
    }
    public void InitForRespawn()
    {
        InitStat();
        Init();
        HealthBar.OnMonsterInit();
        ChangeState(EMonsterState.IDLE);
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedExcute();
    }
    void Update()
    {
        _stateMachine.Excute();
    }

    #region CHANGE_TO_HITTED_CALLED_BY_PLAYER
    public void OnHittedByPlayerNormalAttack(ECharacterLookDir eLookDir, int damage, EPlayerNoramlAttackType eAttackType)
    {
        if (ECurrentState != EMonsterState.DIE)
        {
            IsHittedByPlayerNormalAttack = true;

            int beforeDamageHP;
            int AfterDamageHP;
            Stat.OnHitted(damage, out beforeDamageHP, out AfterDamageHP);
            HittedByNormalAttackNoArgsEventHandler?.Invoke();
            HittedByNormalAttackEffectEventHandler?.Invoke(eAttackType);
            HittedByNormalAttackWSUIEventHandler?.Invoke(damage, beforeDamageHP, AfterDamageHP);
            #region PROCESS_BACK_TTACK_OR_THIRD_ATTACK
            if (eAttackType == EPlayerNoramlAttackType.BACK_ATTACK || eAttackType == EPlayerNoramlAttackType.ATTACK_3)
            {
                BigAttackEventHandler?.Invoke();
                Managers.HitParticle.PlayBigHittedParticle(transform.position);
            }
            #endregion
            ((BaseMonsterState)_states[(int)ECurrentState]).OnHittedByPlayerNormalAttack(eLookDir, damage, eAttackType);
            
            IsHittedByPlayerNormalAttack = false;
        }
    }
    public void OnPlayerBlockSuccess()              { ChangeState(EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS); }
    public void OnHittedByPlayerKnockbackBomb()     { ChangeState(EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB); }
    public void OnHittedByPlayerSpawnReaper()       { ChangeState(EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS); }
    #endregion

    #region ANIM_CALLBACK
    public void OnAttackAnimFullyPlayed()       { ((monster_states.Attack)_states[(uint)EMonsterState.ATTACK]).OnAttackAnimFullyPlayed(); }
    public void OnMonsterDieAnimFullyPlayed()   { ((monster_states.Die)_states[(uint)EMonsterState.DIE]).OnDieAnimFullyPlayed(); }
    public void OnMonsterFootStep()             { FootDustParticle.Play(); }
    #endregion

    public void ChangeState(EMonsterState eChangingState)
    {
        ECurrentState = eChangingState;
        _stateMachine.ChangeState(_states[(uint)eChangingState]);
        MonsterChangeStateEventHandler?.Invoke(eChangingState);
    }


    public void OnHittedAnimFullyPlayed()
    {
        switch (ECurrentState)
        {
            case EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS:
                ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS]).OnHittedAnimFullyPlayed();
                return;
            case EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS:
                ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS]).OnHittedAnimFullyPlayed();
                return;
            case EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB:
                ((monster_states.BaseHittedState)_states[(uint)EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB]).OnHittedAnimFullyPlayed();
                return;
        }
    }

    public void SetLookDir()
    {
        if (ECurrentState == EMonsterState.ATTACK ||
            ECurrentState == EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS ||
            ECurrentState == EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS ||
            ECurrentState == EMonsterState.DIE)
        {
            return;
        }
        Vector2 dir = PlayerTransform.position - transform.position;
        if (dir.x > 0)
        {
            ELookDir = define.ECharacterLookDir.RIGHT;
            transform.localRotation = Quaternion.Euler(sRightRotationVector);
        }
        else
        {
            ELookDir = define.ECharacterLookDir.LEFT;
            transform.localRotation = Quaternion.Euler(sLeftRotationVector);
        }
    }
    protected override void InitStates()
    {
        _stateMachine = new StateMachine<BaseMonsterController>();
        _states = new State<BaseMonsterController>[(uint)EMonsterState.COUNT];
        _states[(uint)EMonsterState.IDLE] = new monster_states.Idle(this);
        _states[(uint)EMonsterState.TRACE] = new monster_states.Trace(this);
        _states[(uint)EMonsterState.ATTACK] = new monster_states.Attack(this);
        _states[(uint)EMonsterState.HITTED_BY_PLAYER_BLOCK_SUCCESS] = new monster_states.HittedKnockbackByBlockSuccess(this);
        _states[(uint)EMonsterState.HITTED_BY_PLAYER_SKILL_PARALYSIS] = new monster_states.HittedParalysis(this);
        _states[(uint)EMonsterState.HITTED_BY_PLAYER_SKILL_KNOCKBACK_BOMB] = new monster_states.HittedKnockbackByBomb(this);
        _states[(uint)EMonsterState.DIE] = new monster_states.Die(this);
        _stateMachine.Init(this, _states[(uint)EMonsterState.IDLE]);
    }
    protected abstract void InitStat();

    void OnDrawGizmosSelected()
    {

    }


}
