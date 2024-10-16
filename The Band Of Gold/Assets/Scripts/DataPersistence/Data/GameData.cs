using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int DeathCount;
    public Vector3 playerPosition;

    // the values defined in this constructor will be defualt values
    // the game starts with when there's no data to Load
    public GameData()
    {
        playerPosition = Vector3.zero;
    }
}
