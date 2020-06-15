using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimer : MonoBehaviour
{
    Slider _bar;

    //// MonoBehaviour
    void Awake(){
        _bar = this.GetComponent<Slider>();
    }

    void Update(){
        if(_bar.value >= 0){
            ChangeValueIn(-Time.deltaTime);
        }else{
            ChangeValueIn(0);
        }
    }

    //// Public API
    public void SetUp(float max){
        _bar.maxValue = max;
        _bar.value = max;
    }

    // Private methods
    void ChangeValueIn(float valueToChange){
        _bar.value += valueToChange;
    }
}