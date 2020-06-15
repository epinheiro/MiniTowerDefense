using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolSetup", menuName = "Data/Meta/Pooling", order = 5)]
public class PoolingSetup : ScriptableObject
{
    public int eachConstructionPoolSize;
    public int enemyPoolSize;
    public int projectilePoolSize;
}
