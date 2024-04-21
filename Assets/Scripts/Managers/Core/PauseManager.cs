using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseManager 
{
    // TODO : MainMenu로 간 다음 다시 GameScene 로드했을 때에, PauseManager가 작동안하는 것 수정해야 함!
    public bool IsPaused { get; private set; } = false;
    GameObject mPauseMenu;
    RectTransform mPauseMenuTransform;
    Vector3 mOriginalScale;
    Button mResumeBtn;
    public void Init()
    {
        GameObject ori = Managers.Resources.Load<GameObject>("Prefabs/UI/UIPauseMenu");
        if (ori == null)
            Debug.Assert(false);
        mPauseMenu = Object.Instantiate(ori);
        mPauseMenu.name = "UIPauseMenu";
        mResumeBtn = Utill.GetComponentInChildrenOrNull<Button>(mPauseMenu, "UIPauseResumeBtn");
        mResumeBtn.onClick.AddListener(OnResumeBtnClicked);
        Button mainMenuBtn = Utill.GetComponentInChildrenOrNull<Button>(mPauseMenu, "UIPasueMainMenuBtn");
        mainMenuBtn.onClick.AddListener(OnMainMenuBtnClicked);
        mPauseMenuTransform = Utill.GetComponentInChildrenOrNull<RectTransform>(mPauseMenu.gameObject, "VerticalLayout");
        mOriginalScale = mPauseMenuTransform.localScale;
        mPauseMenu.SetActive(false);
        Object.DontDestroyOnLoad(mPauseMenu);
    }
    public void Pause()
    {
        Debug.Log("Pause");
        Time.timeScale = 1f;
        IsPaused = true;
        mPauseMenu.SetActive(true);
        mPauseMenuTransform
            .DOScale(mOriginalScale.x + 0.3f, 0.2f)
            .SetEase(Ease.OutElastic);
        EventSystem.current.SetSelectedGameObject(mResumeBtn.gameObject);
    }
    public void Unpause()               { onUnpause(); }

    public void OnResumeBtnClicked()    { onUnpause(); }
    public void OnMainMenuBtnClicked()
    {
        onUnpause();
        SceneManager.LoadScene((int)define.ESceneType.MAIN_MENU);
    }

    private void onUnpause()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        mPauseMenu.SetActive(false);
        mPauseMenuTransform.localScale = mOriginalScale;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
