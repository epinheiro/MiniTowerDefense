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
    List<Vector3> _spawnPointList;
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

        PrepareSpawnPointList();

        // Debug calls
        foreach(Vector3 spawn in _spawnPointList){
            _gameManager.StartCoroutine(SpawnEnemyWithDelay(spawn, 1, 5)); // TODO - Futurely DELETE this call
        }
    }

    public void ReturnEnemyElement(GameObject go){
        _enemySystem.ReturnEnemyElement(go);
    }

    //// Private methods
    void PrepareSpawnPointList(){
        _spawnPointList = new List<Vector3>();
        Transform spawnPoints = _gameManager.SpawnPoints.transform;
        for(int i=0; i<spawnPoints.childCount; i++){
            _spawnPointList.Add(spawnPoints.GetChild(i).transform.position);
        }
    }

    void OnWaveNumberChange(){
    }

    //// Coroutines
    IEnumerator SpawnEnemyWithDelay(Vector3 position, float delaySeconds, int remainingEnemies){
        if(remainingEnemies > 0){
            bool worked = false;
            try{
                _enemySystem.SpawnEnemyAt(position);
                worked = true;
            }catch(System.Exception){}

            yield return new WaitForSecondsRealtime(delaySeconds);

            if(worked){ 
                yield return SpawnEnemyWithDelay(position, delaySeconds, --remainingEnemies);
            }else{ // Wait to be possible to instantiate more enemies
                yield return SpawnEnemyWithDelay(position, delaySeconds, remainingEnemies);
            }
        }
    }
}
