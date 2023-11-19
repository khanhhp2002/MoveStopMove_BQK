using UnityEngine;

public class BotManager : Singleton<BotManager>
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _maxBots;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _delaySpawnTime;
    private ObjectPool<Bot> _botPool;

    public void Awake()
    {
        _botPool = new ObjectPool<Bot>(_botPrefab.gameObject, null, OnBotReturnToPool, _maxBots);
    }

    public void Start()
    {
        for (int i = 0; i < _maxBots; i++)
        {
            SpawnBot();
        }
    }

    private void SpawnBot()
    {
        Debug.Log("Respawn !!!");
        Vector3 randomPosition = new Vector3(Random.Range(-_spawnRadius, _spawnRadius), GameplayManager.Instance.Player.transform.position.y, Random.Range(-_spawnRadius, _spawnRadius));
        _botPool.Pull(randomPosition);
    }

    private void OnBotReturnToPool(Bot bot)
    {
        bot.ForceControlBotAnimation(BotState.Idle);
        Invoke(nameof(SpawnBot), _delaySpawnTime);
    }

}
