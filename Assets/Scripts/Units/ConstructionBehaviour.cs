using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConstructionBehaviour : MonoBehaviour
{
    // Resources
    Material _blueprintMaterial;
    Material _activeMaterial;

    // Components
    Renderer _renderer;
    CapsuleCollider _collider;
    NavMeshObstacle _aiObstacle;


    protected float lifeTime;

    //// MonoBehaviour methods
    void Awake(){
        this._renderer = GetComponent<Renderer>();

        this._blueprintMaterial = Resources.Load("Materials/mat_construction_blueprint") as Material;
        this._activeMaterial = Resources.Load("Materials/mat_construction") as Material;

        this._collider = GetComponent<CapsuleCollider>();
        this._aiObstacle = GetComponent<NavMeshObstacle>();

        ResetToBlueprint();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    //// Public API
    public void ResetToBlueprint(){
        _renderer.material = _blueprintMaterial;
        _collider.enabled = false;
        _aiObstacle.enabled = false;
    }

    public void Activate(){
        _renderer.material = _activeMaterial;
        _collider.enabled = true;
        _aiObstacle.enabled = true;
    }
}
