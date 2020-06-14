using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public enum Layout {inGame, gameOver} // TODO - new layouts - , constructionConfirmation, constructionDestruction}

    GameObject _overlayUI;
    GameObject _popups;
    GameObject _endGamePopup;

    //// MonoBehaviour
    void Awake(){
        _overlayUI = this.transform.Find("OverlayUI").gameObject;
        _popups = this.transform.Find("Popups").gameObject;
        _endGamePopup = _popups.transform.Find("EndGame").gameObject;

        ChangeLayout(Layout.inGame);
    }

    //// Public API
    public void ChangeLayout(Layout layout){
        switch(layout){
            case Layout.inGame:
                SetActive(_overlayUI, true);
                SetActive(_endGamePopup, false);
                break;

            case Layout.gameOver:
                SetActive(_overlayUI, false);
                SetActive(_endGamePopup, true);
                break;

            default:
                throw new System.Exception(string.Format("Not expected layout {0}", layout));
        }
    }

    //// Private methods
    void SetActive(GameObject gameObject, bool activate){
        if(gameObject.activeSelf != activate){
            gameObject.SetActive(activate);
        }
    }
}
