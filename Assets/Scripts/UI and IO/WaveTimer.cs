using UnityEngine;
using UnityEngine.UI;

public class WaveTimer : MonoBehaviour
{
    // Game Manager
    GameManager _gameManager;

    // Attributes
    Slider _bar;

    //// MonoBehaviour
    void Awake(){
        _gameManager = GameManager.Instance;
        _bar = this.GetComponent<Slider>();
    }

    void Update(){
        if(_gameManager.State == GameManager.GameState.InGame){
            if(_bar.value >= 0){
                ChangeValueIn(-Time.deltaTime);
            }else{
                ChangeValueIn(0);
            }
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