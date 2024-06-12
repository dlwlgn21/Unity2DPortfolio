using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager 
{
    private CinemachineVirtualCamera _currSceneMainVirtualCam;
    private float _initialOrthographicSize;
    private float _zoomInSize;
    private const float ZOOM_TIME = 1f;
    private bool _isWorking = false;
    public void Init()
    {
        GameObject go = GameObject.FindGameObjectWithTag("MainVirtualCam");
        _currSceneMainVirtualCam = go.GetComponent<CinemachineVirtualCamera>();
        _initialOrthographicSize = _currSceneMainVirtualCam.m_Lens.OrthographicSize;
        _zoomInSize = _initialOrthographicSize - 0.5f;
    }


    public void StartCamZoom()
    {
        if (_isWorking)
        {
            return;
        }
        _currSceneMainVirtualCam.StartCoroutine(StartCamZoomCo());
    }

    IEnumerator StartCamZoomCo()
    {
        _isWorking = true;
        _currSceneMainVirtualCam.m_Lens.OrthographicSize = _zoomInSize;
        float elapsedTime = 0f;
        while (elapsedTime < ZOOM_TIME)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _currSceneMainVirtualCam.m_Lens.OrthographicSize = _initialOrthographicSize;
        _isWorking = false;
    }
}
