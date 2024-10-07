using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseManager 
{
    // TODO : MainMenu로 간 다음 다시 GameScene 로드했을 때에, PauseManager가 작동안하는 것 수정해야 함!
    public bool IsPaused { get; private set; } = false;
    GameObject _pauseMenu;
    RectTransform _pauseMenuTransform;
    Vector3 _originalScale;
    Button _resumeBtn;
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
            Button mainMenuBtn = Utill.GetComponentInChildrenOrNull<Button>(_pauseMenu, "UIPasueMainMenuBtn");
            mainMenuBtn.onClick.AddListener(OnMainMenuBtnClicked);
            _pauseMenuTransform = Utill.GetComponentInChildrenOrNull<RectTransform>(_pauseMenu.gameObject, "VerticalLayout");
            _originalScale = _pauseMenuTransform.localScale;
            _pauseMenu.SetActive(false);
            Object.DontDestroyOnLoad(_pauseMenu);
        }
    }
    public void Pause()
    {
        Debug.Log("Pause");
        Time.timeScale = 1f;
        IsPaused = true;
        _pauseMenu.SetActive(true);
        _pauseMenuTransform
            .DOScale(_originalScale.x + 0.3f, 0.2f)
            .SetEase(Ease.OutElastic);
        EventSystem.current.SetSelectedGameObject(_resumeBtn.gameObject);
    }
    public void Unpause()               { onUnpause(); }

    public void OnResumeBtnClicked()    { onUnpause(); }
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
        _pauseMenuTransform.localScale = _originalScale;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
