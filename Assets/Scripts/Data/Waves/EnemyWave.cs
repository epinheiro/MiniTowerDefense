using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/EnemyFormation", order = 3)]
public class EnemyWave : ScriptableObject{
    public int quantity;

    public SpawnSystem.SpawnTypes formation;
}
