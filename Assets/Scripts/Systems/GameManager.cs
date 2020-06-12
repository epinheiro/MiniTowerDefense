using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Meta setup
    readonly int _constructionPoolSize = 5;
    readonly int _enemyPoolSize = 20;

    // Enumerators
    public enum InteractionMode {NoSelection, WallSelection, TowerSelection}
    public enum Tags {Enemy, Core, Ground, Forest, UI, Construction, Blueprint}

    // Main game controller
    InteractionMode _lastPlayerInteraction = InteractionMode.NoSelection;
    InteractionMode _currentPlayerInteraction = InteractionMode.NoSelection;
    public InteractionMode Interaction{
        get { return _currentPlayerInteraction; }
        set {
            if(_currentPlayerInteraction != value){
                _currentPlayerInteraction = value;
                OnChangePlayerInteraction();
            }
        }
    }

    // Delegations
    public delegate void PlayerInteractionAction(InteractionMode newState);
    PlayerInteractionAction _interactionChangedListeners;

    // Wave and Enemies related variables
    EnemySystem _enemySystem;
    public EnemySystem Enemies{
        get { return _enemySystem; }
    }

    [SerializeField]
    GameObject _enemyPrefab = null;

    public readonly int totalWaves = 5;

    int _currentWave = 0;

    public int Wave{
        get { return _currentWave; }
        set {
            _currentWave = value;
            OnWaveNumberChange();
        }
    }

    // Core related variables
    [SerializeField]
    GameObject _coreGameObject = null;
    public GameObject Core{
        get { return _coreGameObject; }
    }

    // UI related variables
    [SerializeField]
    InputController _inputControllerReference = null;
    public InputController Input{
        get { return _inputControllerReference; }
    }

    // Construction
    ConstructionSystem _constructionSystem;
    [SerializeField]
    GameObject _towerPrefab = null;
    [SerializeField]
    GameObject _wallPrefab = null;


    //// MonoBehaviour methods
    void Awake(){
        if(_coreGameObject == null) SetupErrorMessage("Core game object not linked");
        if(_inputControllerReference == null) SetupErrorMessage("UIController game object not linked");
        
        // Construction System
        if(_towerPrefab == null) SetupErrorMessage("Tower game object prefab not linked");
        if(_wallPrefab == null) SetupErrorMessage("Wall game object prefab not linked");
        Transform constructionsParent = transform.Find("Constructions");
        _constructionSystem = new ConstructionSystem(this, _towerPrefab, _wallPrefab, _constructionPoolSize, constructionsParent);
        Input.RegisterMouseMovementListener(_constructionSystem.OnMouseChange);
        Input.RegisterMouseClickListener(_constructionSystem.OnMouseClick);

        _interactionChangedListeners += _inputControllerReference.OnPlayerInteractionChanged;
        _interactionChangedListeners += _constructionSystem.OnPlayerInteractionChanged;

        // Enemy System
        if(_enemyPrefab == null) SetupErrorMessage("Enemy game object prefab not linked");
        Transform enemiesParent = transform.Find("Enemies");
        _enemySystem = new EnemySystem(this, _enemyPrefab, _enemyPoolSize, enemiesParent);
    }

    void Start(){
    }

    void Update()
    {

    }

    //// Public click callbacks
    public void PlayerInteractionClicked(InteractionMode newInteraction){
        if(newInteraction == Interaction){
            _lastPlayerInteraction = Interaction;
            Interaction = InteractionMode.NoSelection;
        }else{
            _lastPlayerInteraction = Interaction;
            Interaction = newInteraction;
        }
    }

    //// Private methods
    void OnWaveNumberChange(){

    }

    void OnChangePlayerInteraction(){
        if(_interactionChangedListeners != null) {
            _interactionChangedListeners(Interaction);
        }
    }

    void SetupErrorMessage(string message){
        throw new System.Exception(string.Format("GameManager not correctly setup: {0}", message));
    }
}
