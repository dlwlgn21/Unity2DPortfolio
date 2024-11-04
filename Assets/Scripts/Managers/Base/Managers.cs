using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers sInstance;
    public static Managers Instance { get { Init(); return sInstance; } }

    readonly ResourceManager _resourceManager = new();
    readonly InputManager _inputManager = new();
    readonly DataManager _dataManager = new();
    readonly SceneManagerEX _sceneManager = new();
    readonly PauseManager _pauseManager = new();
    readonly SoundManager _soundManager = new();
    readonly UIManager _uiManager = new();
    readonly TweenManager _tweenManager = new();
    readonly GameEventManager _gameEventManager = new();
    readonly MainMenuManager _mainMenuManager = new();
    readonly MonsterPoolManager _monsterPoolManager = new();
    readonly MonsterSpawnManager _monsterSpawnManager = new();
    readonly DropItemManager _dropItemManager = new();
    readonly ProjectilePoolManager _projectilePoolManager = new();
    readonly CamManager _camManager = new();
    readonly CamSwitchManager _camSwitchManager = new();
    readonly UIDialogManager _dialogManager = new();
    readonly PlayerRespawnManager _playerRespawnManager = new();
    readonly PlayerSkillManager _playerSkillManager = new();
    readonly PlayerLevelManager _playerLevelManager = new();
    readonly FullScreenEffectManager _fullScreenEffectManager = new();

    public static InputManager Input { get { return Instance._inputManager; } }
    public static DataManager Data { get { return Instance._dataManager; } }
    public static ResourceManager Resources { get { return Instance._resourceManager; } }
    public static SceneManagerEX Scene { get { return Instance._sceneManager; } }
    public static UIManager UI { get { return Instance._uiManager; } }
    public static PauseManager Pause { get { return Instance._pauseManager; } }
    public static SoundManager Sound { get { return Instance._soundManager; } }
    public static TweenManager Tween { get { return Instance._tweenManager; } }
    public static MainMenuManager MainMenu { get { return Instance._mainMenuManager; } }
    public static MonsterPoolManager MonsterPool { get { return Instance._monsterPoolManager; } }
    public static MonsterSpawnManager MonsterSpawn { get { return Instance._monsterSpawnManager; } }
    public static DropItemManager DropItem { get { return Instance._dropItemManager; } }
    public static ProjectilePoolManager ProjectilePool { get { return Instance._projectilePoolManager; } }
    public static UIDialogManager Dialog { get { return Instance._dialogManager; } }
    public static CamManager Cam { get { return Instance._camManager; } }
    public static CamSwitchManager CamSwitch { get { return Instance._camSwitchManager; } }
    public static GameEventManager GameEvent { get { return Instance._gameEventManager; } }
    public static PlayerRespawnManager PlayerRespawn { get { return Instance._playerRespawnManager; } }
    public static PlayerSkillManager PlayerSkill { get { return Instance._playerSkillManager; } }
    public static PlayerLevelManager PlayerLevel { get { return Instance._playerLevelManager; } }
    public static FullScreenEffectManager FullScreenEffect { get { return Instance._fullScreenEffectManager; } }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (sInstance._sceneManager.ECurrentScene)
        {
            case define.ESceneType.MainMenu:
                {
                    sInstance._inputManager.OnUpdate();
                    break;
                }
            case define.ESceneType.Tutorial:
            case define.ESceneType.AbandonLoadScene:
            case define.ESceneType.ColossalBossCaveScene:
            /* INTENTIONAL FALL THROUGH */
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
                //if (UnityEngine.Input.GetKeyUp(KeyCode.P))
                //{
                //    Time.timeScale = 0.2f;
                //}
                //if (UnityEngine.Input.GetKeyUp(KeyCode.L))
                //{
                //    Time.timeScale = 1f;
                //}
                sInstance._inputManager.OnUpdate();
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
            sInstance._dataManager.Init();
            sInstance._soundManager.Init();
            sInstance._tweenManager.Init();
            sInstance._sceneManager.Init();
            sInstance._fullScreenEffectManager.Init();
        }
    }
    public static void Clear()          
    {
        sInstance._inputManager.Clear(); 
        sInstance._uiManager.Clear();
        sInstance._playerSkillManager.Clear();
        sInstance._tweenManager.Clear();
    }
}
