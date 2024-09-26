using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CamSwitchZone : MonoBehaviour
{
    [SerializeField] private float _xPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Managers.CamSwitch.SwitchCameraToRoom(_xPos);
            gameObject.SetActive(false);
        }
    }
}
