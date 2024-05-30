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
    bool _isNewGameBtnClicked = false;

    GameObject _mainMenu;
    Button _newGameBtn;
    Button _settingBtn;
    Button _exitBtn;

    GameObject _fadeout;
    Image _fadeImg;
    public void Init()
    {
        if (_mainMenu == null)
        {
            GameObject mainMenu = Managers.Resources.Load<GameObject>("Prefabs/UI/UIMainMenu");
            Debug.Assert(mainMenu != null);
            _mainMenu = Object.Instantiate(mainMenu);
            _mainMenu.name = "UIMainMenu";
            _newGameBtn = Utill.GetComponentInChildrenOrNull<Button>(_mainMenu, "UINewGameBtn");
            _settingBtn = Utill.GetComponentInChildrenOrNull<Button>(_mainMenu, "UISettingBtn");
            _exitBtn = Utill.GetComponentInChildrenOrNull<Button>(_mainMenu, "UIExitBtn");
            _newGameBtn.onClick.AddListener(OnNewGameBtnClicked);
            _settingBtn.onClick.AddListener(OnSettingBtnClicked);
            _exitBtn.onClick.AddListener(OnExitBtnClicked);
            Object.DontDestroyOnLoad(_mainMenu);
        }


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
        _mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_newGameBtn.gameObject);
    }
    public void OnNewGameBtnClicked()
    {
        _isNewGameBtnClicked = true;
        _fadeout.SetActive(true);
        _fadeImg.DOFade(0, 3.0f).SetEase(Ease.InOutExpo).OnComplete(OnFadeOutCompleted);
        _mainMenu.SetActive(false);
        _isNewGameBtnClicked = false;
        Managers.Sound.Play(DataManager.SFX_MENU_CHOICE_PATH);
        EventSystem.current.SetSelectedGameObject(null);
        Managers.Scene.LoadScene(define.ESceneType.TUTORIAL);
    }
    public void OnSettingBtnClicked()
    {
        if (_isNewGameBtnClicked)
            return;
    }
    public void OnExitBtnClicked()
    {
        if (_isNewGameBtnClicked)
            return;
        Application.Quit();
    }

    public void OnFadeOutCompleted()
    {
       _fadeImg.color = Color.black; 
       _fadeout.SetActive(false);
    }
}
