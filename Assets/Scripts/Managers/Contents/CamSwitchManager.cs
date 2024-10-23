
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public sealed class CamSwitchManager 
{
    private CinemachineVirtualCamera _mainCam;
    private CinemachineVirtualCamera _roomCam;
    public static float ATTACK_TUTORIAL_X_POS = 18f;
    public static float ROLL_TUTORIAL_X_POS = 35f;
    public static float BACK_ATTACK_TUTORIAL_X_POS = 51f;
    public static float BLOCK_TUTORIAL_X_POS = 67f;
    public void Init()
    {
        _mainCam = GameObject.Find("MainVCAM").GetComponent<CinemachineVirtualCamera>();
        _roomCam = GameObject.Find("RoomVCAM")?.GetComponent<CinemachineVirtualCamera>();
        _mainCam.Follow = Managers.Cam.CamFollow.transform;
        Debug.Assert(_mainCam != null);
    }

    public void SwitchCameraToMain()
    {
        _mainCam.enabled = true;
        _roomCam.enabled = false;
    }
    public void SwitchCameraToRoom(float xPos)
    {
        Debug.Assert(_roomCam != null);
        Vector3 pos = _roomCam.gameObject.transform.position;
        _roomCam.gameObject.transform.position = new Vector3(xPos, pos.y, pos.z);
        SwitchCameraToRoom();
    }
    public void SwitchCameraToRoom()
    {
        Debug.Assert(_roomCam != null);
        _mainCam.enabled = false;
        _roomCam.enabled = true;
    }

    public void Clear()
    {
        _mainCam = null;
        _roomCam = null;
    }

}
