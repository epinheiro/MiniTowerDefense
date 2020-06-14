using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    // Game manager
    GameManager _gameManager;

    // Control
    bool _isActive = false;
    public bool Active{
        get { return _isActive; }
    }
    UILifeBar _lifeBar;

    // Attributes
    GameObject _coreReference;
    NavMeshAgent _aiAgent;
    int _totalLife;
    int _currentLife;
    int CurrentLife{
        get { return _currentLife; }
        set {
            _currentLife = value;
            _lifeBar.HardSetValue(_currentLife);
        }
    }


    void Awake(){
        _aiAgent = this.GetComponent<NavMeshAgent>();
        _lifeBar = this.transform.Find("LifeBar").GetComponent<UILifeBar>();
    }

    //// MonoBehaviour methods
    void Start(){
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();  // TODO - is it better another access method?
        _coreReference = _gameManager.Core; 
        ChangeAgentDestination(_coreReference.transform.position);
    }

    void OnTriggerEnter(Collider other){
        switch(System.Enum.Parse(typeof(GameManager.Tags), other.tag)){
            case GameManager.Tags.Core:
                ChangeAgentDestination(this.transform.position);
                EnemyLookAt(_coreReference.transform);
                break;
        }
    }

    //// Public API
    public void SetEnemyAttributes(Vector3 spawnPoint, EnemyAttributes attributes){
        _isActive = true;
        this.transform.position = spawnPoint;
        UnpackEnemyAttributes(attributes);
    }

    public void EnemyHit(){
        // Debug.Log("DAMAGE! in " + this.name); // TODO - delete debug print
        _currentLife--;
        if(_currentLife <= 0 ){
            _gameManager.Enemies.ReturnEnemyElement(this.gameObject);
            ResetEnemy();
        }else{
            _lifeBar.ChangeValueIn(-1); // CHECK - Hardcoded damage value
        }
    }


    //// Private methods
    void UnpackEnemyAttributes(EnemyAttributes attributes){
        _totalLife = _currentLife = attributes.life;
        _lifeBar.SetUp(_totalLife);

        _aiAgent.speed = attributes.speed;
    }

    void ResetEnemy(){
        _isActive = false;
    }

    void ChangeAgentDestination(Vector3 goToPosition){
        _aiAgent.destination = goToPosition; 
    }

    void EnemyLookAt(Transform target){
        this.transform.LookAt(target);
    }
}
