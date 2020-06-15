using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem
{
    // Meta
    GameManager _gameManager;
    
    // Systems
    PrefabPoolingSystem _pool;
    
    //// Public API
    public ProjectileSystem(GameObject prefab, Transform parent){
        this._gameManager = GameManager.Instance;
        this._pool = new PrefabPoolingSystem(prefab, _gameManager.ProjectilePoolSize, parent);
    }

    public GameObject SpawnProjectile(Transform origin, Transform target){
        GameObject go;
        try{
            go = _pool.GetInstance();
        }catch(System.Exception){
            _pool.EnlargePoolSize((_gameManager.ProjectilePoolSize/2)+1);
            go = _pool.GetInstance();
        }
        
        go.transform.position = origin.position;
        go.GetComponent<ProjectileBehaviour>().SetProjectionAttributes(target, OnProjectileCallback);
        return go;
    }

    public void ReturnProjectile(GameObject go){
        _pool.ReturnInstance(go);
    }

    //// Private methods
    void OnProjectileCallback(GameObject projectile, GameObject target){
        _pool.ReturnInstance(projectile);
        target.GetComponent<EnemyBehaviour>().EnemyHit();
    }
}
