using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZonePlayerDetetable
{
    public void OnPlayerEnter(Collider2D collision);
    public void OnPlayerStay(Collider2D collision);
    public void OnPlayerExit(Collider2D collision);
}
