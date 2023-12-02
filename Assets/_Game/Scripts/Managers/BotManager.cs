using UnityEngine;

public class BotManager : Singleton<BotManager>
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _maxBots;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _delaySpawnTime;
    private ObjectPool<Bot> _botPool;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        _botPool = new ObjectPool<Bot>(_botPrefab.gameObject, null, OnBotReturnToPool, _maxBots);
    }

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        for (int i = 0; i < _maxBots; i++)
        {
            SpawnBot();
        }
    }

    /// <summary>
    /// Spawns a bot.
    /// </summary>
    private void SpawnBot()
    {
        if (GameplayManager.Instance.AliveCounter - 1 <= _maxBots - _botPool.pooledCount || GameplayManager.Instance.Player.IsDead) return;
        Vector3 randomPosition = new Vector3(Random.Range(-_spawnRadius, _spawnRadius), 0f, Random.Range(-_spawnRadius, _spawnRadius));
        _botPool.Pull(randomPosition);
    }

    /// <summary>
    /// OnBotReturnToPool is called when a bot returns to the pool.
    /// </summary>
    /// <param name="bot"></param>
    private void OnBotReturnToPool(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Idle);
        Invoke(nameof(SpawnBot), _delaySpawnTime);
    }

}
