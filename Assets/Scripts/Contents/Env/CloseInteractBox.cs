using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseInteractBox : InteractBox
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)define.EColliderLayer.PLAYER_BODY)
        {
            _parent.OnPlayerEnterCloseInteractBox();
            _boxCollider.enabled = false;
        }
    }
}
