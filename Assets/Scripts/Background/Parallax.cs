using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform _camTransform;
    [SerializeField] private float _parallaxEffect;
    private float _xLength;
    private float _xStartPos;

    private void Start()
    {
        _xStartPos = transform.position.x;
        _xLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float tmp = (_camTransform.position.x * (1 - _parallaxEffect));
        float dist = (_camTransform.position.x * _parallaxEffect);
        Vector3 pos = transform.position;
        transform.position = new Vector3(_xStartPos + dist, 3f, pos.z);
        if (tmp > _xStartPos + _xLength)
            _xStartPos += _xLength;
        else if (tmp < _xStartPos - _xLength)
            _xStartPos -= _xLength;
    }

}
