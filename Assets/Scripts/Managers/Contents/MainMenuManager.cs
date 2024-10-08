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
        _mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_newGameBtn.gameObject);
    }
    public void OnNewGameBtnClicked()
    {
        _isNewGameBtnClicked = true;
        _mainMenu.SetActive(false);
        _isNewGameBtnClicked = false;
        Managers.Sound.Play(DataManager.SFX_MENU_CHOICE_PATH);
        EventSystem.current.SetSelectedGameObject(null);
        Managers.Scene.LoadScene(define.ESceneType.Tutorial);
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


}
