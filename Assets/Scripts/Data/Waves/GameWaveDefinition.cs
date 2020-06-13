using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameWaves", order = 1)]
public class GameWaveDefinition : ScriptableObject
{
    public List<WaveData> waves;
}
