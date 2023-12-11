using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/ZoneSO")]
public class ZoneSO : ScriptableObject
{
    public List<ZoneData> ZoneDataList;

    public int CurrentZoneIndex;
}
