using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    private static Dictionary<Collider, CharacterBase> _cacheCharacters = new Dictionary<Collider, CharacterBase>();

    /// <summary>
    /// Get the character from the collider.
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public static CharacterBase GetCharacter(this Collider collider)
    {
        if (_cacheCharacters.ContainsKey(collider))
            return _cacheCharacters[collider];
        _cacheCharacters.Add(collider, collider.GetComponent<CharacterBase>());
        return _cacheCharacters[collider];
    }
}
