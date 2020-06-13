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

    // Attributes
    GameObject _coreReference;
    NavMeshAgent _aiAgent;


    void Awake(){
        _aiAgent = this.GetComponent<NavMeshAgent>();
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
    public void SetEnemyAttributes(Vector3 spawnPoint){
        _isActive = true;
        this.transform.position = spawnPoint;
    }

    public void EnemyHit(){
        Debug.Log("DAMAGE! in " + this.name);
        // TODO - proper DAMAGE calculation
        _gameManager.Enemies.ReturnEnemyElement(this.gameObject);
        ResetEnemy();
    }


    //// Private methods
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
