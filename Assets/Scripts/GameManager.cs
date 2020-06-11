using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Enumerators
    public enum PlayerInteraction {NoSelection, WallSelection, TowerSelection}
    public enum Tags {Enemy, Core}

    // Main game controller
    PlayerInteraction _lastPlayerInteraction = PlayerInteraction.NoSelection;
    PlayerInteraction _currentPlayerInteraction = PlayerInteraction.NoSelection;
    public PlayerInteraction Interaction{
        get { return _currentPlayerInteraction; }
        set {
            if(_currentPlayerInteraction != value){
                _currentPlayerInteraction = value;
                OnChangePlayerInteraction();
            }
        }
    }

    // Delegations
    public delegate void PlayerInteractionAction(PlayerInteraction newState);
    PlayerInteractionAction _interactionChangedListeners;

    // Wave and Enemies related variables
    Transform enemiesParent;

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
    UIController _uiControllerReference = null;
    public UIController UI{
        get { return _uiControllerReference; }
    }

    // Construction
    Transform constructionsParent;
    [SerializeField]
    GameObject _towerPrefab = null;
    [SerializeField]
    GameObject _wallPrefab = null;


    //// MonoBehaviour methods
    void Awake(){
        if(_coreGameObject == null) SetupErrorMessage("Core game object not linked");
        if(_uiControllerReference == null) SetupErrorMessage("UIController game object not linked");
        if(_towerPrefab == null) SetupErrorMessage("Tower game object prefab not linked");
        if(_wallPrefab == null) SetupErrorMessage("Wall game object prefab not linked");

        constructionsParent = transform.Find("Constructions");
        enemiesParent = transform.Find("Enemies");

        _interactionChangedListeners += _uiControllerReference.OnPlayerInteractionChanged;
    }

    void Start(){
    }

    void Update()
    {

    }

    //// Public click callbacks
    public void PlayerInteractionClicked(PlayerInteraction newInteraction){
        if(newInteraction == Interaction){
            _lastPlayerInteraction = Interaction;
            Interaction = PlayerInteraction.NoSelection;
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
