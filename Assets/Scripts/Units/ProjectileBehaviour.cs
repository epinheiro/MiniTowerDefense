using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // Game manager
    GameManager _gameManager;

    // Delegation
    public delegate void TriggerCallback(GameObject projectile, GameObject target);
    TriggerCallback _onTriggerCallback;

    // Control variables
    bool _isActive = false;
    float _velocity;
    Transform _target;
    EnemyBehaviour _targetBehaviour;

    //// MonoBehaviour methods
    void Start(){
        _gameManager = GameManager.Instance;
        _velocity = _gameManager.ProjectileVelocity;
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

    void OnTriggerEnter(Collider collider){
        switch(System.Enum.Parse(typeof(GameManager.Tags), collider.gameObject.tag)){
            case GameManager.Tags.Enemy:
                _onTriggerCallback(this.gameObject, collider.gameObject);
                ResetProjectile();
                break;
        }
    }

    //// Public API
    public void SetProjectionAttributes(Transform target, TriggerCallback callback){
        _target = target;
        _isActive = true;
        _onTriggerCallback = callback;

        _targetBehaviour = target.GetComponent<EnemyBehaviour>();
    }

    //// Private methods
    void ResetProjectile(){
        _target = null;
        _isActive = false;
        _onTriggerCallback = null;
    }
}
