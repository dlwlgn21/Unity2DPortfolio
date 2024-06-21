using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers sInstance;
    public static Managers Instance { get { Init(); return sInstance; } }

    ResourceManager _resourceManager = new ResourceManager();
    InputManager _inputManager = new InputManager();
    DataManager _dataManager = new DataManager();
    SceneManagerEX _sceneManager = new SceneManagerEX();
    PauseManager _pauseManager = new PauseManager();
    SoundManager _soundManager = new SoundManager();
    TweenManager _tweenManager = new TweenManager();
    GameEventManager _gameEventManager = new GameEventManager();
    MainMenuManager _mainMenuManager = new MainMenuManager();
    MonsterPoolManager _monsterPoolManager = new MonsterPoolManager();
    PlayerSkillPoolManager _playerSkillPoolManager = new PlayerSkillPoolManager();
    PlayerSkillManager _playerSkillManager = new PlayerSkillManager();
    CamShakeManager _camShakeManager = new CamShakeManager();
    CameraManager _camManager = new CameraManager();
    UIDialogManager _dialogManager = new UIDialogManager();
    TestStaticRegisterEventManager _staticRegisterEventManager = new TestStaticRegisterEventManager();
    // 6.5일 전투시스템에서 공격성공, 피격시에 슬로우 타임 적용하기 위해서 TimeManager 추가 
    TimeManager _timeManager = null;
    public static TimeManager TimeManager { get { return Instance._timeManager; } }
    public static InputManager Input { get { return Instance._inputManager; } }
    public static DataManager Data { get { return Instance._dataManager; } }
    public static ResourceManager Resources { get { return Instance._resourceManager; } }
    public static SceneManagerEX Scene { get { return Instance._sceneManager; } }
    public static PauseManager Pause { get { return Instance._pauseManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static TweenManager Tween { get { return Instance._tweenManager; } }
    public static MainMenuManager MainMenu { get { return Instance._mainMenuManager; } }
    public static MonsterPoolManager MonsterPool { get { return Instance._monsterPoolManager; } }
    public static PlayerSkillPoolManager SkillPool { get { return Instance._playerSkillPoolManager; } }
    public static PlayerSkillManager Skill { get { return Instance._playerSkillManager; } }
    public static UIDialogManager Dialog { get { return Instance._dialogManager; } }
    public static CamShakeManager CamShake { get { return Instance._camShakeManager; } }
    public static CameraManager CamManager { get { return Instance._camManager; } }
    public static GameEventManager GameEvent { get { return Instance._gameEventManager; } }

    // Added part For BloodParticle
    HitParticleManager _hitParticle = new HitParticleManager();
    public static HitParticleManager HitParticle { get { return Instance._hitParticle; } }


    // TODO : 어떻게 할지 결정해야 한당. 이거 일단 테스트용임.
    public static TestStaticRegisterEventManager RegisterStaticEventManager { get { return Instance._staticRegisterEventManager; } }



    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (sInstance._sceneManager.ECurrentScene)
        {
            case define.ESceneType.MAIN_MENU:
                {

                    break;
                }
            case define.ESceneType.TUTORIAL:
            /* INTENTIONAL FALL THROUGH */
            case define.ESceneType.MAIN_PLAY:
                // Pause Check
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                {
                    if (!sInstance._pauseManager.IsPaused)
                    { 
                        sInstance._pauseManager.Pause();
                    }
                    else
                    {
                        sInstance._pauseManager.Unpause();
                    }
                }
                if (sInstance._pauseManager.IsPaused)
                {
                    return;
                }

                // FOR TEST
                if (UnityEngine.Input.GetKeyUp(KeyCode.P))
                {
                    Time.timeScale = 0.2f;
                }
                if (UnityEngine.Input.GetKeyUp(KeyCode.L))
                {
                    Time.timeScale = 1f;
                }
                Input.OnUpdate();
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
            sInstance._dataManager.Init();
            sInstance._sceneManager.Init();

            // TODO : 6.5일 TimeManager를 위해 추가. 꼼수사용하는 거 같아서 뭔가 찝찝. 나중에 바꾸던지 해용
            GameObject timeManager = GameObject.Find("@TimeManager");
            if (timeManager == null)
            {
                timeManager = new GameObject { name = "@TimeManager" };
                sInstance._timeManager = timeManager.AddComponent<TimeManager>();
                sInstance._timeManager.Init();
            }
        }
    }
    public static void Clear()          { Input.Clear(); }
}
