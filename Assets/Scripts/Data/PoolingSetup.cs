using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolingSetup", menuName = "Data/Meta/Pooling", order = 5)]
public class PoolingSetup : ScriptableObject
{
    public int towerPoolSize;
    public int wallPoolSize;
    public int enemyPoolSize;
    public int projectilePoolSize;
}
