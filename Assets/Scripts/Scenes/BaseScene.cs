using define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public define.ESceneType ESceneType { get; protected set; }

    public BaseScene(ESceneType eSceneType)
    {
        ESceneType = eSceneType;
    }


    protected virtual void Init()
    {
        GameObject eventSys = GameObject.Find("EventSystem");
        if (eventSys == null)
        {
            eventSys = Managers.Resources.Load<GameObject>("Prefabs/EventSystem");
            Object.Instantiate(eventSys).name = "@EventSystem";
        }
    }

    public abstract void Clear();
}
