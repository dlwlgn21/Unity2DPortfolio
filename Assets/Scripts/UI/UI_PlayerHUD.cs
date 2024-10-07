using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UI_PlayerHUD : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
