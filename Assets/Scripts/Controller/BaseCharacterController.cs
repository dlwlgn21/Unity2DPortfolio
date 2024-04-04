using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour
{
    public Animator Animator { get; set; }
    public Animator HitEffectAniamtor { get; set; }

    public Rigidbody2D RigidBody { get; set; }
    public ECharacterLookDir ELookDir { get; protected set; }
    public Transform NormalAttackPoint { get; protected set; }

    public SpriteRenderer SpriteRenderer { get; set; }
    public float NormalAttackRange { get; protected set; }

    protected TextMesh mTextMesh;
    protected UIHealthBar mHealthBar;

    protected Vector3 mCachedAttackPointLocalRightPos;
    protected Vector3 mCachedAttackPointLocalLeftPos;
    

    protected abstract void initStates();

    public static string HIT_EFFECT_1_KEY = "Hit1";
    public static string HIT_EFFECT_2_KEY = "Hit2";
    public static string HIT_EFFECT_3_KEY = "Hit3";

    void Start()
    {
        Init();
        initStates();
    }
    public virtual void Init()
    {
        RigidBody = gameObject.GetOrAddComponent<Rigidbody2D>();
        Animator = gameObject.GetOrAddComponent<Animator>();
        SpriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();
        NormalAttackPoint = transform.Find("NormalAttackPoint").gameObject.transform;
        Debug.Assert(NormalAttackPoint != null);
        mCachedAttackPointLocalRightPos = NormalAttackPoint.localPosition;
        Vector3 leftPos = NormalAttackPoint.localPosition;
        leftPos.x = -leftPos.x;
        mCachedAttackPointLocalLeftPos = leftPos;

        foreach (Animator aniamtor in gameObject.GetComponentsInChildren<Animator>())
        {
            if (aniamtor != null && aniamtor.gameObject.name != gameObject.name)
            {
                HitEffectAniamtor = aniamtor;
                break;
            }
        }
        Debug.Assert(HitEffectAniamtor != null);
        HitEffectAniamtor.gameObject.SetActive(false);

        mTextMesh = Utill.GetComponentInChildrenOrNull<TextMesh>(gameObject, "DamagePopup");
        Debug.Assert(mTextMesh != null);
    }

    void OnDrawGizmosSelected()
    {
        if (NormalAttackPoint == null)
            return;
        Gizmos.DrawWireSphere(NormalAttackPoint.position, 1f);
    }

    public void ShowDamagePopup(int damage)
    {
        mTextMesh.text = damage.ToString();
        mTextMesh.gameObject.GetComponent<Animator>().Play("DamagePopup", -1, 0f);
        Debug.Assert(mHealthBar != null);
        mTextMesh.color = mHealthBar.HealthBar.color;
    }
}
