using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
    public Action GameEventHandler;

    public void Invoke()
    {
        if (GameEventHandler != null)
            GameEventHandler.Invoke();
    }
}
