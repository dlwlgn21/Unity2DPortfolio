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
        ECurrentScene = GetCurrentESceneType();
    }
    public void LoadScene(define.ESceneType eType)
    {
        GameObject.Find("@Scene").GetComponent<BaseScene>().Clear();
        ECurrentScene = eType;
        SceneManager.LoadScene((int)eType);
        Managers.FullScreenEffect.StartFullScreenEffect(EFullScreenEffectType.SCENE_TRANSITION);
    }

    define.ESceneType GetCurrentESceneType()
    {
        define.ESceneType retType = define.ESceneType.Count;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case (int)define.ESceneType.MainMenu:
                retType = define.ESceneType.MainMenu;
                break;
            case (int)define.ESceneType.Tutorial:
                retType = define.ESceneType.Tutorial;
                break;
            case (int)define.ESceneType.AbandonLoadScene:
                retType = define.ESceneType.AbandonLoadScene;
                break;
            case (int)define.ESceneType.ColossalBossCaveScene:
                retType = define.ESceneType.ColossalBossCaveScene;
                break;
            default:
                Debug.Assert(false);
                break;
        }
        Debug.Assert(retType != define.ESceneType.Count);
        return retType;
    }

    public void Clear()
    {
        GameObject.Find("@Scene").GetComponent<BaseScene>().Clear();
    }
}
