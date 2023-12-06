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
    [SerializeField] private byte _level = 1;
    public byte Level { get => _level; set => _level = value; }

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
    [SerializeField] private List<byte> _unlockedWeapons = new List<byte>();
    public List<byte> UnlockedWeapons { get => _unlockedWeapons; set => _unlockedWeapons = value; }

    [SerializeField] private List<byte> _unlockedPants = new List<byte>();
    public List<byte> UnlockedPants { get => _unlockedPants; set => _unlockedPants = value; }

    [SerializeField] private List<byte> _unlockedHairs = new List<byte>();
    public List<byte> UnlockedHairs { get => _unlockedHairs; set => _unlockedHairs = value; }

    // Equipped items.
    [SerializeField] private byte _equippedWeapon = 0;
    public byte EquippedWeapon { get => _equippedWeapon; set => _equippedWeapon = value; }

    [SerializeField] private byte _equippedPant = 0;
    public byte EquippedPant { get => _equippedPant; set => _equippedPant = value; }

    [SerializeField] private byte _equippedHair = 0;
    public byte EquippedHair { get => _equippedHair; set => _equippedHair = value; }

    // User settings.
    [SerializeField] private bool _isVibrationEnabled = true;
    public bool IsVibrationEnabled { get => _isVibrationEnabled; set => _isVibrationEnabled = value; }

    [SerializeField] private bool _isSoundEnabled = true;
    public bool IsSoundEnabled { get => _isSoundEnabled; set => _isSoundEnabled = value; }

    [SerializeField] private bool _isADSEnable = true;
    public bool IsADSEnable { get => _isADSEnable; set => _isADSEnable = value; }
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
        _equippedWeapon = 0;
        _equippedPant = 0;
        _equippedHair = 0;
    }
}
