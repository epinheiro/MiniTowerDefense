using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Meta setup
    public static GameManager Instance;
    GameSetup _gameSetup;
    public int TowerPoolSize{
        get { return _gameSetup.towerPoolSize; }
    }
    public int WallPoolSize{
        get { return _gameSetup.wallPoolSize; }
    }
    public int EnemyPoolSize{
        get { return _gameSetup.enemyPoolSize; }
    }
    public int ProjectilePoolSize{
        get { return _gameSetup.projectilePoolSize; }
    }
    public int CoreTotalLife{
        get { return _gameSetup.coreTotalLife; }
    }
    public float CoreMenaceCheckPeriod{
        get { return _gameSetup.coreMenaceCheckPeriod; }
    }
    public int TowerEnemyLockdownLimit{
        get { return _gameSetup.towerEnemyLockdownLimit; }
    }
    public float TowerMenaceCheckPeriod{
        get { return _gameSetup.towerMenaceCheckPeriod; }
    }
    public float ProjectileVelocity{
        get { return _gameSetup.projectileVelocity; }
    }

    // Enumerators
    public enum GameState{InGame, EndGame}
    public enum InteractionMode {NoSelection, WallSelection, TowerSelection, ConstructionConfirmation, DestructionConfirmation}
    public enum Tags {Enemy, Core, Ground, Forest, UI, Construction, Blueprint, Projectile}

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
    GameState _gameState;
    public GameState State{
        get { return _gameState; }
    }

    // Delegations
    public delegate void PlayerInteractionAction(InteractionMode newState);
    PlayerInteractionAction _interactionChangedListeners;

    // Wave and Enemies related variables
    SpawnSystem _spawn;
    public SpawnSystem Enemies{
        get { return _spawn; }
    }

    [SerializeField]
    GameObject _enemyPrefab = null;

    // Projectile related variables
    ProjectileSystem _projectileSystem;
    public ProjectileSystem Projectiles{
        get { return _projectileSystem; }
    }
    [SerializeField]
    GameObject _projectilePrefab = null;

    // Map related variables
    [SerializeField]
    GameObject _mapObject = null;
    GameObject _core;
    GameObject _spawnPoints;
    public GameObject Core{
        get { return _core; }
    }
    public GameObject SpawnPoints{
        get { return _spawnPoints; }
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
        Instance = this;

        // Meta
        _gameSetup = Resources.Load("Data/GameSetup") as GameSetup;

        // Map
        if(_mapObject == null) SetupErrorMessage("Map game object not linked");
        else{
            _core = _mapObject.transform.Find("Core").gameObject;
            _spawnPoints = _mapObject.transform.Find("SpawnPoints").gameObject;
        }

        if(_inputControllerReference == null) SetupErrorMessage("InputController game object not linked");
        
        // Construction System
        if(_towerPrefab == null) SetupErrorMessage("Tower game object prefab not linked");
        if(_wallPrefab == null) SetupErrorMessage("Wall game object prefab not linked");
        Transform constructionsParent = transform.Find("Constructions");
        _constructionSystem = new ConstructionSystem(_towerPrefab, _wallPrefab, constructionsParent);
        Input.RegisterMouseMovementListener(_constructionSystem.OnMouseChange);
        Input.RegisterMouseClickListener(_constructionSystem.OnMouseClick);

        _interactionChangedListeners += _inputControllerReference.OnPlayerInteractionChanged;
        _interactionChangedListeners += _constructionSystem.OnPlayerInteractionChanged;

        // Projectile system
        if(_projectilePrefab == null) SetupErrorMessage("Projectile game object prefab not linked");
        Transform projectilesParent = transform.Find("Projectiles");
        _projectileSystem = new ProjectileSystem(_projectilePrefab, projectilesParent);

        // Enemy System
        if(_enemyPrefab == null) SetupErrorMessage("Enemy game object prefab not linked");
        Transform enemiesParent = transform.Find("Enemies");
        _spawn = new SpawnSystem(_enemyPrefab, enemiesParent);
    }

    void Start(){
        BeginGameProcedure();
    }

    void Update()
    {

    }

    //// Public API
    public void BeginGameProcedure(){
        _gameState = GameState.InGame;
        _spawn.BeginGame();
    }
    public void EndGameProcedure(string text, string buttonText){
        _gameState = GameState.EndGame;
        // Debug.Log("YOU LOST!"); // TODO - insert debug flag?
        this.StopAllCoroutines();
        Input.SetEndGameLayout(text, buttonText);
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

    public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    //// Private methods
    void OnChangePlayerInteraction(){
        if(_interactionChangedListeners != null) {
            _interactionChangedListeners(Interaction);
        }
    }

    void SetupErrorMessage(string message){
        throw new System.Exception(string.Format("GameManager not correctly setup: {0}", message));
    }
}
