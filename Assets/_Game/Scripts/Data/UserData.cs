using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class UserData
{
    // Player infomation.
    [SerializeField] private string _name = "New Player";
    public string Name
    {
        get => _name;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _name = value;
            }
        }
    }

    // Player stats.
    [SerializeField] private int _level = 1;
    public int Level { get => _level; set => _level = value; }

    [SerializeField] private int _experience = 0;
    public int Experience { get => _experience; set => _experience = value; }

    [SerializeField] private int _goldAmount = 0;
    public int GoldAmount { get => _goldAmount; set => _goldAmount = value; }

    // Statistics.
    [SerializeField] private int _numberOfGames = 0;
    public int NumberOfGames { get => _numberOfGames; set => _numberOfGames = value; }

    [SerializeField] private int _numberOfWins = 0;
    public int NumberOfWins { get => _numberOfWins; set => _numberOfWins = value; }

    [SerializeField] private int _highScore = 0;
    public int HighScore { get => _highScore; set => _highScore = value; }

    // Unlocked items.
    [SerializeField] private List<int> _unlockedWeapons = new List<int>();
    public List<int> UnlockedWeapons { get => _unlockedWeapons; set => _unlockedWeapons = value; }

    [SerializeField] private List<int> _unlockedPants = new List<int>();
    public List<int> UnlockedPants { get => _unlockedPants; set => _unlockedPants = value; }

    [SerializeField] private List<int> _unlockedHairs = new List<int>();
    public List<int> UnlockedHairs { get => _unlockedHairs; set => _unlockedHairs = value; }

    // Equipped items.
    [SerializeField] private int _equippedWeapon = 0;
    public int EquippedWeapon { get => _equippedWeapon; set => _equippedWeapon = value; }

    [SerializeField] private int _equippedPant = 0;
    public int EquippedPant { get => _equippedPant; set => _equippedPant = value; }

    [SerializeField] private int _equippedHair = 0;
    public int EquippedHair { get => _equippedHair; set => _equippedHair = value; }

    public UserData()
    {
        // Player infomation.
        _name = "New Player";
        // Player stats.
        _level = 1;
        _experience = 0;
        _goldAmount = 0;
        // Unlocked items.
        _unlockedWeapons.Add(0);
        _unlockedPants.Add(0);
        _unlockedHairs.Add(0);
        // Equipped items.
        _equippedWeapon = 2;
        _equippedPant = 0;
        _equippedHair = 0;
    }
}