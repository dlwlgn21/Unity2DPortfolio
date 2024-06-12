using UnityEngine;

public class UIWSMonsterHpBar : UIHealthBar
{
    [SerializeField] [Range(-10f, 10f)] private float _yOffset;
    private Transform _parentTransform;
    private float _yMargin;
    private Vector3 _originalLocalScale;
    private void Start()
    {
        SetFullHpBarRatio();
        _yOffset = -0.4f;
        _originalLocalScale = transform.localScale;
    }
    public override void Init()
    {
        if (_parentTransform == null)
        {
            _parentTransform = transform.parent;
            _yMargin = transform.parent.GetComponent<Collider2D>().bounds.size.y + _yOffset;
        }
        SetFullHpBarRatio();
    }

    private void Update()
    {
        if (_parentTransform.localRotation.eulerAngles.y > 0f)
        {
            transform.localScale = new Vector3(-1f, _originalLocalScale.y, _originalLocalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1f, _originalLocalScale.y, _originalLocalScale.z);
        }
        transform.position = _parentTransform.position + Vector3.up * (_yMargin + _yOffset);
    }
}
