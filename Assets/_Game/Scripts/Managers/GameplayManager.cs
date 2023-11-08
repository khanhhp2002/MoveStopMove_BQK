using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Material[] _pants;
    [SerializeField] private CharacterBase _player;

    public CharacterBase Player => _player;

    /// <summary>
    /// Get a random pant's skin material.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Material GetPantByIndex(int index = -1)
    {
        if (index < 0 || index >= _pants.Length)
        {
            return _pants[Random.Range(0, _pants.Length)];
        }
        else
        {
            return _pants[index];
        }
    }
}
