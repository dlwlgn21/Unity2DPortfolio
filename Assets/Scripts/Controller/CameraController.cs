using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _margin;
    
    //Transform mPlayer;
    float mKeepDepth;

    void Start()
    {
        //GameObject go = GameObject.FindGameObjectWithTag("Player");
        //Debug.Assert(go != null, "There is No Player");
        //mPlayer = go.transform;
        //_margin = 0f;
        mKeepDepth = Camera.main.transform.position.z;
    }


    private void LateUpdate()
    {
        //Vector3 pos = mPlayer.position;
        //pos.x -= _margin;
        //pos.y += _margin;
        //pos.z = mKeepDepth;

        Vector3 pos = Camera.main.transform.position;
        pos.z = mKeepDepth;
        Camera.main.transform.position = pos;
    }
}
