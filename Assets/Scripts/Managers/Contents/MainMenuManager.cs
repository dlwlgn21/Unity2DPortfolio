using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum EMainMenuButtonType
{
    NewGame,
    Setting,
    Exit,
    None
}


public sealed class MainMenuManager
{
    GameObject _mainMenu;
    Button _newGameBtn;
    Button _settingBtn;
    Button _exitBtn;
    public EMainMenuButtonType ECurrentSelectedButtonType { get; set; } = EMainMenuButtonType.NewGame;
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
            Managers.Input.KeyboardHandler -= OnEnterDown;
            Managers.Input.KeyboardHandler += OnEnterDown;
            _mainMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_newGameBtn.gameObject);
        }
    }
    void OnEnterDown()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            switch (ECurrentSelectedButtonType)
            {
                case EMainMenuButtonType.NewGame:
                    OnNewGameBtnClicked();
                    break;
                case EMainMenuButtonType.Setting:
                    OnSettingBtnClicked();
                    break;
                case EMainMenuButtonType.Exit:
                    OnExitBtnClicked();
                    break;
                default:
                    break;
            }
        }
    }

    void OnNewGameBtnClicked()
    {
        _mainMenu.SetActive(false);
        Managers.Sound.Play(DataManager.SFX_MENU_CHOICE_PATH);
        EventSystem.current.SetSelectedGameObject(null);
        Managers.Scene.LoadScene(define.ESceneType.Tutorial);
    }
    void OnSettingBtnClicked()
    {
        Managers.Sound.Play(DataManager.SFX_MENU_CHOICE_PATH);
    }
    void OnExitBtnClicked()
    {
        Managers.Sound.Play(DataManager.SFX_MENU_CHOICE_PATH);
        Application.Quit();
    }

    public void Clear()
    {
        Managers.Input.KeyboardHandler -= OnEnterDown;
    }
}
