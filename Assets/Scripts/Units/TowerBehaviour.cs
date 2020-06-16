using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : ConstructionBehaviour
{
    // Game manager
    GameManager _gameManager;
    
    // Balance variables
    float _currentTimeCount = 0;
    float _secondsToCheckMenace;
    int _lockdownEnemiesLimit;

    // Enemy related attributes
    List<GameObject> _menaces;

    // Components
    protected SphereCollider _perceptionTrigger;

    //// MonoBehaviour methods
    protected override void Awake(){
        base.Awake();

        _menaces = new List<GameObject>();
        _perceptionTrigger = this.GetComponent<SphereCollider>();
    }

    protected override void Start(){
        base.Start();
        _gameManager = GameManager.Instance;

        _secondsToCheckMenace = _gameManager.TowerMenaceCheckPeriod;
        _lockdownEnemiesLimit = _gameManager.TowerEnemyLockdownLimit;
    }

    void Update(){
        if(_gameManager.State == GameManager.GameState.InGame){
            if(_menaces.Count > 0 && _currentTimeCount >= _secondsToCheckMenace){
                _currentTimeCount = 0;
                List<GameObject> menacesLost = new List<GameObject>();
                
                int count = 0;

                for(int i=0; i<_menaces.Count; i++){
                    if(_menaces[i].GetComponent<EnemyBehaviour>().Active){
                        if(count < _lockdownEnemiesLimit){
                        //Debug.Log(string.Format("SHOOT {0}", _menaces[i])); // TODO - debug print
                        _gameManager.Projectiles.SpawnProjectile(this.transform, _menaces[i].transform);
                        count++;
                        }else{
                            break;
                        }
                    }else{
                        menacesLost.Add(_menaces[i]);
                    }
                }

                foreach(GameObject menace in menacesLost){
                    _menaces.Remove(menace);
                }
            }
            _currentTimeCount += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other){
        switch(System.Enum.Parse(typeof(GameManager.Tags), other.tag)){
            case GameManager.Tags.Enemy:
                _menaces.Add(other.gameObject);
                break;
        }
    }

    void OnTriggerExit(Collider other){
        switch(System.Enum.Parse(typeof(GameManager.Tags), other.tag)){
            case GameManager.Tags.Enemy:
                _menaces.Remove(other.gameObject);
                break;
        }
    }

    //// Public API
    public override void ResetToBlueprint(){
        base.ResetToBlueprint();
        _perceptionTrigger.enabled = false;
    }

    public override void Activate(ConstructionSystem.GameObjectAction callback){
        base.Activate(callback);
        _perceptionTrigger.enabled = true;
    }
}
