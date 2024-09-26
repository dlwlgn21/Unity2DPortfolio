using Cinemachine;
using UnityEngine;

public enum ECameraType
{ 
    MAIN_PLAY_CAM,
    ROOM_CAM
}
public class TutorialCameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _mainCam;
    [SerializeField] private CinemachineVirtualCamera _roomCam;
    public static float ATTACK_TUTORIAL_X_POS = 18f;
    public static float ROLL_TUTORIAL_X_POS = 35f;
    public static float BACK_ATTACK_TUTORIAL_X_POS = 51f;
    public static float BLOCK_TUTORIAL_X_POS = 67f;
    public void SwitchCameraToMain()
    {
        _mainCam.enabled = true;
        _roomCam.enabled = false;
    }
    public void SwitchCameraToRoom(float xPos)
    {
        Vector3 pos = _roomCam.gameObject.transform.position;
        _roomCam.gameObject.transform.position = new Vector3(xPos, pos.y, pos.z);
        _mainCam.enabled = false;
        _roomCam.enabled = true;
    }
}
