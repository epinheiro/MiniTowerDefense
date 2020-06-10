using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Control variables
    bool _raycastIsPossible = true;
    
    // Game objects references
    Button _wallButtonReference;
    Button _towerButtonReference;
    GameManager _gameManager;

    // Mouse related attributes
    GameObject _mouseArrow;

    //// MonoBehaviour methods
    void Awake(){
        _mouseArrow = this.transform.Find("MouseArrow").gameObject;

        Transform buttonGroup = this.transform.Find("OverlayUI").Find("Buttons");
        _wallButtonReference = buttonGroup.Find("UIButton_Wall").GetComponent<Button>();
        _towerButtonReference = buttonGroup.Find("UIButton_Tower").GetComponent<Button>();
    }

    void Start(){
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // TODO futurely will be changed to a Init method
    }

    void FixedUpdate(){
        _mouseArrow.transform.position = Input.mousePosition;
    }

    void Update(){
        if (Input.GetButtonDown("Fire1") && _raycastIsPossible) OnMouseClick();
    }

    //// Private methods
    void OnMouseClick(){
        // https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)){
            Debug.DrawRay(transform.position, ray.direction * 100, Color.green);
            Debug.Log("HIT " + hit.transform.name);
        }
    }

    void SetToggleButton(bool isClicked, Button button){        
        if(isClicked){
            ChangeButtonColor(button, Color.grey);
        }else{
            ChangeButtonColor(button, Color.white);
        }
    }

    void ChangeButtonColor(Button button, Color newColor){
        var colors = button.colors;
        colors.normalColor = newColor;
        colors.highlightedColor = newColor;
        button.colors = colors;
    }

    //// Event callbacks
    // Buttons pressed
    public void OnClickTowerButton(){
        _gameManager.PlayerInteractionClicked(GameManager.PlayerInteraction.TowerSelection);
    }

    public void OnClickWallButton(){
        _gameManager.PlayerInteractionClicked(GameManager.PlayerInteraction.WallSelection);
    }
    // Buttons hovered
    public void OnEnterHover(){
        _raycastIsPossible = false;
    }
    public void OnExitHover(){
        _raycastIsPossible = true;
    }
    // State changed
    public void OnPlayerInteractionChanged(GameManager.PlayerInteraction newState){
        switch(newState){
            case GameManager.PlayerInteraction.NoSelection:
                SetToggleButton(false, _wallButtonReference);
                SetToggleButton(false, _towerButtonReference);
                break;

            case GameManager.PlayerInteraction.TowerSelection:
                SetToggleButton(false, _wallButtonReference);
                SetToggleButton(true, _towerButtonReference);
                break;

            case GameManager.PlayerInteraction.WallSelection:
                SetToggleButton(true, _wallButtonReference);
                SetToggleButton(false, _towerButtonReference);
                break;
        }
    }
}
