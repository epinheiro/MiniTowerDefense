﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem
{
    // Meta
    GameManager _gameManager;
    
    // Systems
    PrefabPoolingSystem _pool;
    
    //// Public API
    public EnemySystem(GameManager gameManager, GameObject prefab, int poolSize, Transform enemyParent){
        this._gameManager = gameManager;
        this._pool = new PrefabPoolingSystem(prefab, poolSize, enemyParent);
    }

    public GameObject SpawnEnemyAt(Vector3 position, EnemyAttributes attributes){
        GameObject enemy = _pool.GetInstance();
        enemy.GetComponent<EnemyBehaviour>().SetEnemyAttributes(position, attributes);
        return enemy;
    }

    public void ReturnEnemyElement(GameObject go){
        _pool.ReturnInstance(go);
    }
}
