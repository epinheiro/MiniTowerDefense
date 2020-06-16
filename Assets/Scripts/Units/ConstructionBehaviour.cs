using UnityEngine;
using UnityEngine.AI;

public class ConstructionBehaviour : MonoBehaviour
{
    // Resources
    protected Material _blueprintMaterial;
    protected Material _activeMaterial;

    // Components
    protected Renderer _renderer;
    protected CapsuleCollider _collider;
    protected NavMeshObstacle _aiObstacle;

    // Attributes
    ConstructionSystem.GameObjectAction _onClickCallback;

    protected float lifeTime;

    //// MonoBehaviour methods
    protected virtual void Awake(){
        this._renderer = GetComponent<Renderer>();

        this._blueprintMaterial = Resources.Load("Materials/mat_construction_blueprint") as Material;
        this._activeMaterial = Resources.Load("Materials/mat_construction") as Material;

        this._collider = GetComponent<CapsuleCollider>();
        this._aiObstacle = GetComponent<NavMeshObstacle>();
    }

    protected virtual void Start(){
        ResetToBlueprint();
    }

    void OnMouseDown(){
        _onClickCallback(this.gameObject);
    }

    //// Public API
    public virtual void ResetToBlueprint(){
        _renderer.material = _blueprintMaterial;
        _collider.enabled = false;
        _aiObstacle.enabled = false;
        _onClickCallback = null;
    }

    public virtual void Activate(ConstructionSystem.GameObjectAction onClickCallback){
        _renderer.material = _activeMaterial;
        _collider.enabled = true;
        _aiObstacle.enabled = true;
        _onClickCallback = onClickCallback;
    }
}
