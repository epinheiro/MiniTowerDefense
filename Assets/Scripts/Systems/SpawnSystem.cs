using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem
{
    // Enumerators
    public enum SpawnTypes {DelayedIndianLine, LineGroup}

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
            Spawn(5, spawn, SpawnTypes.LineGroup);
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

    void Spawn(int enemyNumber, Vector3 spawn, SpawnTypes mode){
        switch(mode){
            case SpawnTypes.DelayedIndianLine:
                _gameManager.StartCoroutine(SpawnDelayedSingleEnemy(spawn, 1, enemyNumber));
                break;

            case SpawnTypes.LineGroup:
                _gameManager.StartCoroutine(SpawnEnemyLineGroup(spawn, enemyNumber));
                break;
        }
    }

    //// Coroutines
    IEnumerator SpawnDelayedSingleEnemy(Vector3 position, float delaySeconds, int remainingEnemies){
        if(remainingEnemies > 0){
            bool worked = false;
            try{
                _enemySystem.SpawnEnemyAt(position);
                worked = true;
            }catch(System.Exception){}

            yield return new WaitForSecondsRealtime(delaySeconds);

            if(worked){ 
                yield return SpawnDelayedSingleEnemy(position, delaySeconds, --remainingEnemies);
            }else{ // Wait to be possible to instantiate more enemies
                yield return SpawnDelayedSingleEnemy(position, delaySeconds, remainingEnemies);
            }
        }
    }

    IEnumerator SpawnEnemyLineGroup(Vector3 position, int enemyNumber){
        if(enemyNumber > 0){
            int enemiesSpawned = 0;
            
            try{
                PositionEnemyLine(position, enemyNumber, ref enemiesSpawned);

            }catch(System.Exception){}

            yield return new WaitForSecondsRealtime(.1f);

            if(enemiesSpawned < enemyNumber){ // If pool was insuficient to group spawn, try to delayed single
                yield return SpawnDelayedSingleEnemy(position, 1, enemyNumber - enemiesSpawned);
            }
        }
    }

    void PositionEnemyLine(Vector3 initialPosition, int enemyNumber, ref int enemiesSpawned ){
        Vector3 spawnPoint;

        Vector3 destiny = _gameManager.Core.transform.position;
        Vector3 diff = initialPosition - destiny;

        for(int i=0; i<enemyNumber; i++){
            float modifier = (enemiesSpawned%2==0 ? -1 : 1) * enemiesSpawned;

            if(diff.x == 0){
                float modHorizontal = diff.z > 0 ? 1 : (diff.z == 0 ? 0 : -1);
                spawnPoint = new Vector3(initialPosition.x + modifier * modHorizontal, 0, initialPosition.z);
            }else{
                if(diff.z == 0){
                    float modVertical   = diff.x > 0 ? 1 : (diff.x == 0 ? 0 : -1);
                    spawnPoint = new Vector3(initialPosition.x, 0, initialPosition.z + modifier * modVertical);
                }else{
                    if(Mathf.Sign(diff.z) == Mathf.Sign(diff.x)){
                        spawnPoint = new Vector3((initialPosition.x + modifier), 0, (initialPosition.z + -modifier));
                    }else{
                        spawnPoint = new Vector3((initialPosition.x + modifier), 0, (initialPosition.z + modifier));
                    }
                }
            }

            _enemySystem.SpawnEnemyAt(spawnPoint);

            enemiesSpawned++;
        }
    }
}
