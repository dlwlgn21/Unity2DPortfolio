using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public abstract class ItemController : MonoBehaviour
{
    [SerializeField] GameObject _btnSprite;
    [SerializeField] public define.EItemType _eItemType;
    [SerializeField] protected int _id;
    ParticleSystem _particle;
    private void Start()
    {
        Debug.Assert(_btnSprite != null);
        _particle = Utill.GetFirstComponentInChildrenOrNull<ParticleSystem>(gameObject);
        Debug.Assert(_particle != null);
        _btnSprite.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _btnSprite.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.D))
            {
                _particle.Play();
                GetComponent<BoxCollider2D>().enabled = false;
                _btnSprite.SetActive(false);
                PushItem();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _btnSprite.SetActive(false);
        }
    }

    public abstract void PushItem();
}
