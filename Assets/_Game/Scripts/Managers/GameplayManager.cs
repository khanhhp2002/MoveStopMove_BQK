using System;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Material[] _pants;
    [SerializeField] private CharacterBase _player;
    [SerializeField] private VirtualCameraController _virualCameraController;
    [SerializeField] private GameState _gameState = GameState.None;
    [SerializeField] public UserData _userData;

    public Action OnGoldAmountChange;
    public Action OnGameStatePrepare;
    public Action OnGameStatePlaying;
    public Action OnGameStatePause;
    public Action OnGameStateGameOver;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (SaveManager.Instance.HasData<UserData>())
        {
            _userData = SaveManager.Instance.LoadData<UserData>();
        }
        else
        {
            _userData = new UserData();
            SaveManager.Instance.SaveData(_userData);
        }
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
    /// Get a random pant's skin material.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Material GetPantByIndex(int index = -1)
    {
        if (index < 0 || index >= _pants.Length)
        {
            return _pants[UnityEngine.Random.Range(0, _pants.Length)];
        }
        else
        {
            return _pants[index];
        }
    }

    /// <summary>
    /// Set game state.
    /// </summary>
    /// <param name="gameState"></param>
    public void SetGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Preparing:
                UIManager.Instance.OnGameStatePrepare();
                OnGameStatePrepare?.Invoke();
                //Reset bots count
                //Reset player
                //Spawn bots
                break;
            case GameState.Playing:
                UIManager.Instance.OnGameStatePlaying();
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
                //Hide Playing UI
                //Show End UI
                break;
            case GameState.WeaponShopEnter:
                UIManager.Instance.OnWeaponShopEnter();
                break;
            case GameState.WeaponShopExit:
                UIManager.Instance.OnWeaponShopExit();
                break;
        }
        _gameState = gameState;
    }

    public void ChangeGoldAmount(int amount)
    {
        _userData.GoldAmount += amount;
        OnGoldAmountChange?.Invoke();
    }
}
