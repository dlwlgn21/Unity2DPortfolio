using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInteractBox : InteractBox
{
    public bool IsPressEKey { get; private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER)
            _parent.OnPlayerEnterInteractBox();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER)
            _parent.OnPlayerExitInteractBox();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E))
            {
                IsPressEKey = true;
                _boxCollider.enabled = false;
            }
        }
    }
}
