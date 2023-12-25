using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

public class BotManager : Singleton<BotManager>
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _maxBots;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _delaySpawnTime;
    [SerializeField] private VFXController _spawnVFXPrefab;
    private List<Bot> _activeBot = new List<Bot>();
    private ObjectPool<VFXController> _spawnVFXPool;
    private ObjectPool<Bot> _botPool;
    private Stack<string> _namePool = new Stack<string>();
    private bool _isGettingRandomName;
    private bool _firstLoad = true;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        _botPool = new ObjectPool<Bot>(_botPrefab.gameObject, null, OnBotReturnToPool, _maxBots);
        _spawnVFXPool = new ObjectPool<VFXController>(_spawnVFXPrefab.gameObject, null, null, _maxBots);
        _firstLoad = true;
        if (CheckNetworkStatus())
            StartCoroutine(GetDataFromApi());
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        //GameplayManager.Instance.OnGameStatePlaying += SetAllBotName;
        for (int i = 0; i < _maxBots; i++)
        {
            SpawnBot();
        }
    }

    /// <summary>
    /// Spawns a bot.
    /// </summary>
    private bool SpawnBot()
    {
        if (_botPool.pooledCount is 0 || GameplayManager.Instance.AliveCounter - 1 <= _maxBots - _botPool.pooledCount || GameplayManager.Instance.GameState == GameState.GameOver) return false;
        StartCoroutine(SpawnVFX());
        return true;
    }

    /// <summary>
    /// Spawns a VFX.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnVFX()
    {
        Debug.Log("Spawn --------------------------------------");
        Vector3 randomPosition = new Vector3(Random.Range(-_spawnRadius, _spawnRadius), 0f, Random.Range(-_spawnRadius, _spawnRadius));
        Vector3 distance = randomPosition - GameplayManager.Instance.Player.transform.position;
        if (_firstLoad && distance.sqrMagnitude < 625)
        {
            randomPosition = distance.normalized * 26f + GameplayManager.Instance.Player.transform.position;
        }
        VFXController vFXController = _spawnVFXPool.Pull(randomPosition);
        Debug.Log("Spawn VFX");
        yield return new WaitForSeconds(2f);
        Bot bot = _botPool.Pull(randomPosition);
        _activeBot.Add(bot);
        if (_namePool.Count > 0) bot.Name.text = _namePool.Pop();
        else bot.Name.text = RandomStringGenerator.GetRandomString(Random.Range(6, 11));
        vFXController.ReturnToPool();
        if (_firstLoad)
        {
            _firstLoad = false;
            UIManager.Instance.AllowInteract();
        }
    }

    /// <summary>
    /// OnBotReturnToPool is called when a bot returns to the pool.
    /// </summary>
    /// <param name="bot"></param>
    private void OnBotReturnToPool(Bot bot)
    {
        if (_namePool.Count <= 10 && !_isGettingRandomName && CheckNetworkStatus())
        {
            StartCoroutine(GetDataFromApi());
        }
        _activeBot.Remove(bot);
        bot.ForceControlBotAnimation(BotState.Idle);
        Invoke(nameof(SpawnBot), _delaySpawnTime);
    }

    /*public void ForceSpawnAll()
    {
        while (SpawnBot()) ;
    }*/

    private void SetAllBotName()
    {
        foreach (Bot bot in _activeBot)
        {
            bot.Name.text = _namePool.Pop();
        }
    }

    IEnumerator GetDataFromApi()
    {
        _isGettingRandomName = true;
        using (UnityWebRequest www = UnityWebRequest.Get($"https://randomuser.me/api/?results={_maxBots + 10}&inc=name,nat"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                // Deserialize JSON response
                ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(www.downloadHandler.text);

                // Access the data
                foreach (Result result in apiResponse.results)
                {
                    string name = $"#{result.nat} {result.name.first} {result.name.last}";
                    _namePool.Push(name);
                }
            }
        }
        _isGettingRandomName = false;
    }

    private bool CheckNetworkStatus()
    {
        if (NetworkInterface.GetIsNetworkAvailable())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
[System.Serializable]
public class Name
{
    public string title;
    public string first;
    public string last;
}

[System.Serializable]
public class Result
{
    public Name name;
    public string nat;
}

[System.Serializable]
public class Info
{
    public string seed;
    public int results;
    public int page;
    public string version;
}

[System.Serializable]
public class ApiResponse
{
    public List<Result> results;
    public Info info;
}

