using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [Header("Weapon Storage"), Space(5f)]
    [SerializeField] private WeaponSO _weaponSO;

    /// <summary>
    /// Get a weapon data by index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public WeaponData GetWeaponDataByIndex(int index = -1)
    {
        if (index < 0 || index >= _weaponSO.weaponDataList.Count)
        {
            return _weaponSO.weaponDataList[Random.Range(0, _weaponSO.weaponDataList.Count)];
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
}
