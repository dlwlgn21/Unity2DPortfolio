using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public sealed class PauseManager 
{
    // TODO : MainMenu로 간 다음 다시 GameScene 로드했을 때에, PauseManager가 작동안하는 것 수정해야 함!
    public bool IsPaused { get; private set; } = false;
    GameObject _pauseMenu;
    Button _resumeBtn;
    Button _goMainMenuBtn;
    public void Init()
    {
        if (GameObject.Find("UIPauseMenu") == null)
        {
            GameObject ori = Managers.Resources.Load<GameObject>("Prefabs/UI/UIPauseMenu");
            if (ori == null)
                Debug.Assert(false);
            _pauseMenu = Object.Instantiate(ori);
            _pauseMenu.name = "UIPauseMenu";
            _resumeBtn = Utill.GetComponentInChildrenOrNull<Button>(_pauseMenu, "UIPauseResumeBtn");
            _resumeBtn.onClick.AddListener(OnResumeBtnClicked);
            _goMainMenuBtn = Utill.GetComponentInChildrenOrNull<Button>(_pauseMenu, "UIPasueMainMenuBtn");
            _goMainMenuBtn.onClick.AddListener(OnMainMenuBtnClicked);
            _pauseMenu.SetActive(false);
            Object.DontDestroyOnLoad(_pauseMenu);
        }
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        _pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_resumeBtn.gameObject);
    }
    public void Unpause()               
    { 
        onUnpause(); 
    }

    public void OnResumeBtnClicked()    
    { 
        onUnpause();
    }
    public void OnMainMenuBtnClicked()
    {
        onUnpause();
        SceneManager.LoadScene((int)define.ESceneType.MainMenu);
    }

    private void onUnpause()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        _pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
