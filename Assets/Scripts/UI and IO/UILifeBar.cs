using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILifeBar : MonoBehaviour
{
    Camera _camera;
    Slider _bar;

    //// MonoBehaviour
    void Awake(){
        _bar = this.transform.Find("Slider").GetComponent<Slider>();
        _camera = Camera.main;
    }

    void Update(){
        this.transform.LookAt(_camera.transform);
    }

    //// Public API
    public void SetUp(int maxLife){
        _bar.maxValue = maxLife;
        _bar.value = maxLife;
    }

    public void ChangeValueIn(int valueToChange){
        _bar.value += valueToChange;
    }

    public void HardSetValue(int newValue){
        _bar.value = newValue;
    }

}
