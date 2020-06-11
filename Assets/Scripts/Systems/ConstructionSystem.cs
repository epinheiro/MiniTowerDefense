using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSystem
{
    GameManager _gameManager;
    PrefabPoolingSystem _towerPool;
    PrefabPoolingSystem _wallPool;

    GameObject _currentStructurePlacement;

    //// Public API
    public ConstructionSystem(GameManager gameManager, GameObject _towerPrefab, GameObject _wallPrefab, int poolSize, Transform constructionsParent){
        this._gameManager = gameManager;
        this._towerPool = new PrefabPoolingSystem(_towerPrefab, poolSize, constructionsParent);
        this._wallPool = new PrefabPoolingSystem(_wallPrefab, poolSize, constructionsParent);
    }

    public void OnMouseChange(Vector3 point){
        if(_currentStructurePlacement != null){
            _currentStructurePlacement.transform.LookAt(_gameManager.Core.transform);
            _currentStructurePlacement.transform.position = point;
        }
    }

    public void OnMouseClick(Vector3 point){
        _currentStructurePlacement = null;
    }
}
