using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public abstract class InteractBox : MonoBehaviour
{
    protected DoorController _parent;
    protected BoxCollider2D _boxCollider;
    private void Start()
    {
        _parent = gameObject.transform.parent.gameObject.GetComponent<DoorController>();
        Debug.Assert(_parent != null);
        _boxCollider = GetComponent<BoxCollider2D>();
    }

}
