using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // Game manager
    GameManager _gameManager;

    // Delegation
    public delegate void CollisionCallback(GameObject projectile, GameObject target);
    CollisionCallback _onCollisionCallback;

    // Control variables
    bool _isActive = false;
    float _velocity = 5f; // CHECK - Hardcoded velocity
    Transform _target;
    EnemyBehaviour _targetBehaviour;

    //// MonoBehaviour methods
    void Start(){
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();  // TODO - is it better another access method?
    }

    void Update(){
        if(_isActive){
            if(_targetBehaviour.Active){
                this.transform.position = Vector3.MoveTowards(transform.position, _target.position, _velocity * Time.deltaTime);
            }else{
                _gameManager.Projectiles.ReturnProjectile(this.gameObject);
                ResetProjectile();
            }
        }
    }

    void OnCollisionEnter(Collision collision){
        switch(System.Enum.Parse(typeof(GameManager.Tags), collision.gameObject.tag)){
            case GameManager.Tags.Enemy:
                _onCollisionCallback(this.gameObject, collision.gameObject);
                ResetProjectile();
                break;
        }
    }

    //// Public API
    public void SetProjectionAttributes(Transform target, CollisionCallback callback){
        _target = target;
        _isActive = true;
        _onCollisionCallback = callback;

        _targetBehaviour = target.GetComponent<EnemyBehaviour>();
    }

    //// Private methods
    void ResetProjectile(){
        _target = null;
        _isActive = false;
        _onCollisionCallback = null;
    }
}
