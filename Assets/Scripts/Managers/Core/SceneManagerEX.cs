using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        Managers.FullScreenEffect.StartFullScreenEffect(EFullScreenEffectType.SCENE_TRANSITION);
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
            case (int)define.ESceneType.ABANDON_ROAD_SCENE:
                retType = define.ESceneType.ABANDON_ROAD_SCENE;
                break;
            default:
                Debug.Assert(false);
                break;
        }
        Debug.Assert(retType != define.ESceneType.COUNT);
        return retType;
    }
}
