using UnityEngine;
using UnityEngine.Events;

public class PlayerFallDeadZone : MonoBehaviour
{
    static public UnityAction PlayerFallDeadZoneEventHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            PlayerFallDeadZoneEventHandler?.Invoke();
        }
    }

    private void OnDestroy()
    {
        PlayerFallDeadZoneEventHandler = null;
    }
}
