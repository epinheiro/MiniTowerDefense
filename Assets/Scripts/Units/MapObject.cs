using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    GameObject _core;
    GameObject _spawnPoints;
    public GameObject Core{
        get { return _core; }
    }
    public GameObject SpawnPoints{
        get { return _spawnPoints; }
    }

    void Awake(){
        _core = this.transform.Find("Core").gameObject;
        _spawnPoints = this.transform.Find("SpawnPoints").gameObject;
    }


}
