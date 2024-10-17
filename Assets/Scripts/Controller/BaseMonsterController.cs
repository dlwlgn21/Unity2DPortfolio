using define;
using monster_states;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.UI;

public abstract class BaseMonsterController : BaseCharacterController
{
    public static UnityAction BigAttackEventHandler;
    public static UnityAction HittedByNormalAttackNoArgsEventHandler; // CamShake
    public static UnityAction<EPlayerNoramlAttackType> HittedByNormalAttackEffectEventHandler;

    protected readonly static Vector3 LEFT_ROT_VECTOR = new(0f, 180f, 0f);
    protected readonly static Vector3 RIGHT_ROT_VECTOR = new(0f, 0f, 0f);
    public UI_WSMonsterHpBar HealthBar { get; protected set; }
    public Transform PlayerTransform { get; protected set; }
    public EMonsterNames EMonsterType { get; protected set; }
    public MonsterStat Stat { get; protected set; }

    protected PlayerController _pc;
    protected Coroutine _parallysisCoroutineOrNull = null;
    protected MonsterHitFlasher _hitFlasher;

    UI_WSMonsterPopupDamageController _damageTextController;
    HitParticleController _hitParticleController;
    MonsterHitAnimController _hitAnimController;

    public override void Init()
    {
        if (HealthBar == null)
        {
            base.Init();
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _pc = PlayerTransform.gameObject.GetComponent<PlayerController>();
            Stat = gameObject.GetOrAddComponent<MonsterStat>();
            HealthBar = Utill.GetComponentInChildrenOrNull<UI_WSMonsterHpBar>(gameObject, "UIWSMonsterHpBar");
            _hitFlasher = Utill.GetFirstComponentInChildrenOrNull<MonsterHitFlasher>(gameObject);
            _damageTextController = Utill.GetFirstComponentInChildrenOrNull<UI_WSMonsterPopupDamageController>(gameObject);
            _hitParticleController = Utill.GetFirstComponentInChildrenOrNull<HitParticleController>(gameObject);
            _hitAnimController = Utill.GetFirstComponentInChildrenOrNull<MonsterHitAnimController>(gameObject);
        }
        InitStat();
    }

    public abstract void InitStat();
    public abstract void DamagedFromPlayer(ECharacterLookDir attackerDir, int damage, EPlayerNoramlAttackType eAttackType);
    public abstract void OnPlayerBlockSuccess();
    public abstract void OnHittedByPlayerSkill(data.SkillInfo skillInfo);

    protected abstract void SetLightControllersTurnOffTimeInSec();
    protected void DecreasHpAndInvokeHitEvents(int damage, EPlayerNoramlAttackType eAttackType)
    {
        int beforeDamageHP;
        int AfterDamageHP;
        int actualDamage = Stat.DecreaseHpAndGetActualDamageAmount(damage, out beforeDamageHP, out AfterDamageHP);

        #region Visual
        HealthBar.DecraseHP(beforeDamageHP, AfterDamageHP);
        _damageTextController.ShowPopup(actualDamage);
        _hitFlasher.StartDamageFlash();
        _hitAnimController.PlayHitEffect(transform.position, eAttackType);
        if (eAttackType == EPlayerNoramlAttackType.Attack_3 || eAttackType == EPlayerNoramlAttackType.BackAttack)
        {
            if (BigAttackEventHandler != null)
                BigAttackEventHandler.Invoke();
            _hitParticleController.PlayBigAttackParticle();
        }
        HittedByNormalAttackNoArgsEventHandler?.Invoke();
        HittedByNormalAttackEffectEventHandler?.Invoke(eAttackType);
        #endregion
    }
    protected void AddKnockbackForce(Vector2 force)
    {
        if (_pc == null)
        {
            _pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            PlayerTransform = _pc.transform;
        }
        Vector2 dir = PlayerTransform.position - transform.position;
        if (dir.x > 0)
            RigidBody.AddForce(new Vector2(-force.x, force.y), ForceMode2D.Impulse);
        else
            RigidBody.AddForce(force, ForceMode2D.Impulse);
    }

}
