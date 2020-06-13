using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem
{
    // Meta
    GameManager _gameManager;
    
    // Systems
    PrefabPoolingSystem _pool;
    
    //// Public API
    public EnemySystem(GameManager gameManager, GameObject _enemyPrefab, int poolSize, Transform enemyParent){
        this._gameManager = gameManager;
        this._pool = new PrefabPoolingSystem(_enemyPrefab, poolSize, enemyParent);

        // Debug calls
        _gameManager.StartCoroutine(SpawnEnemyWithDelay(1, 5)); // TODO - Futurely DELETE this call
    }

    public GameObject SpawnEnemyAt(Vector3 position){
        GameObject enemy = _pool.GetInstance();
        enemy.GetComponent<EnemyBehaviour>().SetEnemyAttributes(position);
        return enemy;
    }

    public void ReturnEnemyElement(GameObject go){
        _pool.ReturnInstance(go);
    }

    //// Coroutines
    IEnumerator SpawnEnemyWithDelay(float delaySeconds, int remainingEnemies){
        if(remainingEnemies > 0){
            SpawnEnemyAt(GameObject.Find("EnemySpawnPoint-TEST").transform.position); // TODO - change this placeholder spawnpoint
            yield return new WaitForSecondsRealtime(delaySeconds);
            yield return SpawnEnemyWithDelay(delaySeconds, --remainingEnemies);
        }
    }
}
