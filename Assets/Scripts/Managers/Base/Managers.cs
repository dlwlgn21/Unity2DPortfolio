using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using UnityEngine.Windows;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers sInstance;
    public static Managers Instance { get { Init(); return sInstance; } }

    ResourceManager mResourceManager = new ResourceManager();
    InputManager mInputManager = new InputManager();
    DataManager mDataManager = new DataManager();
    PauseManager mPauseManager = new PauseManager();
    TweenManager mTweenManager = new TweenManager();
    MainMenuManager mMainMenuManager = new MainMenuManager();
    public static InputManager Input { get { return Instance.mInputManager; } }
    public static DataManager Data { get { return Instance.mDataManager; } }
    public static ResourceManager Resources { get { return Instance.mResourceManager; } }
    public static PauseManager Pause { get { return Instance.mPauseManager; } }
    public static TweenManager Tween { get { return Instance.mTweenManager; } }
    public static MainMenuManager MainMenu { get { return Instance.mMainMenuManager; } }

    // Added part For BloodParticle
    HitParticleManager mHitParticle = new HitParticleManager();
    public static HitParticleManager HitParticle { get { return Instance.mHitParticle; } }
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case (int)define.ESceneType.MAIN_MENU:
                {

                }
                break;
            case (int)define.ESceneType.GAME_SCENE:
                {
                    // Pause Check
                    if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (!sInstance.mPauseManager.IsPaused)
                            sInstance.mPauseManager.Pause();
                        else
                            sInstance.mPauseManager.Unpause();
                    }
                    if (sInstance.mPauseManager.IsPaused)
                        return;
                    Input.OnUpdate();
                }
                break;
        }
    }

    static void Init()
    {
        if (sInstance == null)
        {
            GameObject go = GameObject.Find("@GameManagers");
            if (go == null)
            {
                go = new GameObject { name = "@GameManagers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            sInstance = go.GetComponent<Managers>();
            sInstance.mTweenManager.Init();
            sInstance.mMainMenuManager.Init();
            sInstance.mDataManager.Init();
            sInstance.mHitParticle.Init();
            sInstance.mPauseManager.Init();
        }
    }

    public static void Clear()          { Input.Clear(); }

}
