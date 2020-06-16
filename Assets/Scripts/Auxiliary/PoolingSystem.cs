using System.Collections.Generic;
using UnityEngine;

public class PrefabPoolingSystem
{
    // Readonly variables
    readonly Vector3 _initialPosition = new Vector3(0,3,0);
    readonly Quaternion _rotationIdentity = Quaternion.identity;

    // Meta
    static int _iteration = 0;

    // Pool attributes
    List<GameObject> _pool;
    public int Size{
        get { return _pool.Count; }
    }
    int _currentIndex = 0;
    public int Used{
        get { return _currentIndex; }
    }
    public int Available{
        get { return Size - Used; }
    }
    GameObject _poolPrefab;
    Transform _parent;


    //// Public API
    public PrefabPoolingSystem(GameObject prefab, int poolSize, Transform parent){
        this._poolPrefab = prefab;

        this._pool = new List<GameObject>();

        this._parent = parent;
        
        InstantiateNewElements(prefab, poolSize);
    }

    public GameObject GetInstance(){
        if(_currentIndex < Size){
            GameObject go = _pool[_currentIndex];
            _currentIndex++;

            go.name = string.Format("{0}-{1}", _poolPrefab.name, _iteration);
            _iteration++;

            go.SetActive(true);

            return go;
        }else{
            throw new System.Exception(string.Format("PoolingSystem does not have more instantiated objects - current size {0}", Size));
        }
    }

    public void ReturnInstance(GameObject element){
        if(_pool.Remove(element)){
            ResetGameObjectAttributes(element);

            _currentIndex--;
            _pool.Add(element);
        }else{
            if(_pool.Contains(element)){
                throw new System.Exception("PoolingSystem couldn't remove the specified element");
            }else{
                throw new System.Exception("The given element is not responsability of this PoolingSystem");
            }
        }
    }

    public void EnlargePoolSize(int numberOfNewElements){
        InstantiateNewElements(this._poolPrefab, numberOfNewElements);
    }

    public int TryToReleasePoolElements(int numberToRelease){
        int removed = 0;
        for(int i=_pool.Count-1; i>=0 ; i--){
            GameObject go = _pool[i];
            if(!go.activeSelf){
                if(_pool.Remove(go)){
                    removed++;
                    if(removed == numberToRelease) break;
                }
            }
        }
        return removed;
    }

    public override string ToString(){
        string message = string.Format("[{0}] ", _pool.Count);;
        foreach(GameObject go in _pool){
            message += string.Format("{0}({1}); ", go.name, go.activeSelf ? "X" : " ");
        }
        return message;
    }

    public List<GameObject> GetAllElementsActive(){
        List<GameObject> allActive = new List<GameObject>();
        for(int i=0 ; i<Size; i++){
            GameObject element = _pool[i];
            if(element.activeSelf){
                allActive.Add(element);
            }else{
                break;
            }
        }
        return allActive;
    }

    //// Private methods
    void InstantiateNewElements(GameObject prefab, int numberOfNewElements){
        for(int i=0; i<numberOfNewElements; i++){
            GameObject go = MonoBehaviour.Instantiate(prefab, _initialPosition, _rotationIdentity);
            go.SetActive(false);

            _pool.Add(go);

            if(_parent != null){
                Transform transform = go.transform;
                transform.SetParent(_parent);
                transform.localPosition = _initialPosition;
                transform.localRotation = _rotationIdentity;
            }
        }
    }

    void ResetGameObjectAttributes(GameObject go){
        go.SetActive(false);
        go.transform.position = _initialPosition;
        go.transform.rotation = _rotationIdentity;
    }
}
