using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Wave/GameWaveDefinition", order = 1)]
public class GameWaveDefinition : ScriptableObject
{
    public WaveData[] waves;
}
