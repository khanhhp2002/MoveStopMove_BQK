using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [Header("Weapon Storage"), Space(5f)]
    [SerializeField] private WeaponSO _weaponSO;
    private Dictionary<WeaponType, ObjectPool<BulletBase>> _weaponPools = new Dictionary<WeaponType, ObjectPool<BulletBase>>();

    // Hand Weapon Logic ====================================================================================================

    /// <summary>
    /// Get a weapon data by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public WeaponData GetWeaponDataByIndex(int index = -1)
    {
        if (index < 0 || index >= _weaponSO.weaponDataList.Count)
        {
            return _weaponSO.weaponDataList[UnityEngine.Random.Range(0, _weaponSO.weaponDataList.Count)];
        }
        else
        {
            return _weaponSO.weaponDataList[index];
        }
    }

    /// <summary>
    /// Get a random weapon data.
    /// </summary>
    /// <returns></returns>
    public WeaponData GetRandomWeaponData()
    {
        return GetWeaponDataByIndex();
    }

    /// <summary>
    /// Get total weapon data.
    /// </summary>
    /// <returns></returns>
    public byte GetWeaponDataCount()
    {
        return (byte)_weaponSO.weaponDataList.Count;
    }

    // Throw Weapon Logic ====================================================================================================

    /// <summary>
    /// Try to get a weapon data by weapon type from the weapon pool.
    /// </summary>
    /// <param name="weaponType"></param>
    /// <param name="weaponPrefab"></param>
    /// <returns></returns>
    public BulletBase GetWeapon(WeaponType weaponType, BulletBase weaponPrefab)
    {
        if (!_weaponPools.ContainsKey(weaponType))
        {
            _weaponPools.Add(weaponType, new ObjectPool<BulletBase>(weaponPrefab.gameObject, 5));
        }

        return _weaponPools[weaponType].Pull();
    }
}
