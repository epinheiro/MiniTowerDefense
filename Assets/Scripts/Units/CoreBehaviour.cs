using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBehaviour : MonoBehaviour
{
    // Game Manager
    GameManager _gameManager;
    SpawnSystem _spawnSystem;

    // Balance variables
    float _currentTimeCount = 0;
    float _secondsToCheckMenace;

    // Core gameplay attributes
    int _coreTotalLife;
    int _currentLife;
    int CurrentLife{
        get { return _currentLife; }
        set {
            _currentLife = value;
            _lifeBar.HardSetValue(_currentLife);
        }
    }
    UILifeBar _lifeBar;

    
    // Enemy related attributes
    List<EnemyBehaviour> _menaces;

    //// MonoBehaviour methods
    void Awake(){
        _menaces = new List<EnemyBehaviour>();
        _lifeBar = this.transform.Find("LifeBar").GetComponent<UILifeBar>();
    }

void Start(){
        _gameManager = GameManager.Instance;

        _spawnSystem = _gameManager.Enemies;

        _coreTotalLife = _gameManager.CoreTotalLife;
        _currentLife = _coreTotalLife;
        _secondsToCheckMenace = _gameManager.CoreMenaceCheckPeriod;
        _lifeBar.SetUp(_coreTotalLife);
    }

    void Update(){
        if(CurrentLife <= 0){
            Debug.Log("INFO - Defeat");
            _gameManager.EndGameProcedure("Game Over", "Try again");
        }else{
            if(_currentTimeCount >= _secondsToCheckMenace){
                _currentTimeCount = 0;
                CheckForMenaces();
                if(_menaces.Count > 0){
                    TakeDamageFromManaces();
                }
            }
        }
        _currentTimeCount += Time.deltaTime;
    }

    //// Private methods
    void CheckForMenaces(){
        List<GameObject> activeEnemies = _spawnSystem.GetActiveEnemiesGameObjects();
        foreach(GameObject enemy in activeEnemies){
            EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
            if(Vector3.Distance(this.transform.position, enemy.transform.position) <= 1.5f){ // CHECK - hardcoded distance
                _menaces.Add(enemyBehaviour);
            }else{
                try{
                    _menaces.Remove(enemyBehaviour);
                }catch(System.Exception){}
            }
        }
    }

    void TakeDamageFromManaces(){
        CurrentLife -= ClearMenaceList(); // CHECK - Hardcoded damage (1 to each enemy life left)
    }

    int ClearMenaceList(){
        int purgedEnemyLifeLeft = 0;

        foreach(EnemyBehaviour eb in _menaces){
            purgedEnemyLifeLeft += eb.EraseEnemy();
        }

        _menaces.Clear();

        return purgedEnemyLifeLeft;
    }
}
