using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using define;
public abstract class ItemController : MonoBehaviour
{
    [SerializeField] GameObject _btnSprite;
    [SerializeField] public define.EItemType _eItemType;
    [SerializeField] protected int _id;
    private void Start()
    {
        Debug.Assert(_btnSprite != null);
        _btnSprite.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _btnSprite.SetActive(true);
            transform.DOScale(DoTweenValueContainer.TWEEN_SCALE_END_VALUE, DoTweenValueContainer.TWEEN_SCALE_END_TIME_IN_SEC).SetEase(Ease.InOutElastic);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.D))
            {
                GetComponent<BoxCollider2D>().enabled = false;
                _btnSprite.SetActive(false);
                PushItemToInventory();
                transform.DOScale(Vector3.zero, DoTweenValueContainer.TWEEN_SCALE_END_TIME_QUARTER_IN_SEC).SetEase(Ease.InOutElastic).OnComplete(OnScaleZeroTweenEnded);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _btnSprite.SetActive(false);
            transform.DOScale(Vector3.one, DoTweenValueContainer.TWEEN_SCALE_END_TIME_IN_SEC).SetEase(Ease.InOutElastic);
        }
    }

    public abstract void PushItemToInventory();

    void OnScaleZeroTweenEnded()
    {
        // TODO : Pooling ÇØ¾ßÇÔ.
        Destroy(gameObject);
    }
}
