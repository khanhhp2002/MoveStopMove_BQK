using System;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private CharacterBase _player;
    [SerializeField] private VirtualCameraController _virualCameraController;
    [SerializeField] private GameState _gameState = GameState.None;

    public Material[] _obstacleMaterials;
    public UserData UserData;

    // Events
    public Action OnGoldAmountChange;
    public Action OnGameStatePrepare;
    public Action OnGameStatePlaying;
    public Action OnGameStatePause;
    public Action OnGameStateGameOver;
    public Action OnCounterChange;
    public int MaxAliveCounter;
    private int _aliveCounter;
    public int AliveCounter
    {
        get => _aliveCounter;
        set
        {
            _aliveCounter = value;
            OnCounterChange?.Invoke();
            (_player as Player).Ranking = _aliveCounter;
            if (_aliveCounter is 1)
            {
                SetGameState(GameState.GameOver);
            }
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        //tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;
        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (SaveManager.Instance.HasData<UserData>())
        {
            UserData = SaveManager.Instance.LoadData<UserData>();
        }
        else
        {
            UserData = new UserData();
            SaveManager.Instance.SaveData(UserData);
        }
        GameplayUI.Instance.gameObject.SetActive(false);
        ReviveUI.Instance.gameObject.SetActive(false);
        GameoverUI.Instance.gameObject.SetActive(false);
        WeaponShopUI.Instance.gameObject.SetActive(false);
        SkinShopUI.Instance.gameObject.SetActive(false);
        SettingUI.Instance.gameObject.SetActive(false);
    }
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        SetGameState(GameState.Preparing);
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _gameState == GameState.Playing)
        {
            SetGameState(GameState.Preparing);
        }
        if (Input.GetKeyDown(KeyCode.Space) && _gameState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
    }

    /// <summary>
    /// Returns the player.
    /// </summary>
    public CharacterBase Player => _player;

    /// <summary>
    /// Returns current game state.
    /// </summary>
    public GameState GameState => _gameState;

    /// <summary>
    /// Set game state.
    /// </summary>
    /// <param name="gameState"></param>
    public void SetGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Preparing:
                OnGameStatePrepare?.Invoke();
                MaxAliveCounter = RuntimeData.Instance.ZoneData.ZoneDataList[RuntimeData.Instance.ZoneData.CurrentZoneIndex].MaxAliveCounter;
                Debug.Log(MaxAliveCounter);
                _aliveCounter = MaxAliveCounter;
                //Reset bots count
                //Reset player
                //Spawn bots
                break;
            case GameState.Playing:
                OnGameStatePlaying?.Invoke();
                //Start bots
                //Start player
                break;
            case GameState.Paused:
                //Hide Playing UI
                //Show Pause UI
                //Stop bots
                //Stop player
                break;
            case GameState.GameOver:
                UIManager.Instance.OpenLoseUI();
                //Hide Playing UI
                //Show End UI
                break;
        }
        _gameState = gameState;
    }

    /// <summary>
    /// Change user gold amount.
    /// </summary>
    /// <param name="amount"></param>
    public void ChangeGoldAmount(int amount)
    {
        UserData.GoldAmount += amount;
        OnGoldAmountChange?.Invoke();
    }
}
