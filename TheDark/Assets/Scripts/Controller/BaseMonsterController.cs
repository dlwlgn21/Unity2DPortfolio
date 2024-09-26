using define;
using monster_states;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseMonsterController : BaseCharacterController
{
    public static UnityAction BigAttackEventHandler;
    public static UnityAction HittedByNormalAttackNoArgsEventHandler; // DamageFlash, CamShake
    public static UnityAction<EPlayerNoramlAttackType> HittedByNormalAttackEffectEventHandler;
    public static UnityAction<int, int, int> HittedByNormalAttackWSUIEventHandler;


    protected readonly static Vector3 LEFT_ROT_VECTOR = new(0f, 180f, 0f);
    protected readonly static Vector3 RIGHT_ROT_VECTOR = new(0f, 0f, 0f);
    public UIWSMonsterHpBar HealthBar { get; protected set; }
    public Transform PlayerTransform { get; protected set; }
    public EMonsterNames EMonsterType { get; protected set; }
    public MonsterStat Stat { get; protected set; }

    protected PlayerController _pc;
    public bool IsHittedByPlayerNormalAttack { get; protected set; } = false;

    public override void Init()
    {
        if (HealthBar == null)
        {
            base.Init();
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Assert(PlayerTransform != null);
            _pc = PlayerTransform.gameObject.GetComponent<PlayerController>();
            Stat = gameObject.GetOrAddComponent<MonsterStat>();
            HealthBar = Utill.GetComponentInChildrenOrNull<UIWSMonsterHpBar>(gameObject, "UIWSMonsterHpBar");
        }
        InitStat();
    }

    public abstract void InitStat();
    public abstract void OnHittedByPlayerNormalAttack(ECharacterLookDir eLookDir, int damage, EPlayerNoramlAttackType eAttackType);
    public abstract void OnPlayerBlockSuccess();
    public abstract void OnHittedByPlayerKnockbackBomb();
    public abstract void OnHittedByPlayerSpawnReaper();
    public abstract void OnDie();


    protected void DecreasHpAndInvokeHitEvents(int damage, EPlayerNoramlAttackType eAttackType)
    {
        int beforeDamageHP;
        int AfterDamageHP;
        int actualDamage = Stat.DecreaseHpAndGetActualDamageAmount(damage, out beforeDamageHP, out AfterDamageHP);
        HittedByNormalAttackNoArgsEventHandler?.Invoke();
        HittedByNormalAttackEffectEventHandler?.Invoke(eAttackType);
        HittedByNormalAttackWSUIEventHandler?.Invoke(actualDamage, beforeDamageHP, AfterDamageHP);
    }
}
