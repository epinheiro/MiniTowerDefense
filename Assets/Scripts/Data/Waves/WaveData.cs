using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Wave/WaveData", order = 2)]
public class WaveData : ScriptableObject
{
    public float totalTime;

    public int spawnPointsUsed;

    public EnemyWave enemy1;
    public EnemyWave enemy2;
}
