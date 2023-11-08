using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Material[] _pants;
    [SerializeField] private CharacterBase _player;
    [SerializeField] private VirtualCameraController _virualCameraController;

    [SerializeField] private GameState _gameState = GameState.None;

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
            return _pants[Random.Range(0, _pants.Length)];
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
                CameraManager.Instance.OnGameStatePrepare();
                //Reset bots count
                //Reset player
                //Spawn bots
                break;
            case GameState.Playing:
                UIManager.Instance.OnGameStatePlaying();
                CameraManager.Instance.OnGameStatePlaying();
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
        }
        _gameState = gameState;
    }
}
