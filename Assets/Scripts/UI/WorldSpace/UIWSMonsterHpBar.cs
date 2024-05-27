using UnityEngine;

public class UIWSMonsterHpBar : UIHealthBar
{
    [SerializeField] [Range(0.2f, 0.5f)] private float _yOffset;
    private Transform _parentTransform;
    private float _yMargin;

    protected override void Init()
    {
        _parentTransform = transform.parent;
        _yMargin = transform.parent.GetComponent<Collider2D>().bounds.size.y + _yOffset;
        SetFullHpBarRatio();
    }

    private void Update()
    {
        transform.position = _parentTransform.position + Vector3.up * (_yMargin + _yOffset);
    }
}
