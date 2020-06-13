using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem
{
    // Game manager
    GameManager _gameManager;

    // Enemies
    EnemySystem _enemySystem;

    // Wave related variables
    public readonly int totalWaves = 5;
    int _currentWave = 0;
    public int Wave{
        get { return _currentWave; }
        set {
            _currentWave = value;
            OnWaveNumberChange();
        }
    }
    
    //// Public API
    public SpawnSystem(GameManager gameManager, GameObject enemyPrefab, int poolSize, Transform enemyParent){
        this._gameManager = gameManager;
        _enemySystem = new EnemySystem(gameManager, enemyPrefab, poolSize, enemyParent);

        // Debug calls
        _gameManager.StartCoroutine(SpawnEnemyWithDelay(1, 5)); // TODO - Futurely DELETE this call
    }

    public void ReturnEnemyElement(GameObject go){
        _enemySystem.ReturnEnemyElement(go);
    }

    //// Private methods
    void OnWaveNumberChange(){
    }

    //// Coroutines
    IEnumerator SpawnEnemyWithDelay(float delaySeconds, int remainingEnemies){
        if(remainingEnemies > 0){
            _enemySystem.SpawnEnemyAt(GameObject.Find("EnemySpawnPoint-TEST").transform.position); // TODO - change this placeholder spawnpoint
            yield return new WaitForSecondsRealtime(delaySeconds);
            yield return SpawnEnemyWithDelay(delaySeconds, --remainingEnemies);
        }
    }
}
