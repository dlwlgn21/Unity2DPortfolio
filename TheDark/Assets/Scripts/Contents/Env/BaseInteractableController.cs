using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractableController : MonoBehaviour, IZonePlayerDetetable
{
    protected InteractKey _interactKey;

    protected void Init()
    {
        _interactKey = Utill.GetComponentInChildrenOrNull<InteractKey>(gameObject, "InteractEKey");
        Debug.Assert(_interactKey != null);
    }
    public abstract void OnPlayerEnter(Collider2D collision);

    public abstract void OnPlayerExit(Collider2D collision);

    public abstract void OnPlayerStay(Collider2D collision);
}
