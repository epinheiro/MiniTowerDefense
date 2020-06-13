using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBehaviour : MonoBehaviour
{
    // Balance variables
    float _secondsToCheckMenace = 2;

    // Core gameplay attributes
    float _coreTotalLife = 1;

    
    // Enemy related attributes
    List<EnemyBehaviour> _menaces;

    //// MonoBehaviour methods
    void Awake(){
        _menaces = new List<EnemyBehaviour>();
    }

    void Start()
    {
        this.StartCoroutine(CheckMenacesCoroutine(_secondsToCheckMenace));
    }

    void Update()
    {
        if(_coreTotalLife == 0){
            Debug.Log("YOU LOST!");
        }
    }

    void OnTriggerEnter(Collider other){
        switch(System.Enum.Parse(typeof(GameManager.Tags), other.tag)){
            case GameManager.Tags.Enemy:
                _menaces.Add(other.GetComponent<EnemyBehaviour>());
                break;
        }
    }

    //// Coroutines
    IEnumerator CheckMenacesCoroutine(float secondsToCheckMenace){
        while(true){
            yield return new WaitForSecondsRealtime(secondsToCheckMenace);
            if(_menaces.Count > 0){
                TakeDamageFromManaces();
            }
        }
    }

    //// Private methods
    void TakeDamageFromManaces(){
        _coreTotalLife -= _menaces.Count; // CHECK - Hardcoded damage
        ClearMenaceObjectList();
    }

    void ClearMenaceObjectList(){
        foreach(EnemyBehaviour eb in _menaces){
            eb.EnemyHit();
        }
        _menaces.Clear();
    }
}
