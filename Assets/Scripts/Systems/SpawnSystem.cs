using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem
{
    // Enumerators
    public enum SpawnTypes {DelayedIndianLine, LineGroup}

    // Game manager
    GameManager _gameManager;

    // Data
    GameWaveDefinition _gameWaveDefinition;
    WaveData[] _waves;

    Dictionary<EnemyWave.Type, EnemyAttributes> enemyTypesDict;

    // Enemies
    EnemySystem _enemySystem;

    // Wave related variables
    List<Vector3> _spawnPointList;
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

        LoadDataFiles();

        _gameManager.StartCoroutine(BeginSpawns());
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

    void LoadDataFiles(){
        _gameWaveDefinition = Resources.Load("Data/Waves/Game") as GameWaveDefinition;
        _waves = _gameWaveDefinition.waves;

        enemyTypesDict = new Dictionary<EnemyWave.Type, EnemyAttributes>();
        enemyTypesDict.Add(EnemyWave.Type.Fast, Resources.Load("Data/Units/Enemy1") as EnemyAttributes);
        enemyTypesDict.Add(EnemyWave.Type.Strong, Resources.Load("Data/Units/Enemy2") as EnemyAttributes);
    }

    void OnWaveNumberChange(){
    }

    void Spawn(int enemyNumber, Vector3 spawn, SpawnTypes mode, EnemyAttributes attributes){
        switch(mode){
            case SpawnTypes.DelayedIndianLine:
                _gameManager.StartCoroutine(SpawnDelayedSingleEnemy(spawn, 1, enemyNumber, attributes));
                break;

            case SpawnTypes.LineGroup:
                _gameManager.StartCoroutine(SpawnEnemyLineGroup(spawn, enemyNumber, attributes));
                break;
        }
    }

    //// Coroutines
    IEnumerator BeginSpawns(){
        while(_currentWave < _waves.Length){
            WaveData wave = _waves[_currentWave];

            float totalTime = 0;

            foreach(EnemyWave subWave in wave.enemySubWaves){
                totalTime += subWave.timeUntilNextSubWave;
            }

            Debug.Log(string.Format("INFO - Beggining wave {0} for {1} seconds", _currentWave, totalTime));

            foreach(EnemyWave subWave in wave.enemySubWaves){
                EnemyWave.Type expectedType = subWave.enemyType;

                EnemyAttributes value;
                enemyTypesDict.TryGetValue(expectedType, out value);

                int quantity = subWave.quantity;
                SpawnTypes formation = subWave.formation;
                float timeLimit = subWave.timeUntilNextSubWave;

                Debug.Log(string.Format("INFO - SubWave {0} type {1} in {2} formation for {3} seconds", quantity, expectedType, formation, timeLimit));

                Spawn(quantity, _spawnPointList[Random.Range(0, _spawnPointList.Count)], formation, value);
                yield return new WaitForSecondsRealtime(timeLimit);
            }
            Wave++;
        }

        Debug.Log("End spawns!");

        yield return new WaitUntil(() => _enemySystem.GetActiveNumber() == 0);
        _gameManager.EndGameProcedure("Victory!", "Play again?");
    }

    IEnumerator SpawnDelayedSingleEnemy(Vector3 position, float delaySeconds, int remainingEnemies, EnemyAttributes attributes){
        if(remainingEnemies > 0){
            bool worked = false;
            try{
                _enemySystem.SpawnEnemyAt(position, attributes);
                worked = true;
            }catch(System.Exception){}

            yield return new WaitForSecondsRealtime(delaySeconds);

            if(worked){ 
                yield return SpawnDelayedSingleEnemy(position, delaySeconds, --remainingEnemies, attributes);
            }else{ // Wait to be possible to instantiate more enemies
                yield return SpawnDelayedSingleEnemy(position, delaySeconds, remainingEnemies, attributes);
            }
        }
    }

    IEnumerator SpawnEnemyLineGroup(Vector3 position, int enemyNumber, EnemyAttributes attributes){
        if(enemyNumber > 0){
            int enemiesSpawned = 0;
            
            try{
                PositionEnemyLine(position, enemyNumber, ref enemiesSpawned, attributes);

            }catch(System.Exception){}

            yield return new WaitForSecondsRealtime(.1f);

            if(enemiesSpawned < enemyNumber){ // If pool was insuficient to group spawn, try to delayed single
                yield return SpawnDelayedSingleEnemy(position, 1, enemyNumber - enemiesSpawned, attributes);
            }
        }
    }

    void PositionEnemyLine(Vector3 initialPosition, int enemyNumber, ref int enemiesSpawned, EnemyAttributes attributes){
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

            _enemySystem.SpawnEnemyAt(spawnPoint, attributes);

            enemiesSpawned++;
        }
    }
}
