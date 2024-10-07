using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using define;
public abstract class ItemController : MonoBehaviour
{
    [SerializeField] GameObject _btnSprite;
    [SerializeField] public define.EItemType _eItemType;
    [SerializeField] protected int _id;
    protected ItemInfo _itemInfo;
    private void Start()
    {
        Debug.Assert(_btnSprite != null);
        _btnSprite.SetActive(false);
        SetItemInfo();
        Sprite sprite = null;
        switch (_itemInfo.EItemType)
        {
            case EItemType.Equippable:
                sprite = Managers.UI.GetEquipableItemSprite(_itemInfo);
                break;
            case EItemType.Consumable:
                sprite = Managers.UI.GetConsumableItemSprite(_itemInfo);
                break;
            default:
                Debug.Assert(false);
                break;
        }
        Debug.Assert(sprite != null);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _btnSprite.SetActive(true);
            transform.DOPunchPosition(Vector3.up * 0.1f, 1f);
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
                transform.DOScale(new Vector3(0f, 0f, transform.localScale.z), 1f).SetEase(Ease.InOutElastic).OnComplete(OnScaleZeroTweenEnded);
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
    protected abstract void SetItemInfo();
    void OnScaleZeroTweenEnded()
    {
        // TODO : Pooling ÇØ¾ßÇÔ.
        Destroy(gameObject);
    }
}
