using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Wave/WaveData", order = 2)]
public class WaveData : ScriptableObject
{
    public List<EnemyWave> enemySubWaves;
}
