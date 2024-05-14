using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

public abstract class BaseCharacterController : MonoBehaviour
{
    public Animator Animator { get; set; }
    public Animator HitEffectAniamtor { get; set; }
    public Rigidbody2D RigidBody { get; set; }
    public ECharacterLookDir ELookDir { get; set; }
    public Transform NormalAttackPoint { get; protected set; }
    public ParticleSystem FootDustParticle { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public float NormalAttackRange { get; protected set; }
    public UIHealthBar HealthBar { get; set; }
    public UITextPopup DamageText { get; set; }
    public UITextPopup StatusText { get; set; }

    public GameObject HeadLight { get; set; }
    public GameObject AttackLight { get; set; }

    public Vector3 CachedAttackPointLocalRightPos { get; set; }
    public Vector3 CachedAttackPointLocalLeftPos { get; set; }
    public Vector2 OriginalAttackLightLocalPos { get; private set; }
    protected abstract void InitStates();

    public static string HIT_EFFECT_1_KEY = "Hit1";
    public static string HIT_EFFECT_2_KEY = "Hit2";
    public static string HIT_EFFECT_3_KEY = "Hit3";

    private void Awake()
    {
        Init();
        InitStates();
    }

    public virtual void Init()
    {
        RigidBody = gameObject.GetOrAddComponent<Rigidbody2D>();
        Animator = gameObject.GetOrAddComponent<Animator>();
        SpriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();
        NormalAttackPoint = transform.Find("NormalAttackPoint").gameObject.transform;
        Debug.Assert(NormalAttackPoint != null);

        // WorldSpaceUI 와 연동위해 다시 추가. 
        CachedAttackPointLocalRightPos = NormalAttackPoint.localPosition;
        Vector3 leftPos = NormalAttackPoint.localPosition;
        leftPos.x = -leftPos.x;
        CachedAttackPointLocalLeftPos = leftPos;



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

        DamageText = Utill.GetComponentInChildrenOrNull<UITextPopup>(gameObject, "DamagePopup");
        Debug.Assert(DamageText != null);
        StatusText = Utill.GetComponentInChildrenOrNull<UITextPopup>(gameObject, "StatusPopup");
        Debug.Assert(StatusText != null);
        
        FootDustParticle = Utill.GetComponentInChildrenOrNull<ParticleSystem>(gameObject, "FootDustParticle");
        Debug.Assert(FootDustParticle != null);
        AttackLight = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "AttackLight").gameObject;
        Debug.Assert(AttackLight != null);
        OriginalAttackLightLocalPos = AttackLight.transform.localPosition;
        AttackLight.SetActive(false);

        HeadLight = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "HeadLight").gameObject;
        Debug.Assert(HeadLight != null);
    }

    public void RotateAttackLightAccodingCharacterLookDir()
    {
        if (ELookDir == ECharacterLookDir.RIGHT)
        {
            AttackLight.transform.localRotation = Quaternion.Euler(Vector3.zero);
            AttackLight.transform.localPosition = OriginalAttackLightLocalPos;
        }
        else
        {
            AttackLight.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            AttackLight.transform.localPosition = new Vector2(-OriginalAttackLightLocalPos.x, OriginalAttackLightLocalPos.y);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (NormalAttackPoint == null)
            return;
        Gizmos.DrawWireSphere(NormalAttackPoint.position, 1f);
    }
}
