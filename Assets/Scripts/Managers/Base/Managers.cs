using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Managers : MonoBehaviour
{
    static Managers sInstance;
    public static Managers Instance { get { Init(); return sInstance; } }

    ResourceManager mResourceManager = new ResourceManager();
    InputManager mInputManager = new InputManager();
    DataManager mDataManager = new DataManager();
    public static InputManager Input { get { return Instance.mInputManager; } }
    public static DataManager Data { get { return Instance.mDataManager; } }
    public static ResourceManager Resources { get { return Instance.mResourceManager; } }
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Input.OnUpdate();
    }

    static void Init()
    {
        if (sInstance == null)
        {
            GameObject go = GameObject.Find("@GameManagers");
            if (go == null)
            {
                go = new GameObject { name = "@GameManagers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            sInstance = go.GetComponent<Managers>();
            sInstance.mDataManager.Init();
        }
    }

    public static void Clear()
    {
        Input.Clear();
    }
}
