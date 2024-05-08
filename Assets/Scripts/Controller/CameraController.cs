using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _margin;
    
    Transform _player;
    float _keepDepth;

    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(go != null, "There is No Player");
        _player = go.transform;
        //_margin = 0f;
        _keepDepth = -10;
    }


    private void LateUpdate()
    {
        Vector3 pos = _player.position;
        //pos.x -= _margin;
        //pos.y += _margin;
        pos.z = _keepDepth;

        //Vector3 pos = Camera.main.transform.position;
        //pos.z = mKeepDepth;
        gameObject.transform.position = pos;
    }
}
