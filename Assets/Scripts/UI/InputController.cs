using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    // Delegation
    public delegate void PassVector3(Vector3 vector);

    // Notification
    PassVector3 mouseListeners;

    // Control variables
    bool _raycastIsPossible = true;
    
    // Game objects references
    Button _wallButtonReference;
    Button _towerButtonReference;
    GameManager _gameManager;

    // Mouse related attributes
    GameObject _mouseArrow;
    Vector3 _mouseRayCastPoint;
    public Vector3 MouseRayCastPoint{
        get { return _mouseRayCastPoint; }
        set {
            _mouseRayCastPoint = value;
            MouseRayCastListeners();
        }
    }

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

        RaycastHit hit = MouseCameraRayCast();

        MouseRayCastPoint = hit.point; 

        if (Input.GetButtonDown("Fire1") && _raycastIsPossible) OnMouseClick(hit);
    }

    //// Private methods
    RaycastHit MouseCameraRayCast(){
        // https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)){
            Debug.DrawRay(hit.point, -ray.direction * 50, Color.black);
        }

        return hit;
    }
    void OnMouseClick(RaycastHit hit){
        Debug.Log("HIT " + hit.transform.name);
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

    void MouseRayCastListeners(){
        if(mouseListeners != null) mouseListeners(MouseRayCastPoint);
    }

    //// Public API
    public void RegisterMouseMovementListener(PassVector3 listenerCallback){
        mouseListeners += listenerCallback;
    }

    //// Event callbacks
    // Buttons pressed
    public void OnClickTowerButton(){
        _gameManager.PlayerInteractionClicked(GameManager.InteractionMode.TowerSelection);
    }

    public void OnClickWallButton(){
        _gameManager.PlayerInteractionClicked(GameManager.InteractionMode.WallSelection);
    }
    // Buttons hovered
    public void OnEnterHover(){
        _raycastIsPossible = false;
    }
    public void OnExitHover(){
        _raycastIsPossible = true;
    }
    // State changed
    public void OnPlayerInteractionChanged(GameManager.InteractionMode newState){
        switch(newState){
            case GameManager.InteractionMode.NoSelection:
                SetToggleButton(false, _wallButtonReference);
                SetToggleButton(false, _towerButtonReference);
                break;

            case GameManager.InteractionMode.TowerSelection:
                SetToggleButton(false, _wallButtonReference);
                SetToggleButton(true, _towerButtonReference);
                break;

            case GameManager.InteractionMode.WallSelection:
                SetToggleButton(true, _wallButtonReference);
                SetToggleButton(false, _towerButtonReference);
                break;
        }
    }
}
