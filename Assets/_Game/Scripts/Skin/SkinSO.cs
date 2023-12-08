using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SkinSO")]
public class SkinSO : ScriptableObject
{
    public List<HairData> Hairs = new List<HairData>();

    public List<PantData> Pants = new List<PantData>();

    public List<Material> SkinColors = new List<Material>();

    public Material EquipPant(int index = -1)
    {
        if (index is -1)
        {
            return Pants[Random.Range(0, Pants.Count)].Material;
        }
        else
        {
            return Pants[index].Material;
        }
    }
}
