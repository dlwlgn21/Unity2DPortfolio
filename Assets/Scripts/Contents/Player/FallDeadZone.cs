using UnityEngine;
using UnityEngine.Events;

public class FallDeadZone : MonoBehaviour
{
    static public UnityAction PlayerFallDeadZoneEventHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PlayerBody)
        {
            PlayerFallDeadZoneEventHandler?.Invoke();
        }
        else if (collision.gameObject.layer == (int)define.EColliderLayer.MonsterBody)
        {
            NormalMonsterController mc = collision.gameObject.GetComponent<NormalMonsterController>();
            if (mc != null)
                mc.ChangeState(ENormalMonsterState.Die);
        }
    }

    private void OnDestroy()
    {
        PlayerFallDeadZoneEventHandler = null;
    }
}
