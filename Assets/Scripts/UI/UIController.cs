using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Mouse related attributes
    GameObject _mouseArrow;

    //// MonoBehaviour methods
    void Awake(){
        _mouseArrow = this.transform.Find("MouseArrow").gameObject;
    }

    void Start(){
    }

    void FixedUpdate(){
        _mouseArrow.transform.position = Input.mousePosition;
    }

    void Update(){
        if (Input.GetButtonDown("Fire1")) OnMouseClick();
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
}
