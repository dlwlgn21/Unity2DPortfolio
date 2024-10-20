using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CamLookUpDownZoneDetectionController : MonoBehaviour
{
    static public UnityAction PlayerEnterEventHandler;
    static public UnityAction PlayerExitEventHandler;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            if (PlayerEnterEventHandler != null)
                PlayerEnterEventHandler.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            if (PlayerExitEventHandler != null)
                PlayerExitEventHandler.Invoke();
        }
    }

}
