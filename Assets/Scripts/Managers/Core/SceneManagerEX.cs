using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX 
{
    public define.ESceneType ECurrentScene { get; private set; }

    public void Init()
    {
        ECurrentScene = define.ESceneType.MAIN_MENU;
    }
    public void LoadScene(define.ESceneType eType)
    {
        GameObject.Find("@Scene").GetComponent<BaseScene>().Clear();
        ECurrentScene = eType;
        SceneManager.LoadScene((int)eType);
    }

    public define.ESceneType GetCurrentScene()
    {
        define.ESceneType retType = define.ESceneType.COUNT;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case (int)define.ESceneType.MAIN_MENU:
                retType = define.ESceneType.MAIN_MENU;
                break;
            case (int)define.ESceneType.TUTORIAL:
                retType = define.ESceneType.TUTORIAL;
                break;
            case (int)define.ESceneType.MAIN_PLAY:
                retType = define.ESceneType.MAIN_PLAY;
                break;
            default:
                Debug.Assert(false);
                break;
        }
        Debug.Assert(retType != define.ESceneType.COUNT);
        return retType;
    }

}
