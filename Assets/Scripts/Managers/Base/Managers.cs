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

    ResourceManager _resourceManager = new ResourceManager();
    InputManager _inputManager = new InputManager();
    DataManager _dataManager = new DataManager();
    PauseManager _pauseManager = new PauseManager();
    SoundManager _soundManager = new SoundManager();
    TweenManager _tweenManager = new TweenManager();
    GameEventManager _gameEventManager = new GameEventManager();
    MainMenuManager _mainMenuManager = new MainMenuManager();
    MonsterPoolManager _monsterPoolManager = new MonsterPoolManager();
    PlayerSkillPoolManager _playerSkillPoolManager = new PlayerSkillPoolManager();
    CamShakeManager _camShakeManager = new CamShakeManager();
    UIDialogManager _dialogManager = new UIDialogManager();
    public static InputManager Input { get { return Instance._inputManager; } }
    public static DataManager Data { get { return Instance._dataManager; } }
    public static ResourceManager Resources { get { return Instance._resourceManager; } }
    public static PauseManager Pause { get { return Instance._pauseManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static TweenManager Tween { get { return Instance._tweenManager; } }
    public static MainMenuManager MainMenu { get { return Instance._mainMenuManager; } }
    public static MonsterPoolManager MonsterPool { get { return Instance._monsterPoolManager; } }
    public static PlayerSkillPoolManager SkillPool { get { return Instance._playerSkillPoolManager; } }
    public static UIDialogManager Dialog { get { return Instance._dialogManager; } }
    public static CamShakeManager CamShake { get { return Instance._camShakeManager; } }
    public static GameEventManager GameEvent { get { return Instance._gameEventManager; } }

    // Added part For BloodParticle
    HitParticleManager _hitParticle = new HitParticleManager();
    public static HitParticleManager HitParticle { get { return Instance._hitParticle; } }
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
                        if (!sInstance._pauseManager.IsPaused)
                            sInstance._pauseManager.Pause();
                        else
                            sInstance._pauseManager.Unpause();
                    }
                    if (sInstance._pauseManager.IsPaused)
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
            sInstance._soundManager.Init();
            sInstance._tweenManager.Init();
            sInstance._mainMenuManager.Init();
            sInstance._dataManager.Init();
            sInstance._hitParticle.Init();
            sInstance._monsterPoolManager.Init();
            sInstance._playerSkillPoolManager.Init();
            sInstance._pauseManager.Init();
            sInstance._dialogManager.Init();
            sInstance._camShakeManager.Init();
        }
    }
    public static void Clear()          { Input.Clear(); }
}
