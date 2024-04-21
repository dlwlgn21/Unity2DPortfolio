using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager
{
    bool mIsNewGameBtnClicked = false;

    GameObject mMainMenu;
    Button mNewGameBtn;
    Button mSettingBtn;
    Button mExitBtn;

    GameObject mFadeout;
    Image mFadeImg;
    public void Init()
    {
        GameObject mainMenu = Managers.Resources.Load<GameObject>("Prefabs/UI/UIMainMenu");
        Debug.Assert(mainMenu != null);
        mMainMenu = Object.Instantiate(mainMenu);
        Object.DontDestroyOnLoad(mMainMenu);
        mNewGameBtn = Utill.GetComponentInChildrenOrNull<Button>(mMainMenu, "UINewGameBtn");
        mSettingBtn = Utill.GetComponentInChildrenOrNull<Button>(mMainMenu, "UISettingBtn");
        mExitBtn = Utill.GetComponentInChildrenOrNull<Button>(mMainMenu, "UIExitBtn");
        mNewGameBtn.onClick.AddListener(OnNewGameBtnClicked);
        mSettingBtn.onClick.AddListener(OnSettingBtnClicked);
        mExitBtn.onClick.AddListener(OnExitBtnClicked);

        GameObject fadeOut = Managers.Resources.Load<GameObject>("Prefabs/UI/UIFadeOut");
        Debug.Assert(fadeOut != null);
        mFadeout = Object.Instantiate(fadeOut);
        Object.DontDestroyOnLoad(mFadeout);
        mFadeImg = mFadeout.GetComponentInChildren<Image>();
        mFadeout.SetActive(false);
        mMainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mNewGameBtn.gameObject);
    }
    public void OnNewGameBtnClicked()
    {
        mIsNewGameBtnClicked = true;
        mFadeout.SetActive(true);
        mFadeImg.DOFade(0, 2.0f).SetEase(Ease.InOutExpo).OnComplete(OnFadeOutCompleted);
        mMainMenu.SetActive(false);
        mIsNewGameBtnClicked = false;
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene((int)define.ESceneType.GAME_SCENE);
    }
    public void OnSettingBtnClicked()
    {
        if (mIsNewGameBtnClicked)
            return;
    }
    public void OnExitBtnClicked()
    {
        if (mIsNewGameBtnClicked)
            return;
        Application.Quit();
    }

    public void OnFadeOutCompleted()
    {
       mFadeImg.color = Color.black; 
       mFadeout.SetActive(false);
    }
}
