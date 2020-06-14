using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBehaviour : MonoBehaviour
{
    // Balance variables
    float _secondsToCheckMenace = 2; // CHECK - hardcoded check period

    // Core gameplay attributes
    int _coreTotalLife = 30; // CHECK - hardcoded life
    int _currentLife;
    int CurrentLife{
        get { return _currentLife; }
        set {
            _currentLife = value;
            _lifeBar.HardSetValue(_currentLife);
        }
    }
    UILifeBar _lifeBar;

    
    // Enemy related attributes
    List<EnemyBehaviour> _menaces;

    //// MonoBehaviour methods
    void Awake(){
        _menaces = new List<EnemyBehaviour>();
        _lifeBar = this.transform.Find("LifeBar").GetComponent<UILifeBar>();
        _lifeBar.SetUp(_coreTotalLife);

        _currentLife = _coreTotalLife;
    }

    void Start()
    {
        this.StartCoroutine(CheckMenacesCoroutine(_secondsToCheckMenace));
    }

    void Update()
    {
        if(CurrentLife <= 0){
            Debug.Log("YOU LOST!"); // TODO - insert end game screen
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
        CurrentLife -= ClearMenaceList(); // CHECK - Hardcoded damage (1 to each enemy life left)
    }

    int ClearMenaceList(){
        int purgedEnemyLifeLeft = 0;

        foreach(EnemyBehaviour eb in _menaces){
            purgedEnemyLifeLeft += eb.EraseEnemy();
        }

        _menaces.Clear();

        return purgedEnemyLifeLeft;
    }
}
