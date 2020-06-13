﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // Delegation
    public delegate void CollisionCallback(GameObject projectile, GameObject target);
    CollisionCallback _onCollisionCallback;

    // Control variables
    bool isActive = false;
    [Range(0, 100)] float velocity = 5f;
    Transform _target;

    //// MonoBehaviour methods
    void Update(){
        if(isActive){
            this.transform.position = Vector3.MoveTowards(transform.position, _target.position, velocity * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision){
        switch(System.Enum.Parse(typeof(GameManager.Tags), collision.gameObject.tag)){
            case GameManager.Tags.Enemy:
                _onCollisionCallback(this.gameObject, collision.gameObject);
                ResetProjectile();
                // TODO - Return to pool
                break;
        }
    }

    //// Public API
    public void SetProjectionAttributes(Transform target, CollisionCallback callback){
        _target = target;
        isActive = true;
        _onCollisionCallback = callback;
    }

    //// Private methods
    void ResetProjectile(){
        _target = null;
        isActive = false;
        _onCollisionCallback = null;
    }
}
