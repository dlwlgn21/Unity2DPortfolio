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
    public ParticleSystem FootDustParticle { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public UITextPopup DamageText { get; set; }
    public UITextPopup StatusText { get; set; }

    public GameObject HeadLight { get; set; }

    public AttackLightController AttackLightController { get; private set; }

    private GameObject _attackLight;

    public Vector2 OriginalAttackLightLocalPos { get; private set; }
    protected abstract void InitStates();

    public readonly static string HIT_EFFECT_1_KEY = "Hit1";
    public readonly static string HIT_EFFECT_2_KEY = "Hit2";
    public readonly static string HIT_EFFECT_3_KEY = "Hit3";

    private void Awake()
    {
        Init();
        InitStates();
    }

    public virtual void Init()
    {
        if (RigidBody == null)
        {
            RigidBody = gameObject.GetOrAddComponent<Rigidbody2D>();
            Animator = gameObject.GetOrAddComponent<Animator>();
            SpriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();

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

            // AttackLight
            _attackLight = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "AttackLight").gameObject;
            Debug.Assert(_attackLight != null);
            OriginalAttackLightLocalPos = _attackLight.transform.localPosition;
            AttackLightController = _attackLight.GetComponent<AttackLightController>();
            Debug.Assert(AttackLightController != null);
            AttackLightController.Init();

            HeadLight = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "HeadLight").gameObject;
            Debug.Assert(HeadLight != null);
        }
    }

    void OnDrawGizmosSelected()
    {

    }
}
