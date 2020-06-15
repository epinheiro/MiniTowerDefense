using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSetup", menuName = "Data/Meta/GameSetup", order = 5)]
public class GameSetup : ScriptableObject
{
    // Pooling
    public int towerPoolSize = 5;
    public int wallPoolSize = 3;
    public int enemyPoolSize = 20;
    public int projectilePoolSize = 40;

    // Core
    public int coreTotalLife = 30;
    public float coreMenaceCheckPeriod = 2;

    // Tower
    public int towerEnemyLockdownLimit = 2;
    public float towerMenaceCheckPeriod = 0.5f;

    // Projectile
    public float projectileVelocity = 5f;
}
