using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SkinSO")]
public class SkinSO : ScriptableObject
{
    public List<GameObject> Hairs = new List<GameObject>();

    public List<Material> Pants = new List<Material>();

    public List<Material> SkinColors = new List<Material>();

    public Material EquipPant(int index = -1)
    {
        if (index is -1)
        {
            return Pants[Random.Range(0, Pants.Count)];
        }
        else
        {
            return Pants[index];
        }
    }
}
