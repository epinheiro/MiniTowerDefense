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

    void Start()
    {
        _coreReference = GameObject.Find("GameManager").GetComponent<GameManager>().Core; // TODO futurely will be changed to a Init method
        aiAgent.destination = _coreReference.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
