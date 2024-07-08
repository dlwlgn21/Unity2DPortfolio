using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneManagerEX 
{
    public define.ESceneType ECurrentScene { get; private set; }
    private GameObject _fadeout;
    private Image _fadeImg;
    private const float FADE_OUT_TIME = 3f;
    public void Init()
    {
        ECurrentScene = define.ESceneType.MAIN_MENU;
        if (_fadeout == null)
        {
            GameObject fadeOut = Managers.Resources.Load<GameObject>("Prefabs/UI/UIFadeOut");
            Debug.Assert(fadeOut != null);
            _fadeout = Object.Instantiate(fadeOut);
            _fadeout.name = "UIFadeOut";
            Object.DontDestroyOnLoad(_fadeout);
            _fadeImg = _fadeout.GetComponentInChildren<Image>();
            _fadeout.SetActive(false);
        }
    }
    public void LoadScene(define.ESceneType eType)
    {
        GameObject.Find("@Scene").GetComponent<BaseScene>().Clear();
        ECurrentScene = eType;
        SceneManager.LoadScene((int)eType);
        _fadeout.SetActive(true);
        _fadeImg.DOFade(0f, FADE_OUT_TIME).SetEase(Ease.InOutExpo).OnComplete(OnFadeOutCompleted);
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
    public void OnFadeOutCompleted()
    {
        _fadeImg.color = Color.black;
        _fadeout.SetActive(false);
    }
}
