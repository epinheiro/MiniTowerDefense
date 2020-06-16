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
    Vector3 _destination;

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
        _gameManager = GameManager.Instance;
        _coreReference = _gameManager.Core;
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
        InsertAIAgent();
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

    public int EraseEnemy(){
        int remainingLife = this._currentLife;
        ResetEnemy();
        _gameManager.Enemies.ReturnEnemyElement(this.gameObject);

        return remainingLife;
    }

    public void RecalculateRoute(){
        ChangeAgentDestination(_coreReference.transform.position);
    }

    //// Private methods
    void UnpackEnemyAttributes(EnemyAttributes attributes){
        _totalLife = _currentLife = attributes.life;
        _lifeBar.SetUp(_totalLife);

        _aiAgent.speed = attributes.speed;
    }

    void ResetEnemy(){
        _isActive = false;
        Destroy(_aiAgent);
    }

    void InsertAIAgent(){
        _aiAgent = this.gameObject.AddComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        _aiAgent.SetDestination(_destination);
    }

    void ChangeAgentDestination(Vector3 goToPosition){
        _destination = new Vector3(goToPosition.x, 0, goToPosition.z);
    }

    void AIActive(bool isActive){
        _aiAgent.isStopped = !isActive;
    }

    void EnemyLookAt(Transform target){
        this.transform.LookAt(target);
    }
}
