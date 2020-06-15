using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSetup", menuName = "Data/Meta/GameSetup", order = 5)]
public class GameSetup : ScriptableObject
{
    // Pooling
    public int towerPoolSize;
    public int wallPoolSize;
    public int enemyPoolSize;
    public int projectilePoolSize;
}
