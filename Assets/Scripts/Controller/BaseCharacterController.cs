using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour
{
    public Animator Animator { get; set; }
    public Rigidbody2D RigidBody { get; set; }
    public ECharacterLookDir ELookDir { get; protected set; }
    public Transform NormalAttackPoint { get; protected set; }

    public float NormalAttackRange { get; protected set; }
    protected SpriteRenderer mSpriteRenderer;

    protected Vector3 mCachedAttackPointLocalRightPos;
    protected Vector3 mCachedAttackPointLocalLeftPos;

    protected abstract void initStates();


    void Start()
    {
        Init();
        initStates();
    }
    public virtual void Init()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        NormalAttackPoint = transform.Find("NormalAttackPoint").gameObject.transform;
        Debug.Assert(NormalAttackPoint != null);
        mCachedAttackPointLocalRightPos = NormalAttackPoint.localPosition;
        Vector3 leftPos = NormalAttackPoint.localPosition;
        leftPos.x = -leftPos.x;
        mCachedAttackPointLocalLeftPos = leftPos;
    }

    void OnDrawGizmosSelected()
    {
        if (NormalAttackPoint == null)
            return;
        Gizmos.DrawWireSphere(NormalAttackPoint.position, 1f);
    }
}
