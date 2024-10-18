using UnityEngine;
using UnityEngine.Events;

public class InteractBox : MonoBehaviour
{
    static public UnityAction<GameObject> PlayerEnterEventHandler;
    [SerializeField] protected GameObject _iZonePlayerDetectable;
    protected IZonePlayerDetetable _detectable;
    protected BoxCollider2D _boxCollider;

    private void Start()
    {
        _detectable = _iZonePlayerDetectable.GetComponent<IZonePlayerDetetable>();
        Debug.Assert(_detectable != null);
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            Debug.Assert(_iZonePlayerDetectable != null);
            _detectable.OnPlayerEnter(collision);
            if (PlayerEnterEventHandler != null)
                PlayerEnterEventHandler.Invoke(_iZonePlayerDetectable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            _detectable.OnPlayerExit(collision);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            _detectable.OnPlayerStay(collision);
        }
    }


    public void ActiveBox()
    {
        _boxCollider.enabled = true;
    }
    public void Unactive()
    {
        _boxCollider.enabled = false;
        gameObject.SetActive(false);
    }
}
