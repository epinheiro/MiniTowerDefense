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
    public EnemySystem(GameObject prefab, int poolSize, Transform enemyParent){
        this._gameManager = GameManager.Instance;
        this._pool = new PrefabPoolingSystem(prefab, poolSize, enemyParent);
    }

    public GameObject SpawnEnemyAt(Vector3 position, EnemyAttributes attributes){
        GameObject enemy = _pool.GetInstance();
        enemy.GetComponent<EnemyBehaviour>().SetEnemyAttributes(position, attributes);
        enemy.GetComponent<Renderer>().material.color = attributes.color;
        return enemy;
    }

    public void ReturnEnemyElement(GameObject go){
        _pool.ReturnInstance(go);
    }

    public void MapChanged(){
        List<GameObject> actives = GetEnemiesActive();
        foreach(GameObject go in actives){
            go.GetComponent<EnemyBehaviour>().RecalculateRoute();
        }
    }

    public List<GameObject> GetEnemiesActive(){
        return _pool.GetAllElementsActive();;
    }

    // Pooling methods
    public int GetActiveNumber(){
        return _pool.Used;
    }

    public int GetAvailableElements(){
        return _pool.Available;
    }

    public void EnlargePoolSize(int newElements){
        _pool.EnlargePoolSize(newElements);
    }
}
