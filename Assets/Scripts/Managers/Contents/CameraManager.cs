using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class CameraManager 
{
    

    private CinemachineVirtualCamera _currSceneMainVirtualCam;
    //private GameObject _camZoomImg;
    private float _initialOrthographicSize;
    private float _zoomInSize;
    private const float ZOOM_TIME = 1f;
    private bool _isWorking = false;

    //private PlayerController _pc;
    //private Transform _topImgTramsform;
    //private Transform _botImgTramsform;
    //private Vector3 _topImgOriginalPos;
    //private Vector3 _botImgOriginalPos;

    private Ease _ease;
    public void Init()
    {
        if (_currSceneMainVirtualCam == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("MainVirtualCam");
            _currSceneMainVirtualCam = go.GetComponent<CinemachineVirtualCamera>();
            _initialOrthographicSize = _currSceneMainVirtualCam.m_Lens.OrthographicSize;
            _zoomInSize = _initialOrthographicSize - 1f;
            //go = Managers.Resources.Load<GameObject>("Prefabs/CamZoomImg");
            //_camZoomImg = Object.Instantiate(go);
            //Object.DontDestroyOnLoad(_camZoomImg);
            //_camZoomImg.SetActive(false);
            //_topImgTramsform = Utill.GetComponentInChildrenOrNull<Transform>(_camZoomImg, "TopImg");
            //_botImgTramsform = Utill.GetComponentInChildrenOrNull<Transform>(_camZoomImg, "BottomImg");
            //_ease = Ease.InFlash;
            //_camZoomImg.transform.SetParent(_currSceneMainVirtualCam.gameObject.transform);
        }
    }

    public void OnMonsterHittedByPlayerNormalAttack()
    {
        StartCamZoom();
    }

    private void StartCamZoom()
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
