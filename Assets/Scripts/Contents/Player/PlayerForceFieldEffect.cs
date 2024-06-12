using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerForceFieldEffect : MonoBehaviour
{
    public UnityAction ForceFieldStartEventHandler;
    public UnityAction ForceFieldEndEventHandler;
    private Animator _animator;

    ~PlayerForceFieldEffect()
    {
        ForceFieldStartEventHandler = null;
        ForceFieldEndEventHandler = null;
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void PlayForceFieldEffect()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        _animator.Play("ForceFieldEffect", -1, 0f);
    }

    public void OnForceFieldAnimStart()
    {
        ForceFieldStartEventHandler.Invoke();
    }
    public void OnForceFieldAnimFullyPlayed()
    {
        ForceFieldEndEventHandler.Invoke();
    }

}
