using UnityEngine;

public class InteractBox : MonoBehaviour
{
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
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            _detectable.OnPlayerEnter(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            _detectable.OnPlayerExit(collision);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            _detectable.OnPlayerStay(collision);
        }
    }


    public void ActiveBox()
    {
        _boxCollider.enabled = true;
    }
    public void UnactiveBox()
    {
        _boxCollider.enabled = false;
    }
}
