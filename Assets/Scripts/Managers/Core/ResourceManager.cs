using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceManager
{

    public T Instantiate<T>(string path) where T :Object
    {
        T obj = Load<T>(path);
        Debug.Assert(obj != null);
        if (obj == null)
        {
            Debug.DebugBreak();
        }
        return Object.Instantiate(obj, null);
    }
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
