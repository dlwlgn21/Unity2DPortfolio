using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ETutorialZone
{
    ATTACK,
    ROLL,
    BACK_ATTACK,
    BLOCK
}

public class TutorialCamSwitchZone : MonoBehaviour
{
    [SerializeField] private ETutorialZone _eZoneType;
    private TutorialCameraManager _camManager;
    private void Start()
    {
        _camManager = GameObject.FindGameObjectWithTag("CamManager").GetComponent<TutorialCameraManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (_eZoneType)
            {
                case ETutorialZone.ATTACK:
                    _camManager.SwitchCameraToRoom(TutorialCameraManager.ATTACK_TUTORIAL_X_POS);
                    break;
                case ETutorialZone.ROLL:
                    _camManager.SwitchCameraToRoom(TutorialCameraManager.ROLL_TUTORIAL_X_POS);
                    break;
                case ETutorialZone.BACK_ATTACK:
                    _camManager.SwitchCameraToRoom(TutorialCameraManager.BACK_ATTACK_TUTORIAL_X_POS);
                    break;
                case ETutorialZone.BLOCK:
                    _camManager.SwitchCameraToRoom(TutorialCameraManager.BLOCK_TUTORIAL_X_POS);
                    break;
            }
            gameObject.SetActive(false);
        }
    }
}
