using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Camera _cam;
    Vector2 _startPos;
    float _startZ;

    [SerializeField] private float _parallaxFactor;
    Vector2 _travel => (Vector2)_cam.transform.position - _startPos;

    void Start()
    {
        _startPos = transform.position;
        _startZ = transform.position.z;
    }

    void Update()
    {
        Vector2 newPos = _startPos + _travel * _parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, _startZ);
    }

}
