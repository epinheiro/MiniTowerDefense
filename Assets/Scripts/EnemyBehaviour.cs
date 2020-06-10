using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    GameObject _coreReference;
    NavMeshAgent aiAgent;


    void Awake(){
        aiAgent = this.GetComponent<NavMeshAgent>();
    }

    //// MonoBehaviour methods
    void Start()
    {
        _coreReference = GameObject.Find("GameManager").GetComponent<GameManager>().Core; // TODO futurely will be changed to a Init method
        ChangeAgentDestination(_coreReference.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        switch(System.Enum.Parse(typeof(GameManager.Tags), other.tag)){
            case GameManager.Tags.Core:
                ChangeAgentDestination(this.transform.position);
                EnemyLookAt(_coreReference.transform);
                break;
        }
    }


    //// Private methods
    void ChangeAgentDestination(Vector3 goToPosition){
        aiAgent.destination = goToPosition; 
    }

    void EnemyLookAt(Transform target){
        this.transform.LookAt(target);
    }
}
