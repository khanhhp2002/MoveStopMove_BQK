using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : Singleton<UserData>
{
    private int _goldsCount;
    public int GoldsCount { get; set; }
}
