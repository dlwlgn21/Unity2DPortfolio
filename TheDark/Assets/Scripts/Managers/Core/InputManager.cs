using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using define;

public class InputManager 
{
    public Action KeyboardHandler = null;

    public void OnUpdate()
    {
        if (Input.anyKeyDown && KeyboardHandler != null)
            KeyboardHandler.Invoke();
    }

    public void Clear()
    {
        KeyboardHandler = null;
    }
}
