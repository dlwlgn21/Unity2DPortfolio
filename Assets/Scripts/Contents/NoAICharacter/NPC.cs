using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] SpriteRenderer _interactSprite;
    protected Transform mPlayerTransform;
    protected const float INTERACT_DIST = 1.5f;
    protected const float SCALE_TWEEN_TIME = 0.3f;
    Transform mArrowTransform;


    bool mIsEnterTweening;
    bool mIsExitTweening;
    void Awake()
    {
        mPlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mArrowTransform = Utill.GetComponentInChildrenOrNull<Transform>(gameObject, "DownArrow");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsWithinInteractDistance())
            Interact();

        if (_interactSprite.gameObject.activeSelf && !IsWithinInteractDistance())
        {
            // Turn off sprite
            mArrowTransform.DOScale(0f, SCALE_TWEEN_TIME).SetEase(Ease.OutElastic).OnComplete(OnScaleTweenEnded);
        }
        else if (!_interactSprite.gameObject.activeSelf && IsWithinInteractDistance())
        {
            // Turn on sprite
            _interactSprite.gameObject.SetActive(true);
            mArrowTransform.DOScale(0.5f, SCALE_TWEEN_TIME).SetEase(Ease.InElastic);
        }
    }
    public abstract void Interact();
    public void OnScaleTweenEnded()
    {
        _interactSprite.gameObject.SetActive(false);
    }
    protected bool IsWithinInteractDistance()
    {
        if (Vector2.Distance(mPlayerTransform.position, transform.position) < INTERACT_DIST)
            return true;
        return false;
    }
}
