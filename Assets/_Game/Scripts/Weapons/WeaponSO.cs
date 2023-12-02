using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public List<WeaponData> weaponDataList = new List<WeaponData>();
}
