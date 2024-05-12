using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EPillarState
{ 
    IDLE,
    ACTIVATING,
    ACTIVATED,
    UNACTIVATING,
}

public class TutorialPillar : MonoBehaviour
{
    [SerializeField] Sprite _shutDownSprite;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private EPillarState _eCurrState;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _eCurrState = EPillarState.IDLE;
        Debug.Assert(_shutDownSprite !=  null);
    }

    public void PlayAnimation(EPillarState eState)
    {
        _eCurrState = eState;
        switch (_eCurrState)
        {
            case EPillarState.ACTIVATING:
                _animator.Play("TutorialPillarActive");
                break;
            case EPillarState.ACTIVATED:
                _animator.Play("TutorialPillarActivated");
                break;
            case EPillarState.UNACTIVATING:
                _animator.Play("TutorialPillarUnctivate");
                break;
        }
    }

    public void OnActivatingEnd()
    {
        PlayAnimation(EPillarState.ACTIVATED);
    }
    public void OnUnactivatingEnd()
    {
        ShutDown();
    }
    private void ShutDown()
    {
        _animator.StopPlayback();
        _animator.Rebind();
        _animator.enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        _spriteRenderer.sprite = _shutDownSprite;
    }

}
