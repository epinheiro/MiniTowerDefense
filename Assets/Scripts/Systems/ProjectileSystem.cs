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
    public ProjectileSystem(GameManager gameManager, GameObject prefab, int poolSize, Transform parent){
        this._gameManager = gameManager;
        this._pool = new PrefabPoolingSystem(prefab, poolSize, parent);
    }

    public GameObject SpawnProjectile(Transform origin, Transform target){
        GameObject go = _pool.GetInstance();
        go.transform.position = origin.position;
        go.GetComponent<ProjectileBehaviour>().SetProjectionAttributes(target, OnProjectileCallback);
        return go;
    }

    //// Private methods
    void OnProjectileCallback(GameObject projectile, GameObject target){
        _pool.ReturnInstance(projectile);
        Debug.Log("DAMAGE! in " + target.gameObject.name); // TODO - use target to process damage!
    }
}
