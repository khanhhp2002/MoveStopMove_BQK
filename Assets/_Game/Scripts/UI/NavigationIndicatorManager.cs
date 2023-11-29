using UnityEngine;

public class NavigationIndicatorManager : Singleton<NavigationIndicatorManager>
{
    [Header("Navigation Indicator Stats")]
    [SerializeField] private NavigationIndicator _navigationIndicatorPrefab;
    [SerializeField] private int _maxNavigationIndicators;

    private ObjectPool<NavigationIndicator> _navigationIndicatorPool;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _navigationIndicatorPool = new ObjectPool<NavigationIndicator>(_navigationIndicatorPrefab.gameObject, transform, _maxNavigationIndicators);
    }

    /// <summary>
    /// Spawns a navigation indicator at the given position.
    /// </summary>
    /// <param name="navigationIndicatorPosition"></param>
    /// <returns></returns>
    public NavigationIndicator Spawn(Vector2 navigationIndicatorPosition)
    {
        var spawnNavigationIndicator = _navigationIndicatorPool.Pull(navigationIndicatorPosition);
        spawnNavigationIndicator.transform.SetParent(transform);
        return spawnNavigationIndicator;
    }

}
