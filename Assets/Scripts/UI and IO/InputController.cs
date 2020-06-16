using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    // Meta
    GameManager _gameManager;

    // Delegation
    public delegate void PassVector3(Vector3 vector);

    // Notification
    PassVector3 _mouseMovementListeners;
    PassVector3 _mouseClickListeners;

    // Control variables
    bool _raycastIsPossible = true;
    
    //// UI game objects references
    // Main UI
    GameObject _overlayUI;
    WaveTimer _waveTimer;
    // Wall button 
    Button _wallButtonReference;
    // tower button
    Button _towerButtonReference;
    // End game popup
    GameObject _endGamePopup;
    Text _endGameText;
    Text _endGameButtonText;
    // Construction/Destruction popup
    GameObject _constructionPopup;
    Text _constructionText;
    Button _constructionButton1Reference;
    Text _constructionButton1Text;
    Button _constructionButton2Reference;
    Text _constructionButton2Text;

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

        _overlayUI = this.transform.Find("OverlayUI").gameObject;

        Transform buttonGroup = _overlayUI.transform.Find("Buttons");
        _wallButtonReference = buttonGroup.Find("UIButton_Wall").GetComponent<Button>();
        _towerButtonReference = buttonGroup.Find("UIButton_Tower").GetComponent<Button>();

        GameObject _popups = this.transform.Find("Popups").gameObject;
        _endGamePopup = _popups.transform.Find("EndGame").gameObject;

        _constructionPopup = _popups.transform.Find("Construction").gameObject;
        _constructionText = _constructionPopup.transform.Find("Text").GetComponent<Text>();
        Transform buttonsGroup = _constructionPopup.transform.Find("Buttons");
        Transform button1 = buttonsGroup.GetChild(0);
        _constructionButton1Reference = button1.GetComponent<Button>();
        _constructionButton1Text = button1.Find("Text").GetComponent<Text>();
        Transform button2 = buttonsGroup.GetChild(1);
        _constructionButton2Reference = button2.GetComponent<Button>();
        _constructionButton2Text =  button2.Find("Text").GetComponent<Text>();

        _endGameText = _endGamePopup.transform.Find("EndGameText").GetComponent<Text>();
        _endGameButtonText = _endGamePopup.transform.Find("Button").Find("Text").GetComponent<Text>();

        _waveTimer = _overlayUI.transform.Find("Slider").GetComponent<WaveTimer>();

        SetInGameLayout();
    }

    void Start(){
        _gameManager = GameManager.Instance;
    }

    void FixedUpdate(){
        _mouseArrow.transform.position = Input.mousePosition;

        RaycastHit? hit = MouseCameraRayCast();

        if(hit.HasValue){
            MouseRayCastPoint = hit.Value.point;
        }

        if (Input.GetButtonDown("Fire1") && hit.HasValue && _raycastIsPossible) OnMouseClick(hit.Value);
    }

    //// Private methods
    RaycastHit? MouseCameraRayCast(){
        // Adapted from https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit? hitResult = null;
        
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach(RaycastHit hit in hits){
            Debug.DrawRay(hit.point, -ray.direction * 50, Color.black);

            GameManager.Tags objectTag = (GameManager.Tags) System.Enum.Parse(typeof(GameManager.Tags), hit.collider.tag);
            switch(objectTag){
                case GameManager.Tags.Ground:
                    hitResult = hit;
                    break;

                case GameManager.Tags.Blueprint: // Do nothing
                    break;

                case GameManager.Tags.Construction:
                case GameManager.Tags.Enemy:
                case GameManager.Tags.Core:
                    return null;
            }
        }
        return hitResult;
    }
    void OnMouseClick(RaycastHit hit){
        _mouseClickListeners(hit.point);
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
        if(_mouseMovementListeners != null) _mouseMovementListeners(MouseRayCastPoint);
    }

    //// Public API
    public void RegisterMouseMovementListener(PassVector3 listenerCallback){
        _mouseMovementListeners += listenerCallback;
    }

    public void RegisterMouseClickListener(PassVector3 listenerCallback){
        _mouseClickListeners += listenerCallback;
    }

    // Layout change
    public void SetInGameLayout(){
        SetUIElementActive(_overlayUI, true);
        SetUIElementActive(_endGamePopup, false);
        SetUIElementActive(_constructionPopup, false);
    }
    public void SetEndGameLayout(string text, string buttonText){
        SetUIElementActive(_overlayUI, false);
        SetUIElementActive(_endGamePopup, true);
        SetUIElementActive(_constructionPopup, false);

        _endGameText.text = text;
        _endGameButtonText.text = buttonText;
    }
    public void SetConstructionPopupLayout(string mainText, string button1Text, UnityAction button1Callback, string button2Text, UnityAction button2Callback){
        SetUIElementActive(_overlayUI, true);
        SetUIElementActive(_endGamePopup, false);
        SetUIElementActive(_constructionPopup, true);

        _constructionText.text = mainText;
        _constructionButton1Reference.onClick.RemoveAllListeners();
        _constructionButton1Reference.onClick.AddListener(button1Callback);
        _constructionButton1Text.text = button1Text;
        _constructionButton2Reference.onClick.RemoveAllListeners();
        _constructionButton2Reference.onClick.AddListener(button2Callback);
        _constructionButton2Text.text = button2Text;
    }

    public void SetVisibilityWallButton(bool isVisible){
        _wallButtonReference.gameObject.SetActive(isVisible);
    }

    public void SetVisibilityTowerButton(bool isVisible){
        _towerButtonReference.gameObject.SetActive(isVisible);
    }

    public void SetWaveTimer(float seconds){
        _waveTimer.SetUp(seconds);
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

            case GameManager.InteractionMode.ConstructionConfirmation:
                SetToggleButton(false, _wallButtonReference);
                SetToggleButton(false, _towerButtonReference);
                break;
        }
    }

    //// Private methods
    void SetUIElementActive(GameObject gameObject, bool activate){
        if(gameObject.activeSelf != activate){
            gameObject.SetActive(activate);
        }
    }
}
