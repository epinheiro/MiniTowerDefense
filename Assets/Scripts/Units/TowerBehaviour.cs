using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : ConstructionBehaviour
{
    // Balance variables
    readonly float _secondsToCheckMenace = 0.5f;
    readonly int _lockdownEnemiesLimit = 2;

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
        this.StartCoroutine(CheckMenacesCoroutine());
    }

    void Update(){

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

    //// Coroutines
    IEnumerator CheckMenacesCoroutine(){
        while(true){
            if(_menaces.Count > 0){
                int count = 0;
                for(int i=0; i<_menaces.Count; i++){
                    if(count <= _lockdownEnemiesLimit){
                        Debug.Log(string.Format("SHOOT {0}", _menaces[i])); // TODO - change to actual projectile call
                        count++;
                    }else{
                        break;
                    }
                }
            }
            yield return new WaitForSecondsRealtime(_secondsToCheckMenace);
        }
    }

    //// Public API
    public override void ResetToBlueprint(){
        base.ResetToBlueprint();
        _perceptionTrigger.enabled = false;
    }

    public override void Activate(){
        base.Activate();
        _perceptionTrigger.enabled = true;
    }
}
