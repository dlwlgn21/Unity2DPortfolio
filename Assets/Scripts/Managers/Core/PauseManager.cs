using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager 
{
    public bool IsPaused { get; private set; } = false;
    
    public void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }
}
