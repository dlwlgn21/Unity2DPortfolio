using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utill
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T GetComponentInChildrenOrNull<T>(GameObject go, string gameObjectName) where T : UnityEngine.Component
    {
        foreach (var component in go.GetComponentsInChildren<T>())
        {
            if (component != null && component.gameObject.name == gameObjectName)
                return component;
        }
        return null;
    }

    public static T GetFirstComponentInChildrenOrNull<T>(GameObject go) where T : UnityEngine.Component
    {
        foreach (var component in go.GetComponentsInChildren<T>())
        {
            if (component != null)
                return component;
        }
        return null;
    }

}
