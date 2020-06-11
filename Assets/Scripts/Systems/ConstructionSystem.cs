using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSystem
{
    PrefabPoolingSystem towerPool;
    PrefabPoolingSystem wallPool;

    //// Public API
    public ConstructionSystem(GameObject _towerPrefab, GameObject _wallPrefab, int poolSize, Transform constructionsParent){
        towerPool = new PrefabPoolingSystem(_towerPrefab, poolSize, constructionsParent);
        wallPool = new PrefabPoolingSystem(_wallPrefab, poolSize, constructionsParent);
    }

    public void OnMouseChange(Vector3 point){
    }
}
