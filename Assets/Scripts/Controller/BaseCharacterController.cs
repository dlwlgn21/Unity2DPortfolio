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
    public Rigidbody2D RigidBody { get; set; }
    public ECharacterLookDir ELookDir { get; set; }
    public ParticleSystem FootDustParticle { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    protected abstract void InitStates();

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
            FootDustParticle = Utill.GetComponentInChildrenOrNull<ParticleSystem>(gameObject, "FootDustParticle");
            Debug.Assert(FootDustParticle != null);
        }
    }

    void OnDrawGizmosSelected()
    {

    }
}
