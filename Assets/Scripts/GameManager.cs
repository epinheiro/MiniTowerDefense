using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Enumerators
    public enum Tags {Enemy, Core}

    // Wave related variables
    public readonly int totalWaves = 5;

    int _currentWave = 0;

    public int Wave{
        get { return _currentWave; }
        set {
            _currentWave = value;
            OnWaveNumberChange();
        }
    }

    // Core related variables
    public GameObject _coreGameObject;
    public GameObject Core{
        get { return _coreGameObject; }
    }

    //// MonoBehaviour methods
    void Awake(){
        if(_coreGameObject == null) SetupErrorMessage("Core game object not linked");
    }


    void Update()
    {

    }

    //// Private methods
    void OnWaveNumberChange(){

    }

    void SetupErrorMessage(string message){
        throw new System.Exception(string.Format("GameManager not correctly setup: {0}", message));
    }
}
