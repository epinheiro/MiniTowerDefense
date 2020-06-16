using UnityEngine;

public class ConstructionSystem
{
    // Meta
    GameManager _gameManager;
    enum Type {Tower, Wall}

    // Delegation
    public delegate void GameObjectAction(GameObject gameObject);

    // Control
    GameManager.InteractionMode _mode;

    // Systems
    PrefabPoolingSystem _towerPool;
    PrefabPoolingSystem _wallPool;

    // Structure controls
    GameObject _currentStructurePlacement;
    ConstructionBehaviour _currentStructureBehaviour;

    //// Public API
    public ConstructionSystem(Transform constructionsParent){
        GameObject towerPrefab = Resources.Load("Prefabs/Tower") as GameObject;
        GameObject wallPrefab = Resources.Load("Prefabs/Wall") as GameObject;
        this._gameManager = GameManager.Instance;
        this._towerPool = new PrefabPoolingSystem(towerPrefab, _gameManager.TowerPoolSize, constructionsParent);
        this._wallPool = new PrefabPoolingSystem(wallPrefab, _gameManager.WallPoolSize, constructionsParent);

        _mode = _gameManager.Interaction;
    }

    // Public callbacks
    public void OnMouseChange(Vector3 point){
        if(_currentStructurePlacement != null){
            _currentStructurePlacement.transform.LookAt(_gameManager.Core.transform);
            _currentStructurePlacement.transform.position = point;
        }
    }

    public void OnMouseClick(Vector3 point){
        switch(_mode){
            case GameManager.InteractionMode.TowerSelection:
            case GameManager.InteractionMode.WallSelection:
                DettachCurrentStructure();
                break;
        }
    }

    public void OnPlayerInteractionChanged(GameManager.InteractionMode nextState){
        UpdateCurrentStructure(_mode, nextState);
    }

    //// Private methods
    void DettachCurrentStructure(){
        _currentStructurePlacement = null;
        _gameManager.Interaction = GameManager.InteractionMode.ConstructionConfirmation;
        SetConstructionConfirmationLayout();
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
                    if(nextState != GameManager.InteractionMode.ConstructionConfirmation){
                        _towerPool.ReturnInstance(_currentStructurePlacement);
                    }
                    break;

                case GameManager.InteractionMode.WallSelection:
                    if(nextState != GameManager.InteractionMode.ConstructionConfirmation){
                        _wallPool.ReturnInstance(_currentStructurePlacement);
                    }
                    break;

                case GameManager.InteractionMode.ConstructionConfirmation:
                    ReturnConstructionToPool(_currentStructureBehaviour.gameObject);
                    _gameManager.Input.SetInGameLayout();
                    break;

                case GameManager.InteractionMode.DestructionConfirmation:
                    _gameManager.Input.SetInGameLayout();
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

    void ConstructConfirmationCount(ConstructionBehaviour script){
        if(script.GetType() == typeof(TowerBehaviour)){
            if(_towerPool.Available <= 0){
                _gameManager.Input.SetVisibilityTowerButton(false);
            }
        }else{ // WallBehaviour
            if(_wallPool.Available <= 0){
                _gameManager.Input.SetVisibilityWallButton(false);
            }
        }
    }

    void DestructionConfirmationCount(Type type){
        switch(type){
            case Type.Tower:
                if(_towerPool.Available > 0){
                    _gameManager.Input.SetVisibilityTowerButton(true);
                }
                break;
            case Type.Wall:
                if(_wallPool.Available > 0){
                    _gameManager.Input.SetVisibilityWallButton(true);
                }
                break;
        }
    }

    // SetLayout
    void SetConstructionConfirmationLayout(){
        _gameManager.Input.SetConstructionPopupLayout(
            "Construct here?",
            "Construct", OnConstructionConfirmation,
            "Cancel", OnConstructionCancelation
        );
    }

    void SetDestructionLayout(GameObject gameObject){
        _gameManager.Input.SetConstructionPopupLayout(
            "Destroy this?",
            "Destroy", () => {OnDestroyConfirmation(gameObject);},
            "Cancel", OnDestroyCancelation
        );
    }

    Type ReturnConstructionToPool(GameObject gameObject){
        try{
            _towerPool.ReturnInstance(gameObject);
            return Type.Tower;
        }catch(System.Exception){
            _wallPool.ReturnInstance(gameObject);
            return Type.Wall;
        }
    }

    // Callbacks
    void OnConstructionConfirmation(){
        ConstructConfirmationCount(_currentStructureBehaviour);

        _currentStructureBehaviour.Activate(OnStructureClick);
        _currentStructureBehaviour = null;
        _gameManager.Interaction = GameManager.InteractionMode.NoSelection;
        _gameManager.Input.SetInGameLayout();

        _gameManager.Enemies.MapChanged();
    }

    void OnConstructionCancelation(){
        ReturnConstructionToPool(_currentStructureBehaviour.gameObject);
        
        _currentStructureBehaviour = null;
        _gameManager.Interaction = GameManager.InteractionMode.NoSelection;
        _gameManager.Input.SetInGameLayout();
    }

    void OnDestroyConfirmation(GameObject gameObject){
        Type constructionType = ReturnConstructionToPool(gameObject);

        DestructionConfirmationCount(constructionType);

        _currentStructureBehaviour = null;
        _gameManager.Interaction = GameManager.InteractionMode.NoSelection;

        _gameManager.Input.SetInGameLayout();

        _gameManager.Enemies.MapChanged();
    }

    void OnDestroyCancelation(){
        _gameManager.Input.SetInGameLayout();
    }

    void OnStructureClick(GameObject gameObjectClicked){
        if(_gameManager.State == GameManager.GameState.InGame){
            _gameManager.Interaction = GameManager.InteractionMode.DestructionConfirmation;
            SetDestructionLayout(gameObjectClicked);
        }
    }
}
