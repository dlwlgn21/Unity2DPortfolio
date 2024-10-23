using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using define;

public sealed class InputManager 
{
    public Action KeyboardHandler = null;

    public void OnUpdate()
    {
        if (Input.anyKeyDown)
        {
            if (KeyboardHandler != null)
                KeyboardHandler.Invoke();
        }
    }

    public void Clear()
    {
        //KeyboardHandler = null;
    }
}
