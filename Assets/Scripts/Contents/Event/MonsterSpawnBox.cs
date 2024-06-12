using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnBox : MonoBehaviour
{
    enum EPortalState
    { 
        IDLE,
        OPEN,
        STAY,
        CLOSE,
        COUNT
    }

    [SerializeField] private define.EMonsterNames _eMonsterType;
    private Animator _animator;
    private const string IDLE_ANIM_KEY = "GreenMonsterProtalIdle";
    private const string OPEN_ANIM_KEY = "GreenMonsterProtalOpen";
    private const string STAY_ANIM_KEY = "GreenMonsterProtalStay";
    private const string CLOSE_ANIM_KEY = "GreenMonsterProtalClose";
    private void Start() { _animator = GetComponent<Animator>(); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayAnimation(EPortalState.OPEN);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void PlayAnimation(EPortalState eState)
    {
        switch (eState)
        {
            case EPortalState.IDLE:
                break;
            case EPortalState.OPEN:
                _animator.Play(OPEN_ANIM_KEY, -1, 0f);
                break;
            case EPortalState.STAY:
                _animator.Play(STAY_ANIM_KEY, -1, 0f);
                break;
            case EPortalState.CLOSE:
                _animator.Play(CLOSE_ANIM_KEY, -1, 0f);
                break;
        }
    }
    public void OnOpenAnimFullyPlayed()
    {
        PlayAnimation(EPortalState.STAY);
    }

    public void OnValidSpawnTiming()
    {
        Managers.MonsterPool.Get(_eMonsterType, gameObject.transform.position);
    }

    public void OnStayAnimFullyPlayed()
    {
        PlayAnimation(EPortalState.CLOSE);
    }

    public void OnCloseAnimFullyPlayed()
    {
        gameObject.SetActive(false);
    }
}
