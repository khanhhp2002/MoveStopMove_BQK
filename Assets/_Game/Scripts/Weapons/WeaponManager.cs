using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [Header("Weapon Storage"), Space(5f)]
    [SerializeField] private WeaponSO _weaponSO;
    private Dictionary<WeaponType, ObjectPool<BulletBase>> _weaponPools = new Dictionary<WeaponType, ObjectPool<BulletBase>>();

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

    public void Throw(Vector3 spawnPosition, Vector3 direction, float characterRange, float scaleValue, CharacterBase attacker, WeaponData weaponData, Action<CharacterBase> callBack, WeaponType weaponType, BulletBase weaponPrefab)
    {
        if (!_weaponPools.ContainsKey(weaponType))
        {
            _weaponPools.Add(weaponType, new ObjectPool<BulletBase>(weaponPrefab.gameObject, 5));
        }

        BulletBase weapon = _weaponPools[weaponType].Pull();
        weapon.Throw(spawnPosition, direction, characterRange, scaleValue, attacker, weaponData, callBack);
    }
}
