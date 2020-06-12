using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSystem
{
    // Meta
    GameManager _gameManager;

    // Control
    GameManager.InteractionMode _mode;

    // Systems
    PrefabPoolingSystem _towerPool;
    PrefabPoolingSystem _wallPool;

    GameObject _currentStructurePlacement;
    ConstructionBehaviour _currentStructureBehaviour;

    //// Public API
    public ConstructionSystem(GameManager gameManager, GameObject _towerPrefab, GameObject _wallPrefab, int poolSize, Transform constructionsParent){
        this._gameManager = gameManager;
        this._towerPool = new PrefabPoolingSystem(_towerPrefab, poolSize, constructionsParent);
        this._wallPool = new PrefabPoolingSystem(_wallPrefab, poolSize, constructionsParent);

        _mode = _gameManager.Interaction;
    }

    public void OnMouseChange(Vector3 point){
        if(_currentStructurePlacement != null){
            _currentStructurePlacement.transform.LookAt(_gameManager.Core.transform);
            _currentStructurePlacement.transform.position = point;
        }
    }

    public void OnMouseClick(Vector3 point){
        DettachCurrentStructure();
    }

    public void OnPlayerInteractionChanged(GameManager.InteractionMode nextState){
        UpdateCurrentStructure(_mode, nextState);
    }

    //// Private methods
    void DettachCurrentStructure(){
        _currentStructureBehaviour.Activate();
        _currentStructureBehaviour = null;
        
        _currentStructurePlacement = null;
        _gameManager.Interaction = GameManager.InteractionMode.NoSelection;
    }

    void UpdateCurrentStructure(GameManager.InteractionMode currentState, GameManager.InteractionMode nextState){
        if(nextState == GameManager.InteractionMode.NoSelection){
            if(_currentStructurePlacement != null){
                switch(currentState){
                    case GameManager.InteractionMode.TowerSelection:
                        _towerPool.ReturnInstance(_currentStructurePlacement);
                        _currentStructurePlacement = null;
                        _currentStructureBehaviour = null;
                        break;
                    case GameManager.InteractionMode.WallSelection:
                        _wallPool.ReturnInstance(_currentStructurePlacement);
                        _currentStructurePlacement = null;
                        _currentStructureBehaviour = null;
                        break;
                }
            }
            
        }else{
            // Return old
            switch(currentState){ // Important - there is a validation in GameManager Interaction setter that checks if the current and next are the same
                case GameManager.InteractionMode.TowerSelection:
                    _towerPool.ReturnInstance(_currentStructurePlacement);
                    break;

                case GameManager.InteractionMode.WallSelection:
                    _wallPool.ReturnInstance(_currentStructurePlacement);
                    break;
            }

            // Get new
            switch(nextState){
                case GameManager.InteractionMode.TowerSelection:
                    SetBlueprintedCurrentConstrution(_towerPool.GetInstance());
                    break;

                case GameManager.InteractionMode.WallSelection:
                    SetBlueprintedCurrentConstrution(_wallPool.GetInstance());
                    break;
            }
        }
        _mode = nextState;
    }

    void SetBlueprintedCurrentConstrution(GameObject gameObject){
        _currentStructurePlacement = gameObject;
        
        ConstructionBehaviour constructionBehaviour = gameObject.GetComponent<ConstructionBehaviour>();
        constructionBehaviour.ResetToBlueprint();
        _currentStructureBehaviour = constructionBehaviour;
    }
}
